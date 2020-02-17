namespace MobileDevApp.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int ColourSchemeId { get; set; }
        public ColourScheme ColourScheme { get; set; }
    }
}
