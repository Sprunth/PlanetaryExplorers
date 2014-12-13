using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Noise;
using Noise.Modules;
using Noise.Utils;
using Planetary_Explorers.Gui;
using SFML.Graphics;
using SFML.Window;
using Color = SFML.Graphics.Color;
using Image = System.Drawing.Image;

namespace Planetary_Explorers.SpaceMap
{
    internal class Planet : DrawableObject
    {
        private static readonly Random random = new Random();
        private static NoiseMap heightMap;
        private static PlanarNoiseMapBuilder heightMapBuilder;
        private static Perlin module;
        
        private readonly CircleShape _cs;
        private readonly Label _hoverText;

        public Planet(Display parentDisplay) : base(parentDisplay)
        {
            _cs = new CircleShape(32, 32)
            {
                Position = new Vector2f(256, 128),
                OutlineThickness = 3,
                OutlineColor = new Color(20, 20, 20),
                Origin = new Vector2f(16, 16)
            };
            //_cs.FillColor = SFML.Graphics.Color.Magenta;
            _cs.Texture = GeneratePlanetTexture(new Vector2u((uint) _cs.Radius*2, (uint) _cs.Radius*2));
            AddItemToDraw(_cs, 5);

            _hoverText = new Label("Planet", FontManager.ActiveFontManager, new Vector2u(100,40));

            parentDisplay.OnMouseMove += parentDisplay_OnMouseMove;
        }

        void parentDisplay_OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            var mouseCoords = parentDisplay.Target.MapPixelToCoords(
                new Vector2i((int)Math.Round((double)e.X), (int)Math.Round((double)e.Y)));
            if (ContainsVector(new Vector2f(e.X, e.Y)))
            {
                // within planet's sprite
                _cs.FillColor = new Color(255, 255, 255);
                _hoverText.EventSubscribe(true, GameManager.ActiveWindow);
                AddItemToDraw(_hoverText, 30);
                _hoverText.Position = mouseCoords + new Vector2f(20, -20);
            }
            else
            {
                _cs.FillColor = new Color(200,200,200);
                _hoverText.EventSubscribe(false, GameManager.ActiveWindow);
                RemoveItemToDraw(_hoverText, 30);
            }
        }

        public override bool ContainsVector(double x, double y)
        {
            var mouseCoords = parentDisplay.Target.MapPixelToCoords(
                new Vector2i((int)Math.Round(x), (int)Math.Round(y)));
            var dist = Math.Sqrt(
                Math.Pow(mouseCoords.X - (_cs.Position.X + _cs.Origin.X), 2) +
                Math.Pow(mouseCoords.Y - (_cs.Position.Y + _cs.Origin.Y), 2));
            return (dist < _cs.Radius);
        }

        /// <summary>
        /// Change whether to draw planet as selected or not
        /// </summary>
        /// <param name="select">True for selected, False for not selected</param>
        public void Select(bool select)
        {
            _cs.OutlineColor = select ? new Color(200, 210, 40) : new Color(20, 20, 20);
            _cs.OutlineThickness = select ? 4 : 3;
        }

        private static Texture GeneratePlanetTexture(Vector2u texSize)
        {
            var imgSize = texSize;
            module = new Perlin(3, 0.2, NoiseQuality.Best, 4, 0.7, random.Next(0, 1024));
            heightMapBuilder = new PlanarNoiseMapBuilder(imgSize.X, imgSize.Y, 0, module, 2, 6, 1, 5, false);
            heightMap = heightMapBuilder.Build();

            var texColors = new GradientColour();
            texColors.AddGradientPoint(-1, System.Drawing.Color.DimGray);
            texColors.AddGradientPoint(0.0, System.Drawing.Color.SteelBlue);
            texColors.AddGradientPoint(0.8, System.Drawing.Color.Tan);
            texColors.AddGradientPoint(1, System.Drawing.Color.WhiteSmoke);
            var renderer = new ImageBuilder(heightMap, texColors);
            var renderedImg = renderer.Render();
            var img = new Bitmap(renderedImg);
            var sfmlImg = new SFML.Graphics.Image(imgSize.X, imgSize.Y);

            for (uint x = 0; x < imgSize.X; x++)
            {
                for (uint y = 0; y < imgSize.Y; y++)
                {
                    var col = img.GetPixel((int) x, (int) y);
                    sfmlImg.SetPixel(x, y, new Color(col.R, col.G, col.B, col.A));
                }
            }

            var returnTex = new Texture(sfmlImg);
            return returnTex;
        }
    }
}