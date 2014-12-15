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

namespace Planetary_Explorers.SpaceMap
{
    internal class Planet : DrawableObject
    {
        private static readonly Random random = new Random();
        private static NoiseMap heightMap;
        private static PlanarNoiseMapBuilder heightMapBuilder;
        private static Perlin perlin;
        private static RidgedMulti ridgedMulti;
        private static Voronoi voronoi;
        private static Select selectModule;
        
        private readonly CircleShape _cs;
        private readonly Label _hoverText;

        public Planet(Display parentDisplay) : base(parentDisplay)
        {
            _cs = new CircleShape(64, 64)
            {
                Position = new Vector2f(256, 128),
                OutlineThickness = 3,
                OutlineColor = new Color(20, 20, 20)
            };
            _cs.Origin = new Vector2f(_cs.Radius/2f, _cs.Radius/2f);
            //_cs.FillColor = SFML.Graphics.Color.Magenta;
            _cs.Texture = GeneratePlanetTexture(new Vector2u((uint) _cs.Radius*2, (uint) _cs.Radius*2));
            AddItemToDraw(_cs, 5);

            _hoverText = new Label("Planet", FontManager.ActiveFontManager, new Vector2u(100,40));

            parentDisplay.OnMouseMove += parentDisplay_OnMouseMove;
        }

        void parentDisplay_OnMouseMove(object sender, MouseMoveEventArgs e, Vector2f displayCoords)
        {
            if (ContainsVector(displayCoords))
            {
                // within planet's sprite
                _cs.FillColor = new Color(255, 255, 255);
                _hoverText.EventSubscribe(true, GameManager.ActiveWindow);
                AddItemToDraw(_hoverText, 30);
                _hoverText.Position = displayCoords + new Vector2f(20, -20);
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
            var dist = Math.Sqrt(
                Math.Pow(x - (_cs.Position.X + _cs.Origin.X), 2) +
                Math.Pow(y - (_cs.Position.Y + _cs.Origin.Y), 2));
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
            perlin = new Perlin(random.Next(2,3), 0.2, NoiseQuality.Best, 4, 0.7, random.Next(0, 1024));
            ridgedMulti = new RidgedMulti(random.NextDouble()*2, 0.3, 2,NoiseQuality.Best, random.Next(0,1024));
            voronoi = new Voronoi(0.1, random.NextDouble()*2, true, random.Next(0,1024));
            selectModule = new Select(1.0, 1.0, 0.0);
            selectModule.SetSourceModule(0, perlin);
            selectModule.SetSourceModule(1, ridgedMulti);
            selectModule.SetSourceModule(2, voronoi);
            
            heightMapBuilder = new PlanarNoiseMapBuilder(imgSize.X, imgSize.Y, 0, selectModule, 1, 6, 1, 6, false);
            heightMap = heightMapBuilder.Build();

            var texColors = new GradientColour();
            texColors.AddGradientPoint(-1, GenerateProceduralColor());
            texColors.AddGradientPoint(-0.2 + random.NextDouble()*0.4, GenerateProceduralColor());
            texColors.AddGradientPoint(1, GenerateProceduralColor());
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

        /// <summary>
        /// Fancy color generation that looks better than just pure random
        /// </summary>
        /// <returns></returns>
        private static System.Drawing.Color GenerateProceduralColor()
        {           
            _colorGenerationCounter += random.Next(1,6);
            // golden ratio
#if DEBUG
            var ret = Helper.HSL2RGB((double)(((Decimal)(colorOffset + (0.618033988749895f * _colorGenerationCounter))) % 1.0m), 0.5, 0.5);
            Debug.WriteLine(ret);
            return ret;
#else
            return Helper.HSL2RGB((double)(((Decimal)(colorOffset + (0.618033988749895f * _colorGenerationCounter))) % 1.0m), 0.5, 0.5);
#endif
        }
        private static readonly float colorOffset = (float)random.NextDouble();
        private static int _colorGenerationCounter = 2;

    }
}