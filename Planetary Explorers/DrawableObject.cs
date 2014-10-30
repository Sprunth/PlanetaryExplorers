using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Planetary_Explorers
{
    class DrawableObject
    {
        /// <summary>
        /// SFML objects to draw
        /// Each contains a z-level for drawing
        /// Do not directly add to this list. Use AddItemToDraw
        /// </summary>
        private List<Tuple<Drawable, uint>> toDraw;
 
        public DrawableObject()
        {
            toDraw = new List<Tuple<Drawable, uint>>();           
        }

        private void AddItemToDraw(Drawable drawable, uint zlevel)
        {
            var tup = new Tuple<Drawable, uint>(drawable, zlevel);
            var index = toDraw.BinarySearch(tup, ZlevelDrawableCompare.Comparer);
            if (index < 0)
                toDraw.Insert(~index, tup);
            else
                toDraw.Insert(index, tup);
        }
    }

    class ZlevelDrawableCompare : IComparer<Tuple<Drawable, uint>>
    {
        /// <summary>
        /// Compares by zlevels, sorts 0 to +infinity.
        /// </summary>
        int IComparer<Tuple<Drawable, uint>>.Compare(Tuple<Drawable, uint> first, Tuple<Drawable, uint> second)
        {
            if (first.Item2 > second.Item2)
                return 1;
            if (first.Item2 < second.Item2)
                return -1;
            return 0;
        }

        public static IComparer<Tuple<Drawable, uint>> Comparer = new ZlevelDrawableCompare();
    }
}
