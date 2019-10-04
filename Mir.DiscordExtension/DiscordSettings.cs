namespace Mir.DiscordExtension
{
    public class DiscordSettings
    {
        public bool DisplayCharacterName { get; set; } = true;
        public string DetailsFormatting { get; set; } = "{0}{1}{2}";
        public bool EnableDiscordActivity { get; set; } = true;
        public string LargeImage { get; set; } = string.Empty;
        public string SmallImage { get; set; } = string.Empty;
        public bool ShowGroup { get; set; } = true;

        public DiscordSettings()
        {
            
        }
    }
}
