using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;


using System.Diagnostics;

namespace Planetary_Explorers
{
    class PEGame
    {
        RenderWindow window;
        public Vector2u ScreenSize { get; set; }

        RenderTexture target;

        Sprite spr;

        public PEGame()
        {
            ScreenSize = new Vector2u(900, 600);
            var cs = new ContextSettings()
            {AntialiasingLevel = 4};
            window = new RenderWindow(new VideoMode(ScreenSize.X, ScreenSize.Y), "Planetary Explorers", Styles.Default, cs);
            window.KeyPressed += window_KeyPressed;

            window.SetFramerateLimit(60);
            window.SetVerticalSyncEnabled(false);
            window.Closed += window_Closed;


            spr = new Sprite();

            target = new RenderTexture(ScreenSize.X, ScreenSize.Y)
            { Smooth = true };
        }

        public void Initialize()
        {

        }

        public void Run()
        {
            var frameStart = DateTime.Now;
            var lastFPS = 0.0;

            while (window.IsOpen())
            {
                frameStart = DateTime.Now;

                Update();

                Draw();

                // Not super accurate, espcially if frame is working too fast
                var fps = Math.Round(1 / (DateTime.Now - frameStart).TotalSeconds);
                if ((int)fps != (int)lastFPS)
                {
                    window.SetTitle(string.Format("Planetary Explorers | FPS: {0}", fps));
                    lastFPS = fps;
                    
                }
            }

            Dispose();
        }

        private void Update()
        {
            window.DispatchEvents();
        }

        private void Draw()
        {
            target.Clear(new Color(210, 230, 200));
            
            target.Display();

            spr.Texture = target.Texture;

            window.Clear();
            window.Draw(spr);
            window.Display();
        }

        private void window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }

        void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window_Closed(sender, e);
            }
        }

        void Dispose()
        {
            window.Dispose();
            target.Dispose();
        }

    }
}
