using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Planetary_Explorers.Gui;
using SFML.Graphics;

namespace Planetary_Explorers
{
    class GameManager : IUpdateable, IDrawable
    {
        private List<Display> allPageDisplays; 
        /// <summary>
        /// Internal. Do not set this variable! Use the public property
        /// </summary>
        private Display activeDisplayRoot;
        public Display ActiveDisplayRoot
        {
            get { return activeDisplayRoot; }
            set
            {
                if (ActiveDisplayRoot != null)
                    ActiveDisplayRoot.EventSubscribe(false,window);
                if (!allPageDisplays.Contains(value))
                { allPageDisplays.Add(value); }
                value.EventSubscribe(true, window);

                activeDisplayRoot = value;
            }
        }

        private RenderWindow window;
        public static RenderWindow ActiveWindow { get; private set; }
        private FontManager FontMgr;

        public GameManager(RenderWindow window, Display firstDisplay)
        {
            this.window = window;
            ActiveWindow = window;
            allPageDisplays = new List<Display>();
            ActiveDisplayRoot = firstDisplay;

            FontMgr = new FontManager(@"Gui/OpenSans.ttf");
        }

        public void Update()
        {
            ActiveDisplayRoot.Update();
        }

        public void Draw(RenderTexture texOut)
        {
            // No target or source voodoo going on here
            // Just passing the rendertexture to the correct display.
            ActiveDisplayRoot.Draw(texOut);
        }
    }
}
