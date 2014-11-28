using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.ShipImageGenerator
{
    internal class PixelSpaceship
    {
        private const int cols = 12;
        private const int rows = 12;
        private const int EMPTY = 0;
        private const int AVOID = 1;
        private const int SOLID = 2;
        private const int COKPT = 3; // ::added to aid coloring
        private int seed;
        private int colorseed; // ::added to aid coloring
        private int[,] grid;
        private int xscale, yscale;
        private int xmargin, ymargin;
        private MiniMT rng;

        public PixelSpaceship(MiniMT rng)
        {
            this.rng = rng;
            grid = new int[rows, cols];
            xscale = yscale = 1;
            xmargin = ymargin = 0;
        }

        public void Recolor()
        {
            // ::added to aid coloring
            colorseed = rng.Generate();
        }

        private int getHeight()
        {
            return rows*yscale + ymargin*2;
        }

        private int getWidth()
        {
            return cols*xscale + xmargin*2;
        }

        public void SetMargins(int xm, int ym)
        {
            xmargin = xm;
            ymargin = ym;
        }

        public void SetScales(int xs, int ys)
        {
            xscale = xs;
            yscale = ys;
        }

        public void SetSeed(int s)
        {
            seed = s;
        }

        private void Wipe()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    grid[r, c] = EMPTY;
        } // Wipe()

        public void Generate()
        {
            Wipe();
            // FILL IN THE REQUIRED SOLID CELLS
            int[] solidcs = {5, 5, 5, 5, 5};
            int[] solidrs = {2, 3, 4, 5, 9};
            for (int i = 0; i < 5; i++)
            {
                int c = solidcs[i];
                int r = solidrs[i];
                grid[r, c] = SOLID;
            }
            // FILL IN THE SEED-SPECIFIED BODY CELLS, AVOID OR EMPTY
            int[] avoidcs = {4, 5, 4, 3, 4, 3, 4, 2, 3, 4, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 4, 3, 4, 5};
            int[] avoidrs = {1, 1, 2, 3, 3, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10};
            int bitmask = 1;
            for (int i = 0; i < 26; i++)
            {
                int c = avoidcs[i];
                int r = avoidrs[i];
                grid[r, c] = ((seed & bitmask) != 0) ? AVOID : EMPTY;
                bitmask <<= 1;
            }
            // FLIP THE SEED-SPECIFIED COCKPIT CELLS, SOLID OR EMPTY
            int[] emptycs = {4, 5, 4, 5, 4, 5};
            int[] emptyrs = {6, 6, 7, 7, 8, 8};
            bitmask = 1 << 26;
            for (int i = 0; i < 6; i++)
            {
                int c = emptycs[i];
                int r = emptyrs[i];
                grid[r, c] = ((seed & bitmask) != 0) ? SOLID : COKPT; // ::added to aid coloring
                bitmask <<= 1;
            }
            // SKINNING -- wrap the AVOIDs with SOLIDs where EMPTY
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int here = grid[r, c];
                    if (here != EMPTY)
                        continue;
                    bool needsolid = false;

                    if ((c > 0) && (grid[r,c - 1] == AVOID))
                        needsolid = true;
                    if ((c < cols - 1) && (grid[r, c + 1] == AVOID))
                        needsolid = true;
                    if ((r > 0) && (grid[r - 1, c] == AVOID))
                        needsolid = true;
                    if ((r < rows - 1) && (grid[r + 1, c] == AVOID))
                        needsolid = true;

                    if (needsolid)
                        grid[r, c] = SOLID;
                }
            }
            // mirror left side into right side
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols/2; c++)
                    grid[r, cols - 1 - c] = grid[r, c];
            }
        }

        public void Draw(int basex, int basey, RenderTexture texture)
        {
            // ::added to aid coloring
            // here's one (of many) possible ways you might color them...
            float[] sats = {40, 60, 80, 100, 80, 60, 80, 100, 120, 100, 80, 60};
            float[] bris = {40, 70, 100, 130, 160, 190, 220, 220, 190, 160, 130, 100, 70, 40};
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int x1 = basex + xmargin + c*xscale;
                    int y1 = basey + ymargin + r*yscale;
                    int m = grid[r,c];
                    // ::added to aid coloring
                    // for monochrome just Draw SOLID's as black and you're done
                    // otherwise...
                    if (m == SOLID)
                    {
                        //fill(#000000);
                        texture.Clear(Color.White);
                        //rect(x1, y1, xscale, yscale);
                        texture.Draw(new RectangleShape(new Vector2f(x1,y1)){Scale = new Vector2f(xscale, yscale)});
                    }
                    else if (m == AVOID)
                    {
                        float mysat = sats[r];
                        float mybri = bris[c]; //+90;
                        int h = 0;
                        if (r < 6) h = (colorseed & 0xff00) >> 8;
                        else if (r < 9) h = (colorseed & 0xff0000) >> 16;
                        else h = (colorseed & 0xff000000) >> 24;
                        //colorMode(HSB);
                        fill(h, mysat, mybri);

                        rect(x1, y1, xscale, yscale);
                    }
                    else if (m == COKPT)
                    {
                        float mysat = sats[c];
                        float mybri = bris[r] + 40;
                        //colorMode(HSB);
                        int h = (colorseed & 0xff);
                        fill(h, mysat, mybri);
                        rect(x1, y1, xscale, yscale);
                    }
                }
            }
        }
    }
}