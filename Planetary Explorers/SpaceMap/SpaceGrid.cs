using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.SpaceMap
{
    class SpaceGrid : DrawableObject
    {
        private View view;
        private RenderTexture gridTexture;
        private Vertex[] gridlines; 
        private Sprite grid;

        private bool dragging;
        private Vector2i mousePrevDragPos;

        public SpaceGrid(Vector2u mapSize, Vector2u displaySize, Display parentDisplay)
            : base(parentDisplay)
        {
            SetupGrid(mapSize);

            dragging = false;

            parentDisplay.OnMouseMove += parentDisplay_OnMouseMove;
            parentDisplay.OnMousePress += parentDisplay_OnMousePress;
            parentDisplay.OnMouseRelease += parentDisplay_OnMouseRelease;
            parentDisplay.OnKeyPress += parentDisplay_OnKeyPress;
        }
        
        public override void Update()
        {
            gridTexture.Clear(new Color(190, 190, 190));
            gridTexture.Draw(gridlines, PrimitiveType.Lines);
            gridTexture.Display();
            grid.Texture = gridTexture.Texture;
            base.Update();
        }

        void parentDisplay_OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Right)
            {
                view.Move(new Vector2f(10,0));
                gridTexture.SetView(view);
            }
        }

        void parentDisplay_OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            //Debug.WriteLine(gridTexture.MapPixelToCoords(new Vector2i(e.X,e.Y)));

            if (dragging)
            {
                var mousePos = new Vector2i(e.X, e.Y);
                var diffVec = mousePos - mousePrevDragPos;
                view.Move(new Vector2f(-diffVec.X/2f, -diffVec.Y/2f));
                gridTexture.SetView(view);
                mousePrevDragPos = mousePos;
            }
        }

        void parentDisplay_OnMousePress(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            mousePrevDragPos = new Vector2i(e.X, e.Y);
        }

        void parentDisplay_OnMouseRelease(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
        }

        private void SetupGrid(Vector2u mapSize)
        {
            var gridSize = 8;
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
            gridlines = verticies.ToArray();

            gridTexture.Clear(new Color(190, 190, 190));
            //view = new View(new FloatRect(0,0,displaySize.X, displaySize.Y));
            view = new View(new FloatRect(0, 0, 600, 600));
            gridTexture.SetView(view);
            gridTexture.Draw(gridlines, PrimitiveType.Lines);
            gridTexture.Display();

            grid = new Sprite(gridTexture.Texture);
            AddItemToDraw(grid, 0);
        }
    }
}
