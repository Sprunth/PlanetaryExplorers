﻿using System;
using System.Drawing;
using Noise;
using Noise.Modules;
using Noise.Utils;
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
        private CircleShape cs;

        public Planet(Display parentDisplay) : base(parentDisplay)
        {
            cs = new CircleShape(32, 32);
            cs.Position = new Vector2f(256, 128);
            cs.OutlineThickness = 3;
            cs.OutlineColor = new Color(30, 20, 10);
            //cs.FillColor = SFML.Graphics.Color.Magenta;
            cs.Texture = GeneratePlanetTexture(new Vector2u((uint) cs.Radius*2, (uint) cs.Radius*2));

            AddItemToDraw(cs, 5);
        }

        private static Texture GeneratePlanetTexture(Vector2u texSize)
        {
            Vector2u imgSize = texSize;
            module = new Perlin(2, 0.2, NoiseQuality.Best, 4, 0.3, random.Next(0, 1024));
            heightMapBuilder = new PlanarNoiseMapBuilder(imgSize.X, imgSize.Y, 0, module, 2, 6, 1, 5, false);
            heightMap = heightMapBuilder.Build();

            var texColors = new GradientColour();
            texColors.AddGradientPoint(-1, System.Drawing.Color.DimGray);
            texColors.AddGradientPoint(0.0, System.Drawing.Color.SteelBlue);
            texColors.AddGradientPoint(0.8, System.Drawing.Color.Tan);
            texColors.AddGradientPoint(1, System.Drawing.Color.WhiteSmoke);
            var renderer = new ImageBuilder(heightMap, texColors);
            Image renderedImg = renderer.Render();
            var img = new Bitmap(renderedImg);
            var sfmlImg = new SFML.Graphics.Image(imgSize.X, imgSize.Y);

            for (uint x = 0; x < imgSize.X; x++)
            {
                for (uint y = 0; y < imgSize.Y; y++)
                {
                    System.Drawing.Color col = img.GetPixel((int) x, (int) y);
                    sfmlImg.SetPixel(x, y, new Color(col.R, col.G, col.B, col.A));
                }
            }

            var returnTex = new Texture(sfmlImg);
            return returnTex;
        }
    }
}