namespace Mir.DiscordExtension
{
    public struct GameStage
    {
        public int PlayerCount { get; private set; }
        public GameState State { get; private set; }
        public string Details { get; private set; }
        public int CurrentPartyCount { get; private set; }
        public int MaxPartyCount { get; private set; }
        public string LargeImageText { get; private set; }
        public string SmallImageText { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="GameStage"/> lives throughout the life time of a <see cref="DiscordsApp"/> session being active.
        /// </summary>
        /// <param name="state"></param>
        public GameStage(GameState state = GameState.None)
        {
            PlayerCount = -1;
            CurrentPartyCount = -1;
            MaxPartyCount = -1;
            LargeImageText = string.Empty;
            SmallImageText = string.Empty;
            State = state;
            Details = string.Empty;
        }
        /// <summary>
        /// Update the Current sessions Details
        /// </summary>
        /// <param name="details"></param>
        public void UpdateDetails(string details) => Details = details;
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
    }
}