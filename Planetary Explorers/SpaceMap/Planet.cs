using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Noise;
using Noise.Modules;
using Noise.Utils;
using SFML.Graphics;
using SFML.Window;
using Image = System.Drawing.Image;
using Color = System.Drawing.Color;

namespace Planetary_Explorers.SpaceMap
{
    class Planet : DrawableObject
    {
        private CircleShape cs;

        public Planet(Display parentDisplay) : base(parentDisplay)
        {
            cs = new CircleShape(32, 32);
            cs.Position = new Vector2f(256, 128);
            cs.OutlineThickness = 3;
            cs.OutlineColor = new SFML.Graphics.Color(30,20,10);
            //cs.FillColor = SFML.Graphics.Color.Magenta;
            cs.Texture = GeneratePlanetTexture(new Vector2u((uint)cs.Radius*2, (uint)cs.Radius*2));

            AddItemToDraw(cs, 5);
        }

        private static Random random = new Random();
        private static NoiseMap heightMap;
        private static PlanarNoiseMapBuilder heightMapBuilder;
        private static Perlin module;

        private static Texture GeneratePlanetTexture(Vector2u texSize)
        {
            var imgSize = texSize;
            module = new Perlin(2, 0.2, NoiseQuality.Best, 4, 0.3, random.Next(0,1024));
            heightMapBuilder = new PlanarNoiseMapBuilder(imgSize.X, imgSize.Y, 0, module, 2, 6, 1, 5, false);
            heightMap = heightMapBuilder.Build();

            var texColors = new GradientColour();
            texColors.AddGradientPoint(-1, Color.DimGray);
            texColors.AddGradientPoint(0.0,Color.SteelBlue);
            texColors.AddGradientPoint(0.8, Color.Tan);
            texColors.AddGradientPoint(1, Color.WhiteSmoke);
            var renderer = new ImageBuilder(heightMap, texColors);
            var renderedImg = renderer.Render();
            var img = new Bitmap(renderedImg);
            var sfmlImg = new SFML.Graphics.Image(imgSize.X, imgSize.Y);

            for (uint x = 0; x < imgSize.X; x++)
            {
                for (uint y = 0; y < imgSize.Y; y++)
                {
                    var col = img.GetPixel((int) x, (int) y);
                    sfmlImg.SetPixel(x, y, new SFML.Graphics.Color(col.R, col.G, col.B, col.A));
                }
            }
            
            var returnTex = new Texture(sfmlImg);
            return returnTex;
        }
    }
}
