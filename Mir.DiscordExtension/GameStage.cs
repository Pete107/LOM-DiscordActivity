namespace Mir.DiscordExtension
{
    public struct GameStage
    {
        private int _playerCount;
        public int PlayerCount
        {
            get => _playerCount;
            set
            {
                if (_playerCount == value)
                    return;
                _playerCount = value;
            }
        }

        private GameState _state;
        public GameState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
            }
        }

        private string _details;
        public string Details
        {
            get => _details;
            set
            {
                if (_details == value) return;
                _details = value;
            }
        }

        private int _currentPartyCount;
        public int CurrentPartyCount
        {
            get => _currentPartyCount;
            set
            {
                if (_currentPartyCount == value) return;
                _currentPartyCount = value;
            }
        }

        private int _maxPartyCount;
        public int MaxPartyCount
        {
            get => _maxPartyCount;
            set
            {
                if (_maxPartyCount == value) return;
                _maxPartyCount = value;
            }
        }

        private string _largeImageText;
        public string LargeImageText
        {
            get => _largeImageText;
            set
            {
                if (_largeImageText == value) return;
                _largeImageText = value;
            }
        }

        private string _smallImageText;
        public string SmallImageText
        {
            get => _smallImageText;
            set
            {
                if (_smallImageText == value) return;
                _smallImageText = value;
            }
        }

        private string _playersName;
        public string PlayersName
        {
            get => _playersName;
            set
            {
                if (_playersName == value) return;
                _playersName = value;
            }
        }

        private int _playersLevel;
        public int PlayerLevel
        {
            get => _playersLevel;
            set
            {
                if (_playersLevel == value) return;
                _playersLevel = value;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="GameStage"/> lives throughout the life time of a <see cref="DiscordsApp"/> session being active.
        /// </summary>
        /// <param name="state"></param>
        public GameStage(GameState state = GameState.None)
        {
            _playerCount = -1;
            _details = string.Empty;
            _currentPartyCount = -1;
            _maxPartyCount = -1;
            _playersLevel = -1;
            _largeImageText = string.Empty;
            _smallImageText = string.Empty;
            _state = state;
            _playersName = string.Empty;
        }
        /// <summary>
        /// Update the Current sessions State
        /// </summary>
        /// <param name="state"></param>
        public void UpdateState(GameState state) =>
            State = state;
        /// <summary>
        /// Update the Current sessions Player Count. (Requires value being sent via server)
        /// </summary>
        /// <param name="playerCount"></param>
        public void UpdatePlayerCount(int playerCount) =>
            PlayerCount = playerCount;
        /// <summary>
        /// Update the Current sessions Party. 
        /// </summary>
        /// <param name="partyCount"></param>
        /// <param name="maxPartyCount"></param>
        public void UpdateParty(int partyCount, int maxPartyCount)
        {
            CurrentPartyCount = partyCount;
            MaxPartyCount = maxPartyCount;
        }

        public void UpdateLargeImageText(string text) => LargeImageText = text;

        public void UpdateSmallImageText(string text) => SmallImageText = text;

        public void UpdatePlayerName(string text) => PlayersName = text;

        public void UpdatePlayerLevel(int val) => PlayerLevel = val;
    }
}