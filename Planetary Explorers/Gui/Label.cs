using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Planetary_Explorers.Gui
{
    class Label : Display
    {
        public string Contents { get { return text.DisplayedString; } protected set { text.DisplayedString = value; } }
        private Text text;
        private RectangleShape background;
        private FontManager fontMgr;

        public Label(string contents, FontManager fontManager, Vector2u displaySize) : base(displaySize)
        {
            fontMgr = fontManager;

            text = new Text(contents, fontManager[0], 18);
            text.FillColor = Color.Red;
            
            var bounds = text.GetLocalBounds();
            background = new RectangleShape(new Vector2f(text.FindCharacterPos((uint)text.DisplayedString.Length).X, bounds.Height));
            background.Position = new Vector2f(bounds.Left, bounds.Top);
            background.FillColor = Color.Blue;
            

            AddItemToDraw(text, 5);
            AddItemToDraw(background, 3);
        }

        public static void CalculateDisplaySize(Label l)
        {
            
        }
    }
}
