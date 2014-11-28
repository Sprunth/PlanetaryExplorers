using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Planetary_Explorers.ShipImageGenerator
{
    internal class PixelSpaceshipFitter
    {
        private PixelSpaceship ship;
        private int cols, rows;
        private int col, row;
        private int seed;
        // cells store the box fit pattern, contents are byte encoded as:
        // (cell >> 16) & 0xff == work to do
        // (cell) & 0xff == size of allocated area
        // (this somewhat odd encoding scheme is a remnant of the
        //  fractal subdivision method used for the pixel robots t-shirt image)
        private int[,] cells;
        // the types of work that may occur in a cell
        private const int WORK_NONE = 0;
        private const int WORK_DONE = 1;
        private MiniMT rng;

        public PixelSpaceshipFitter(int c, int r, MiniMT rng)
        {
            this.rng = rng;
            cols = c;
            rows = r;
            ship = new PixelSpaceship(rng);
            cells = new int[rows, cols];
        }

        // reset the pattern grid
        private void Wipe()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    cells[r,c] = 0;
            col = row = 0;
        }

        // determines if cells already contain defined area(s)
        private bool IsOccupied(int col, int row, int wid, int hei)
        {
            for (int r = row; r < row + hei; r++)
            {
                for (int c = col; c < col + wid; c++)
                {
                    if (cells[r, c] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // marks cells as containing an area
        private void DoOccupy(int col, int row, int wid, int hei, int val)
        {
            for (int r = row; r < row + hei; r++)
            {
                for (int c = col; c < col + wid; c++)
                {
                    cells[r, c] = val;
                }
            }
        }

        // define the pattern grid
        public void Make(int s)
        {
            seed = s;
            //randomSeed(seed);
            Wipe();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int cell = cells[r,c];
                    if (cell != 0) continue;
                    // figure out the size of area to occupy
                    int sizer, limit;
                    do
                    {
                        limit = Math.Min(cols - c, rows - r);
                        limit = Math.Min(limit, 8);
                        sizer = rng.Generate(limit) + 1;
                    } while (IsOccupied(c, r, sizer, sizer));
                    // flag all cells as occupied by width x height area
                    DoOccupy(c, r, sizer, sizer, sizer);
                    // indicate work to perform in upper-left cell
                    int work = WORK_DONE;
                    cells[r,c] |= (work << 16);
                } // for c
            } // for r
        }

        // is cursor at 0,0
        public bool At00()
        {
            return ((col == 0) && (row == 0));
        }

        // step the cursor forward
        private void Advance(int advcol)
        {
            col += advcol;
            if (col >= cols)
            {
                col = 0;
                row++;
                if (row >= rows)
                {
                    row = 0;
                }
            }
        }

        // Advance through the pattern and Draw the next robot
        public void DrawOne(RenderTexture texture)
        {
            bool drawn = false;
            do
            {
                int cell = cells[row, col];
                int work = (cell >> 16) & 0xff;
                int sizer = (cell) & 0xff;
                if (work != WORK_NONE)
                {
                    int y1 = 16*row;
                    int x1 = 16*col;
                    ship.SetScales(sizer, sizer);
                    ship.SetMargins(2*sizer, 2*sizer);
                    ship.SetSeed(rng.Generate());
                    ship.Generate();
                    ship.Recolor();
                    ship.Draw(x1, y1, texture);
                    drawn = true;
                }
                Advance(sizer);
                if (At00()) return;
            } while (!drawn);
        }
    }
}