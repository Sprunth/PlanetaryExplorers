using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.Gui
{
    class FontManager
    {
        public static FontManager ActiveFontManager { get; private set; }

        private List<Font> allFonts;

        public Font this[int i] { get { return allFonts[i]; } }

        public FontManager(string defaultFontPath)
        {
            allFonts = new List<Font>();
            var defaultFont = new Font(defaultFontPath);
            allFonts.Add(defaultFont);

            ActiveFontManager = this;
        }

        
    }
}
