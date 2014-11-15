using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers
{
    class SpaceMapDisplay : Display
    {
        private SpaceGrid grid;

        public SpaceMapDisplay(Vector2u displaySize): base(displaySize)
        {
            grid = new SpaceGrid(new Vector2u(2000,2000), displaySize, this);

        }
    }
}
