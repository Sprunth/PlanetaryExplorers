using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

using Planetary_Explorers.Spaceships;

namespace Planetary_Explorers.SpaceMap
{
    class SpaceGrid : Display
    {
        private View _view;
        private RenderTexture gridTexture;
        private Vertex[] gridlines; 
        private Sprite grid;

        private bool dragging;
        private Vector2i mousePrevDragPos;

        private readonly List<Planet> allPlanets;

        public SpaceGrid(Vector2u mapSize, Vector2u displaySize)
            : base(displaySize)
        {
            SetupGrid(mapSize);

            dragging = false;

            OnLostFocus += SpaceGrid_OnLostFocus;
            OnMouseMove += SpaceGrid_OnMouseMove;
            OnMousePress += SpaceGrid_OnMousePress;
            OnMouseRelease += SpaceGrid_OnMouseRelease;
            OnKeyPress += SpaceGrid_OnKeyPress;

            allPlanets = new List<Planet>
            {
                new Planet(this)
            };

            var ship = new Spaceship(this);
        }
        
        public override void Update()
        {
            gridTexture.Clear(new Color(190, 190, 190));
            gridTexture.Draw(gridlines, PrimitiveType.Lines);
            gridTexture.Display();
            grid.Texture = gridTexture.Texture;
            base.Update();
        }

        private void SetupGrid(Vector2u mapSize)
        {
            const int gridSize = 16;
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
            //_view = new View(new FloatRect(0,0,displaySize.X, displaySize.Y));
            _view = new View(new FloatRect(0, 0, DisplayView.Size.X, DisplayView.Size.Y));
            DisplayView = _view;
            //gridTexture.SetView(_view);
            gridTexture.Draw(gridlines, PrimitiveType.Lines);
            gridTexture.Display();

            grid = new Sprite(gridTexture.Texture);
            AddItemToDraw(grid, 0);
        }

        private void SpaceGrid_OnLostFocus(object sender, EventArgs e)
        {
            dragging = false;
        }

        void SpaceGrid_OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Right)
            {
                _view.Move(new Vector2f(10, 0));
                DisplayView = _view;
                //gridTexture.SetView(_view);
            }
        }

        void SpaceGrid_OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            //Debug.WriteLine(gridTexture.MapPixelToCoords(new Vector2i(e.X,e.Y)));

            if (dragging)
            {
                var mousePos = new Vector2i(e.X, e.Y);
                var diffVec = mousePos - mousePrevDragPos;
                _view.Move(new Vector2f(-diffVec.X / 1f, -diffVec.Y / 1f));
                DisplayView = _view;
                //gridTexture.SetView(_view);
                mousePrevDragPos = mousePos;
            }
        }

        void SpaceGrid_OnMousePress(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            mousePrevDragPos = new Vector2i(e.X, e.Y);
        }

        void SpaceGrid_OnMouseRelease(object sender, MouseButtonEventArgs e)
        {
            dragging = false;

            foreach (var planet in allPlanets)
            {
                if (planet.ContainsVector(e.X, e.Y))
                    planet.Select(true);
                else
                    planet.Select(false);

                
            }
        }
    }
}
