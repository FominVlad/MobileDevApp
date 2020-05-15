using System.Collections.Generic;

namespace MobileDevApp.Models
{
    public class ColourScheme
    {
        public int Id { get; set; }
        public string SchemeType { get; set; }
        public string HeaderColour { get; set; }
        public string PageColour { get; set; }
        public string TextColour { get; set; }
        public string ButtonColour { get; set; }
    }
}
