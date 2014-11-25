using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.Gui
{
    class Label : Display
    {
        public string Contents { get; protected set; }
        private Text text;
        private FontManager fontMgr;

        public Label(string contents, FontManager fontManager, Vector2u displaySize) : base(displaySize)
        {
            Contents = contents;
            fontMgr = fontManager;

            BackgroundColor = new Color(240,240,240);

            text = new Text(contents, fontManager[0], 14);
            text.Color = Color.Red;

            AddItemToDraw(text, 5);
        }

        public static void CalculateDisplaySize(Label l)
        {
            
        }
    }
}
