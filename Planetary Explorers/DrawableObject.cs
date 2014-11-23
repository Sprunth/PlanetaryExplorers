//#define ZLEVEL_TEST

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers
{
    class DrawableObject : IUpdateable
    {
        private Display parentDisplay;

        public DrawableObject(Display parentDisplay)
        {
            this.parentDisplay = parentDisplay;

            ZLevelTest();

        }

        protected void AddItemToDraw(Drawable drawable, uint zlevel)
        {
            parentDisplay.AddItemToDraw(drawable, zlevel);
        }

        protected void RemoveItemToDraw(Drawable drawable, uint zlevel)
        {
            parentDisplay.RemoveItemToDraw(drawable, zlevel);   
        }

        [Conditional("ZLEVEL_TEST")]
        private void ZLevelTest()
        {
            var r = new Random();
            for (int i = 0; i < 100; i++)
            {
                var zlevel = (uint)r.Next(25);
                var cs = new CircleShape(16, 48)
                {
                    Position = new Vector2f(40 + r.Next(200), 40 + r.Next(200)),
                    OutlineColor = new Color(20, 20, 20),
                    OutlineThickness = 1,
                    FillColor = new Color((byte)(zlevel * 10), 0, 0) //(byte)(150 + r.Next(80)), (byte)(150 + r.Next(80)))
                };
                AddItemToDraw(cs, zlevel);
            }
        }

        public virtual void Update()
        {
            
        }
    }
}
