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
            discord.Started += DiscordOnStartedActivity;
            discord.HasException += DiscordOnHasException;
            discord.Stopped += DiscordOnStopped;
            discord.StartApp();
            Assert.AreEqual(true, discord.Running);

            var expireTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            while (!HasHit && DateTime.UtcNow < expireTime)
            {
                
            }
            Assert.AreEqual(true, HasHit);
        }
        [TestMethod]
        public void TestDiscordActivity()
        {
            var discord = DiscordsApp.GetApp();
            discord.ClientId = 2312323123123123123;
            discord.StartFailure += DiscordOnStartFailure;
            discord.Started += DiscordOnStartedActivity;
            discord.HasException += DiscordOnHasException;
            discord.Stopped += DiscordOnStopped;
            discord.ActivityCallBack += DiscordOnActivityCallBack;
            discord.StartApp();
            //Initialize a Mock of playing in a group see Mock Activity for options.
            MockActivity.SetMockGroupActivity(discord);
            //Send to update Queue
            discord.UpdateActivity();
            Assert.AreEqual(true, HasHit);
            var expireTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            HasHit = false;
            //Create a loop to process the call-backs until there is a response (or it reaches the expire time)
            while (!HasHit && DateTime.UtcNow < expireTime)
            {
            }

            Assert.AreEqual(true, HasHit);
        }

        private void DiscordOnStartedActivity(object sender, EventArgs e)
        {
            HasHit = true;
            DiscordsApp.GetApp().StartLoop();
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
            HasHit = true;
        }

        private void DiscordOnStartFailure(object sender, EventArgs e)
        {
            Console.WriteLine($"Discord Start failed with {(byte)sender}");
        }
    }
}
