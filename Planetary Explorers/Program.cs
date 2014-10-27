using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planetary_Explorers
{
    class Program
    {
        public static PEGame ActiveGame;
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

            ActiveGame = new PEGame();
            ActiveGame.Initialize();
            ActiveGame.Run();
        }
    }
}
