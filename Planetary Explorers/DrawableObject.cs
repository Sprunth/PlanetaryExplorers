using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers
{
    class DrawableObject
    {
        private Display parentDisplay;

        public DrawableObject(Display parentDisplay)
        {
            this.parentDisplay = parentDisplay;

            var r = new Random();
            for (int i = 0; i < 100; i++)
            {
                var zlevel = (uint) r.Next(25);
                var cs = new CircleShape(16, 48)
                {
                    Position = new Vector2f(40 + r.Next(200), 40 + r.Next(200)),
                    OutlineColor = new Color(20, 20, 20),
                    OutlineThickness = 2,
                    FillColor = new Color((byte)(zlevel*10), 0, 0) //(byte)(150 + r.Next(80)), (byte)(150 + r.Next(80)))
                };
                AddItemToDraw(cs, zlevel);
            }
        }

        private void AddItemToDraw(Drawable drawable, uint zlevel)
        {
            var tup = new Tuple<Drawable, uint>(drawable, zlevel);
            var index = parentDisplay.toDraw.BinarySearch(tup, ZlevelDrawableCompare.Comparer);
            if (index < 0)
                parentDisplay.toDraw.Insert(~index, tup);
            else
                parentDisplay.toDraw.Insert(index, tup);
        }

        private void RemoveItemToDraw(Drawable drawable, uint zlevel)
        {
            // Could be sped up with binary search, or keep an index.
            parentDisplay.toDraw.Remove(new Tuple<Drawable, uint>(drawable, zlevel));
        }
    }
}
