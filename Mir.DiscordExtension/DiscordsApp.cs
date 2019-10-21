using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mir.DiscordExtension.Properties;
using Mir.DiscordExtension.SDK;
using Newtonsoft.Json;

namespace Mir.DiscordExtension
{
    public class DiscordsApp
    {
        public bool Running { get; private set; }

        private bool _stop;

        public bool Stop
        {
            get => _stop;
            set
            {
                if (_stop == value)
                    return;
                _stop = value;
                if (_stop)
                    StopApp();
            }
        }

        private static readonly DiscordsApp Instance = new DiscordsApp();
        public static DiscordsApp GetApp() => Instance;

        private Discord _discord;

        private ActivityManager _activityManager;

        private GameStage _currentState;

        private readonly DiscordSettings _settings;
        private const string SettingsFile = @".\DiscordSettings.json";

        private long _clientId;

        public long ClientId
        {
            get => _clientId;
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_clientId == value) return;
                _clientId = value;
            }
        }

        private Activity _activity;
        private DateTime _startTime;
        private ActivityTimestamps _stamp;
        private ActivityParty _party;
        private PartySize _partySize;
        private ActivityAssets _assets;
        private DiscordsApp()
        {
            if (!File.Exists(SettingsFile))
            {
                _settings = new DiscordSettings();
                var output = JsonConvert.SerializeObject(_settings, Formatting.Indented);
                File.WriteAllText(SettingsFile, output);
            }
            else
            {
                var input = File.ReadAllText(SettingsFile);
                _settings = JsonConvert.DeserializeObject<DiscordSettings>(input);
            }   
        }
        /// <summary>
        /// Use the Status type to help the updater identify the incoming parameters
        /// </summary>
        /// <param name="statusType">Status Type</param>
        /// <param name="inputs">PlayerCount:int Details:string Party:int,int SmallImageText:string LargeImageText:string</param>
        /// <example>
        /// This sample shows how to call the <see cref="UpdateStage"/> method.
        /// <code>
        /// UpdateStage(StatusType.Party, 1, 5);
        /// </code>
        /// </example>
        public void UpdateStage(StatusType statusType, params object[] inputs)
        {
            switch (statusType)
            {
                case StatusType.PlayerCount:
                    if (!(inputs[0] is int)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(int)}");
                    _currentState.UpdatePlayerCount((int)inputs[0]);
                    break;
                case StatusType.GameState:
                    if (!(inputs[0] is int)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(GameState)}");
                    _currentState.UpdateState((GameState)inputs[0]);
                    break;
                case StatusType.Party:
                    if (!(inputs[0] is int)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(int)}");
                    if (!(inputs[1] is int)) throw new ArgumentException($"Invalid cast, {inputs[1].GetType()} to {typeof(int)}");
                    _currentState.UpdateParty((int)inputs[0], (int)inputs[1]);
                    _partySize.CurrentSize = _currentState.CurrentPartyCount;
                    _partySize.MaxSize = _currentState.MaxPartyCount;
                    break;
                case StatusType.PlayerName:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdatePlayerName((string)inputs[0]);
                    break;
                case StatusType.PlayerLevel:
                    if (!(inputs[0] is int)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(int)}");
                    _currentState.UpdatePlayerLevel((int)inputs[0]);
                    break;
                case StatusType.PlayerClass:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdatePlayersClass((string)inputs[0]);
                    break;
                case StatusType.SmallImage:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdateSmallImage((string)inputs[0]);
                    _assets.SmallImage = (string) inputs[0];
                    break;
                case StatusType.SmallImageText:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdateSmallImageText((string)inputs[0]);
                    _assets.SmallText = _currentState.SmallImageText;
                    break;
                case StatusType.LargeImage:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdateLargeImage((string)inputs[0]);
                    _assets.LargeImage = (string) inputs[0];
                    break;
                case StatusType.LargeImageText:
                    if (!(inputs[0] is string)) throw new ArgumentException($"Invalid cast, {inputs[0].GetType()} to {typeof(string)}");
                    _currentState.UpdateLargeImageText((string)inputs[0]);
                    _assets.LargeText = _currentState.LargeImageText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusType), statusType, Resources.DiscordsApp_UpdateStage_Invalid_status_Type);
            }
        }
        /// <summary>
        /// Calling this will queue up the Activity of the User, Ensure <see cref="Update"/> is called or activity will not be updated.
        /// It is advised to place <see cref="Update"/> within a continuous loop in order for the call backs to process the update.
        /// </summary>
        public void UpdateActivity()
        {
            try
            {
                if (_currentState.State == GameState.Playing ||
                    _currentState.State == GameState.PlayingGroup)
                {
                    _activity.Details = string.Format(_settings.DetailsFormatting,
                    !string.IsNullOrEmpty(_currentState.PlayersName) && _settings.DisplayCharacterName ? 
                        _currentState.PlayersName + " " : "",//0
                    _currentState.PlayerLevel != -1 ? 
                        $"Level: {_currentState.PlayerLevel} " : "",//1
                    _currentState.PlayerCount != -1 ? 
                        $"UC:{_currentState.PlayerCount}" : "");//2
                }
                else
                {
                    _activity.Details = "";
                }
                SetState();

                SetAssets();

                
                _activityManager.UpdateActivity(_activity, result =>
                {
                    if (result != Result.Ok)
                    {
#if DEBUG
                        Console.WriteLine(result.ToString());
#endif
                    }
                    OnActivityCallBack(result);
                });
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnException(e);
            }
        }
        private void SetState()
        {
            try
            {
                if (_settings.ShowGroup &&
                    _currentState.State == GameState.PlayingGroup)
                {
                    _activity.State = _currentState.State.ToString();
                    _party.Size = _partySize;
                    _activity.Party = _party;
                }
                else if (!_settings.ShowGroup &&
                         _currentState.State == GameState.PlayingGroup)
                {
                    _activity.State = GameState.Playing + " Solo";
                    _activity.Party = default;
                }
                else if (_currentState.State == GameState.Playing)
                {
                    _activity.State = _currentState.State + " Solo";
                    _activity.Party = default;
                }
                else
                {
                    _activity.State = _currentState.State.ToString();
                    _activity.Party = default;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnException(e);
            }
        }
        private void SetAssets()
        {
            try
            {
                if (!string.IsNullOrEmpty(_assets.LargeImage) &&
                    !string.Equals(_activity.Assets.LargeImage, _assets.LargeImage, StringComparison.CurrentCultureIgnoreCase))
                    _activity.Assets.LargeImage = _assets.LargeImage;
                if (!string.IsNullOrEmpty(_assets.SmallImage) &&
                    !string.Equals(_activity.Assets.SmallImage, _assets.SmallImage,
                        StringComparison.CurrentCultureIgnoreCase))
                    _activity.Assets.SmallImage = _assets.SmallImage;
                if (!string.IsNullOrEmpty(_assets.SmallText) &&
                    !string.Equals(_activity.Assets.SmallText, _assets.SmallText,
                        StringComparison.CurrentCultureIgnoreCase))
                    _activity.Assets.SmallText = _assets.SmallText;
                if (!string.IsNullOrEmpty(_assets.LargeText) &&
                    !string.Equals(_activity.Assets.LargeText, _assets.LargeText,
                        StringComparison.CurrentCultureIgnoreCase))
                    _activity.Assets.LargeText = _assets.LargeText;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnException(e);
            }
        }
        /// <summary>
        /// Calling this will create a new session. Creates a new instance of <see cref="Discord"/>
        /// Requires the <value>ClientId</value> to be set.
        /// </summary>
        public void StartApp()
        {
            if (!_settings.EnableDiscordActivity) return;
            if (!CanLaunch())
            {
                OnStartFailure(0);
                return;
            }

            if (ClientId <= 0)
            {
                OnStartFailure(7);
                return;
            }
            byte reason = 1;
            try
            {
                
                _currentState = new GameStage();
                _discord = new Discord(ClientId, (ulong)CreateFlags.NoRequireDiscord);
                reason++;
                _activityManager = _discord.GetActivityManager();
                reason++;
                _startTime = DateTime.UtcNow;
                _stamp = new ActivityTimestamps
                {
                    Start = (long)_startTime.ToUniversalTime().Subtract(
                        new DateTime(1970, 1,1,0,0,0, DateTimeKind.Utc)).TotalMilliseconds
                };
                reason++;
                _partySize = new PartySize
                {
                    CurrentSize = 0, MaxSize = 0
                };
                reason++;
                _party = new ActivityParty
                {
                    Id = Guid.NewGuid().ToString(),
                    Size = _partySize
                };
                reason++;
                if (!string.IsNullOrEmpty(_settings.LargeImage) ||
                    !string.IsNullOrEmpty(_settings.SmallImage))
                {
                    if (!string.IsNullOrEmpty(_settings.LargeImage) &&
                        !string.IsNullOrEmpty(_settings.SmallImage))
                    {
                        _assets = new ActivityAssets
                        {
                            LargeImage = _settings.LargeImage,
                            SmallImage = _settings.SmallImage
                        };
                    }
                    else if (!string.IsNullOrEmpty(_settings.LargeImage))
                    {
                        _assets = new ActivityAssets
                        {
                            LargeImage = _settings.LargeImage
                        };
                    }
                    else if (!string.IsNullOrEmpty(_settings.SmallImage))
                    {
                        _assets = new ActivityAssets
                        {
                            SmallImage = _settings.SmallImage
                        };
                    }
                }
                reason++;
                _activity.Timestamps = _stamp;
                _activity.Assets = _assets;
                _activity.Party = _party;
                Running = true;
                OnStarted();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnStartFailure(reason);
                StopApp();
                OnException(e);
            }
        }
        private void StopApp()
        {
            Running = false;
            _activityManager = null;
            _discord?.Dispose();
            _discord = null;
            _currentState = new GameStage();
            _stamp = default;
            _party = default;
            _partySize = default;
            _assets = default;
            OnStopped();
        }
        /// <summary>
        /// Call this within a lifetime loop to ensure the Activity is updated.
        /// </summary>
        private void Update()
        {
            if (!Running || _discord == null) return;
            try
            {
                _discord?.RunCallbacks();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnException(e);
            }
        }
        private bool CanLaunch() => 
            Process.GetProcessesByName("Discord").Length > 0;
        public void StartLoop()
        {
            try
            {
                Task.Run(() =>
                {
                    while (Running)
                    {
                        Update();
                        Thread.Sleep(500);
                    }
                });
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                OnException(e);
            }
        }
        /// <summary>
        /// 0: Discord not found | 1: Failed to create <see cref="Discord"/> instance | 2:Failed to get Activity Manager | 3: Failed to create <see cref="ActivityTimestamps"/> |
        /// 4: Failed to create <see cref="PartySize"/> | 5: Failed to create <see cref="ActivityParty"/> | 6: Failed to create <see cref="ActivityAssets"/>
        /// 7: ClientId not provided
        /// </summary>
        public event EventHandler<EventArgs> StartFailure;
        protected void OnStartFailure(byte reason) =>
            StartFailure?.Invoke(reason, EventArgs.Empty);
        public event EventHandler<EventArgs> Started;
        protected void OnStarted() =>
            Started?.Invoke(this, EventArgs.Empty);
        public event EventHandler<EventArgs> Stopped;
        protected void OnStopped() =>
            Stopped?.Invoke(this, EventArgs.Empty);
        public event EventHandler<EventArgs> ActivityCallBack;
        protected void OnActivityCallBack(Result result) =>
            ActivityCallBack?.Invoke(result, EventArgs.Empty);
        public event EventHandler<EventArgs> HasException;
        protected void OnException(Exception ex) =>
            HasException?.Invoke(ex, EventArgs.Empty);
    }
}
