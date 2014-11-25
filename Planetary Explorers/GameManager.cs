using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
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

        public GameManager(RenderWindow window, Display firstDisplay)
        {
            this.window = window;
            allPageDisplays = new List<Display>();
            ActiveDisplayRoot = firstDisplay;
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
