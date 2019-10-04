using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mir.DiscordExtension;
using Mir.DiscordExtension.SDK;

namespace Mir.DiscordTests
{
    [TestClass]
    public class MainDiscordTests
    {
        [TestMethod]
        public void TestStartDiscord()
        {
            var discord = DiscordsApp.GetApp();
            discord.ClientId = 2312323123123123123;
            discord.StartFailure += DiscordOnStartFailure;
            discord.Started += DiscordOnStarted;
            discord.HasException += DiscordOnHasException;
            discord.Stopped += DiscordOnStopped;
            discord.StartApp();
        }
        [TestMethod]
        public void TestDiscordActivity()
        {
            var discord = DiscordsApp.GetApp();
            discord.ClientId = 2312323123123123123;
            discord.StartFailure += DiscordOnStartFailure;
            discord.Started += DiscordOnStarted;
            discord.HasException += DiscordOnHasException;
            discord.Stopped += DiscordOnStopped;
            discord.ActivityCallBack += DiscordOnActivityCallBack;
            discord.StartApp();
            discord.UpdateStage(StatusType.Details, "Testing Activity");
            discord.UpdateStage(StatusType.GameState, GameState.PlayingGroup);
            discord.UpdateStage(StatusType.PlayerCount, 1);
            discord.UpdateStage(StatusType.Party, 1, 5);
            discord.UpdateActivity();
            var expireTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            while (!HasHit && DateTime.UtcNow < expireTime)
            {
                discord.Update();
            }

            Assert.AreEqual(true, HasHit);
        }

        public bool HasHit { get; set; }

        private void DiscordOnActivityCallBack(object sender, EventArgs e)
        {
            Console.WriteLine($"Call back Received ({(Result)sender})");
            HasHit = true;
        }

        private void DiscordOnStopped(object sender, EventArgs e)
        {
            Console.WriteLine("Discord Stopped");
        }

        private void DiscordOnHasException(object sender, EventArgs e)
        {
            Console.WriteLine(((Exception) sender));
        }

        private void DiscordOnStarted(object sender, EventArgs e)
        {
            Console.WriteLine("Discord Started");
        }

        private void DiscordOnStartFailure(object sender, EventArgs e)
        {
            Console.WriteLine($"Discord Start failed with {(byte)sender}");
        }
    }
}
