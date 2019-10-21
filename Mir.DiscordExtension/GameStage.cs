namespace Mir.DiscordExtension
{
    public struct GameStage
    {
        private int _playerCount;
        public int PlayerCount => _playerCount;
        private GameState _state;
        public GameState State => _state;
        private int _currentPartyCount;
        public int CurrentPartyCount => _currentPartyCount;
        private int _maxPartyCount;
        public int MaxPartyCount => _maxPartyCount;
        private string _largeImage;
        public string LargeImage => _largeImage;
        private string _largeImageText;
        public string LargeImageText => _largeImageText;
        private string _smallImage;
        public string SmallImage => _smallImage;
        private string _smallImageText;
        public string SmallImageText => _smallImageText;
        private string _playersName;
        public string PlayersName => _playersName;
        private int _playersLevel;
        public int PlayerLevel => _playersLevel;
        private string _playersClass;
        public string PlayersClass => _playersClass;

        /// <summary>
        /// Creates a new instance of <see cref="GameStage"/> lives throughout the life time of a <see cref="DiscordsApp"/> session being active.
        /// </summary>
        /// <param name="state"></param>
        public GameStage(GameState state = GameState.None)
        {
            _playerCount = -1;
            _currentPartyCount = -1;
            _maxPartyCount = -1;
            _playersLevel = -1;
            _largeImage = string.Empty;
            _largeImageText = string.Empty;
            _smallImage = string.Empty;
            _smallImageText = string.Empty;
            _state = state;
            _playersName = string.Empty;
            _playersClass = string.Empty;
        }
        /// <summary>
        /// Update the Current sessions State
        /// </summary>
        /// <param name="state"></param>
        public void UpdateState(GameState state) =>
            _state = state;
        /// <summary>
        /// Update the Current sessions Player Count. (Requires value being sent via server)
        /// </summary>
        /// <param name="playerCount"></param>
        public void UpdatePlayerCount(int playerCount) =>
            _playerCount = playerCount;
        /// <summary>
        /// Update the Current sessions Party. 
        /// </summary>
        /// <param name="partyCount"></param>
        /// <param name="maxPartyCount"></param>
        public void UpdateParty(int partyCount, int maxPartyCount)
        {
            _currentPartyCount = partyCount;
            _maxPartyCount = maxPartyCount;
        }
        public void UpdateLargeImage(string img) => _largeImage = img;
        public void UpdateLargeImageText(string text) => _largeImageText = text;
        public void UpdateSmallImage(string img) => _smallImage = img;
        public void UpdateSmallImageText(string text) => _smallImageText = text;
        public void UpdatePlayerName(string text) => _playersName = text;
        public void UpdatePlayerLevel(int val) => _playersLevel = val;
        public void UpdatePlayersClass(string val) => _playersClass = val;
    }
}