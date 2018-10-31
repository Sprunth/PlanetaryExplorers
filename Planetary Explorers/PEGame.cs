using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planetary_Explorers.PlanetView;
using Planetary_Explorers.SpaceMap;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

using System.Diagnostics;
using SFML.System;

namespace Planetary_Explorers
{
    class PEGame
    {
        readonly RenderWindow _window;
        public Vector2u ScreenSize { get; set; }

        readonly RenderTexture _target;
        readonly Sprite _targetSpr;

        private GameManager _gm;

        public PEGame()
        {
            ScreenSize = new Vector2u(1100, 600);
            var cs = new ContextSettings()
            {
                DepthBits = 24,
                StencilBits = 8,
                AntialiasingLevel = 4,
                MajorVersion = 0,
                MinorVersion = 1
            };
            _window = new RenderWindow(new VideoMode(ScreenSize.X, ScreenSize.Y), "Planetary Explorers", Styles.Close, cs);
            _window.KeyPressed += window_KeyPressed;
            _window.MouseMoved += window_MouseMoved;
            _window.MouseButtonReleased += window_MouseButtonReleased;

            _window.SetFramerateLimit(60);
            _window.SetVerticalSyncEnabled(false);
            _window.Closed += window_Closed;

            _targetSpr = new Sprite();

            _target = new RenderTexture(ScreenSize.X, ScreenSize.Y)
            { Smooth = true };
        }

        public void Initialize()
        {
            // TODO: Use actual display rather than this generic one
            var d = new Display(ScreenSize);
            _gm = new GameManager(_window, d)
            {
                
                ActiveDisplayRoot = new SpaceGrid(new Vector2u(2000, 2000), new Vector2u(800, 500))
                {
                    Position = new Vector2f(40,90)
                }
                
                //ActiveDisplayRoot = new PlanetDisplay(new Vector2u(600,500))
            };
            //((PlanetDisplay)_gm.ActiveDisplayRoot).FocusedPlanet = new Planet(_gm.ActiveDisplayRoot);
        }

        public void Run()
        {
            var frameStart = DateTime.Now;
            var lastFPS = 0;

            while (_window.IsOpen)
            {
                frameStart = DateTime.Now;

                Update();

                Draw();

                // Not super accurate, espcially if frame is working too fast
                var fps = (int)Math.Round(1 / (DateTime.Now - frameStart).TotalSeconds);
                if (fps != lastFPS)
                {
                    _window.SetTitle(string.Format("Planetary Explorers | FPS: {0}", fps));
                    lastFPS = fps;
                }
            }

            Dispose();
        }

        private void Update()
        {
            _window.DispatchEvents();
            _gm.Update();
        }

        private void Draw()
        {
            _target.Clear(new Color(210, 230, 200));
            _gm.Draw(_target);
            _target.Display();

            _targetSpr.Texture = _target.Texture;

            _window.Clear();
            _window.Draw(_targetSpr);
            _window.Display();
        }

        private void window_Closed(object sender, EventArgs e)
        {
            _window.Close();
        }

        void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window_Closed(sender, e);
            }
        }

        void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Middle)
            {
                var planet = ((SpaceGrid) _gm.ActiveDisplayRoot).allPlanets[0];
                _gm.ActiveDisplayRoot = new PlanetDisplay(new Vector2u(600, 500))
                {
                    Position = new Vector2f(400,20)
                };
                ((PlanetDisplay) _gm.ActiveDisplayRoot).FocusedPlanet = planet;
            }
        }

        void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {

        }

        void Dispose()
        {
            _window.Dispose();
            _target.Dispose();
        }
    }
}
