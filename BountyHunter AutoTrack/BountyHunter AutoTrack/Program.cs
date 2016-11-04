using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensage;
using Ensage.Common;
using SharpDX;

namespace BountyHunter_AutoTrack
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.PrintMessage("AutoTrack v 0.0 loaded -> only you can see this message.", 0);
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var me = ObjectMgr.LocalHero;
            var rSpell = me.Spellbook.Spell4;

            uint healthtreshhold = 50;


            if (me.Mana > me.Spellbook.Spell4.ManaCost && rSpell.Cooldown == 0 && me.IsAlive)
            {
                //no double track einfügen, invis abfrage einfügen, sleep einfügen
                var enemies = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && x.IsAlive && !x.IsIllusion && Range(me, x) < rSpell.CastRange * rSpell.CastRange && x.Health * 100 / x.MaximumHealth < healthtreshhold);
                
                if (enemies != null)
                {
                    rSpell.UseAbility(enemies.First());
                }
            }


        }

        private static float Range(dynamic A, dynamic B)
        {
            if (!(A is Unit || A is Vector3)) throw new ArgumentException("Not valid parameters, Accepts Unit/Vector3 only", "A");
            if (!(B is Unit || B is Vector3)) throw new ArgumentException("Not valid parameters, Accepts Unit/Vector3 only", "B");
            if (A is Unit) A = A.Position;
            if (B is Unit) B = B.Position;
            return ((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
        }

    }
}
