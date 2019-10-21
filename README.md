# LOM-Discord-Extension
An extension which can be hooked into a Client and output as Activity on Discord.

# Prerequisites
Netonsoft.Json Reference for .NET Framework 4.5.2 (Can be found on NuGet)
	
[Discords Game SDK](https://discordapp.com/developers/docs/game-sdk/sdk-starter-guide)


# Implementation Usage #1.
	1) Get the instance of the Extension -> DiscordsApp.GetApp()
	
	2) Set your ClientID -> DiscordsApp.GetApp().ClientId = 1231231231231231231
  
	3) Start the Extension -> DiscordsApp.StartApp()
	
	4) Start the Update Loop -> DiscordApp.StartLoop()
  
	5) Initialise the State, Example found further below -> DiscordsApp.GetApp().UpdateState(StatusType, parameters)
  
	6) Push the state to the Activity queue -> DiscordsApp.GetApp().UpdateActivity()
	
	7) To Stop the process set Stop to true, this will stop and reset the application.
  
By default the extension will only detect a Discord Process upon launching  so if a user loads Discord post launch of the Client it will not process.
To overcome this issue you may follow the implementation usage above from steps 3 to 6.

# Implementation Usage #2.
	1) Get the instance of the Extension -> DiscordsApp.GetApp()
	
	2) Set your ClientID -> DiscordsApp.GetApp().ClientId = 1231231231231231231
	
	3) Subscribe to the Following Events:
		DiscordsApp.GetApp().StartFailure
		DiscordsApp.GetApp().Started
		DiscordsApp.GetApp().HasException
		DiscordsApp.GetApp().Stopped
		DiscordsApp.GetApp().ActivityCallBack
		
	4) Within the Subscribed event for Started place DiscordsApp.GetApp().StartLoop()
		Example :
		discord.Started += DiscordOnStartedActivity;
	        private void DiscordOnStartedActivity(object sender, EventArgs e) =>
            		DiscordsApp.GetApp().StartLoop();
			
	5) Initialise the State, Example found further below -> DiscordsApp.GetApp().UpdateState(StatusType, parameters)
  
	6) Push the state to the Activity queue -> DiscordsApp.GetApp().UpdateActivity()
	
	7) To Stop the process set Stop to true, this will stop and reset the application.
			
As with the first usage, you will have to follow steps 4 to 6 to re-initilize the application extension.

# The Parameters mentioned in step 5 are as followed.

	DiscordsApp.GetApp().UpdateStage(StatusType.PlayerCount, int)
	DiscordsApp.GetApp().UpdateStage(StatusType.GameState, GameState.None)
	DiscordsApp.GetApp().UpdateStage(StatusType.Party, int, int)
	DiscordsApp.GetApp().UpdateStage(StatusType.PlayerName, string)
	DiscordsApp.GetApp().UpdateStage(StatusType.PlayerLevel, int)
	DiscordsApp.GetApp().UpdateStage(StatusType.PlayerClass, string)
	DiscordsApp.GetApp().UpdateStage(StatusType.SmallImage, string)
	DiscordsApp.GetApp().UpdateStage(StatusType.SmallImageText, string)
	DiscordsApp.GetApp().UpdateStage(StatusType.LargeImage, string)
	DiscordsApp.GetApp().UpdateStage(StatusType.LargeImageText, string)

# If used incorrectly the extension will not work.


Client/User Usage:
Users are able to disable the extension within configuration file (DiscordSettings.json)
The default structure is as follows:

{

	"DisplayCharacterName": true,
	"DetailsFormatting": "{0}{1}{2}",
	"EnableDiscordActivity": true,
	"LargeImage": "",
	"SmallImage": "",
	"ShowGroup": true
	
}

Setting EnableDiscordActivity will prevent the extension from processing. (won't do anything)

# Details Formatting.

{0} is the Users name if they did not opt to hide it.

{1} is the Level of the user

{2} is the User count of the server (if set)
