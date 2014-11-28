using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Planetary_Explorers.ShipImageGenerator
{
    internal class PixelSpaceshipImageGenerator
    {
        private PixelSpaceshipFitter fitter;
        private PixelSpaceship ship;
        private MiniMT rng; // want a slightly more robust rng for 32 significant bits
        private int currentSeed = 0;
        private RenderTexture texture;

        public PixelSpaceshipImageGenerator()
        {
            Setup();
        }

        private void Setup()
        {
            texture = new RenderTexture(480, 480);
            rng = new MiniMT(0);
            fitter = new PixelSpaceshipFitter(480 / 16, 480 / 16, rng);
            next();
        }

        private void mousePressed()
        {
            next();
        }

        private void next()
        {
            fitter.Make(++currentSeed);
        }

        private void Draw()
        {
            fitter.DrawOne(texture);
            if (fitter.At00())
                next();
        }
    }

    /// <summary>
    /// Just going to use System.random, but keeping API the same
    /// </summary>
    public class MiniMT
    {
        private Random rand;

        public MiniMT(int seed)
        {
            rand = new Random(seed);
        }

        public void SetSeed(int seed)
        {
            rand = new Random(seed);
        }

        public int Generate()
        {
            //return rand.Next(0, 2);
            return rand.Next();
        }

        public int Generate(int limit)
        {
            return rand.Next(limit);
        }
    }
}