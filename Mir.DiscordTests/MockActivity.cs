using Mir.DiscordExtension;

namespace Mir.DiscordTests
{
    public static class MockActivity
    {

        public static void SetMockGroupActivity(DiscordsApp discord)
        {
            //In a group
            discord.UpdateStage(StatusType.GameState, GameState.PlayingGroup);
            //User Count
            discord.UpdateStage(StatusType.PlayerCount, 1);
            //Group count (current, max)
            discord.UpdateStage(StatusType.Party, 1, 5);
            //Players Level
            discord.UpdateStage(StatusType.PlayerLevel, 255);
            //Players Name
            discord.UpdateStage(StatusType.PlayerName, "TestPlayer");
        }

        public static void SetMockSoloActivity(DiscordsApp discord)
        {
            //In a group
            discord.UpdateStage(StatusType.GameState, GameState.Playing);
            //User Count
            discord.UpdateStage(StatusType.PlayerCount, 23);
            //Players Level
            discord.UpdateStage(StatusType.PlayerLevel, 12);
            //Players Name
            discord.UpdateStage(StatusType.PlayerName, "TestPlayer");
        }
    }
}
