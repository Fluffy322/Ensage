using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensage;
using SharpDX;
using Ensage.Common;

namespace NecroSharp
{
    class Program
    {
        static void Main(string[] args)

        {
            Game.PrintMessage("Loaded", 0);

            Game.OnUpdate += Game_OnUpdate;


        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Game.PrintMessage("onUpdateFired", 0);
        }
    }
}
