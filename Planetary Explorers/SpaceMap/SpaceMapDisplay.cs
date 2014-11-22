using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.SpaceMap
{
    class SpaceMapDisplay : Display
    {
        private SpaceGrid grid;

        public SpaceMapDisplay(Vector2u displaySize): base(displaySize)
        {
            grid = new SpaceGrid(new Vector2u(2000,2000), displaySize, this);
            toUpdate.Add(grid);

            var planet = new Planet(this);
            toUpdate.Add(planet);

        }

        public override void Update()
        {
            foreach (var updatable in toUpdate)
            {
                // change position to match grid
            }
            base.Update();
        }
    }
}
