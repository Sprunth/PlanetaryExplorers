using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers
{
    class SpaceGrid : DrawableObject
    {
        private View view;
        private RenderTexture gridTexture;
        private Sprite grid;

        public SpaceGrid(Vector2u mapSize, Vector2u displaySize, Display parentDisplay)
            : base(parentDisplay)
        {
            var gridSize = 16;
            gridTexture = new RenderTexture(2000, 2000);
            var col = new Color(120, 120, 120);
            var verticies = new List<Vertex>();
            for (int x = 0; x < mapSize.X; x += gridSize)
            {
                verticies.Add(new Vertex(new Vector2f(x, 0), col));
                verticies.Add(new Vertex(new Vector2f(x, mapSize.Y), col));
            }
            for (int y = 0; y < mapSize.Y; y += gridSize)
            {
                verticies.Add(new Vertex(new Vector2f(0, y), col));
                verticies.Add(new Vertex(new Vector2f(mapSize.X, y), col));
            }
            var gridlines = verticies.ToArray();

            gridTexture.Clear(new Color(190, 190, 190));
            view = new View(new FloatRect(0,0,displaySize.X, displaySize.Y));
            //gridTexture.SetView(view);
            gridTexture.Draw(gridlines, PrimitiveType.Lines);
            gridTexture.Display();

            grid = new Sprite(gridTexture.Texture);
            AddItemToDraw(grid, 0);
        }
    }
}
