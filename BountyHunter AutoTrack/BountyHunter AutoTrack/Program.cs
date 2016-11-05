using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensage;
using Ensage.Common;
using SharpDX;
using Ensage.Common.Menu;

namespace BountyHunter_AutoTrack
{
    

    class Program
    {
        private static readonly Menu Menu = new Menu("AutoTrack", "autotrack", true, "npc_dota_hero_bounty_hunter", true);
        private static bool IsStealthed = false;

        static void Main(string[] args)
        {
            Game.PrintMessage("AutoTrack v 1.0 loaded", 0);
            Game.OnUpdate += Game_OnUpdate;

 

            Menu.AddToMainMenu();

            Menu.AddItem(new MenuItem("AutoTrack", "Enable Autotrack").SetValue(true));
            Menu.AddItem(new MenuItem("hpthreshold", "HP Threshold").SetValue(new Slider(100, 1)).SetTooltip("Using Track on Targets with HP lower than x%"));

            var interruptMenu = new Menu("Interrupt Stealth", "interruptMenu", false);
            interruptMenu.AddItem(new MenuItem("dontCancelStealth", "Dont cancel Stealth?").SetValue(true));
            interruptMenu.AddItem(new MenuItem("onlyIfReady", "Only cancel if Stealth is ready").SetValue(true));
            Menu.AddSubMenu(interruptMenu);
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame)
                return;
            
            var me = ObjectMgr.LocalHero;

            if (me.Modifiers.FirstOrDefault(e => e.Name == "modifier_bounty_hunter_wind_walk") != null)
                IsStealthed = true;
            else
                IsStealthed = false;
                
            if (me == null || !me.IsAlive || !Menu.Item("AutoTrack").GetValue<bool>())
                return;
            if (IsStealthed && Menu.Item("dontCancelStealth").GetValue<bool>())
                return;
            if (IsStealthed && Menu.Item("onlyIfReady").GetValue<bool>() && me.Spellbook.Spell3.Cooldown > 0)
                return;

            

            var rSpell = me.Spellbook.Spell4;

            int healthtreshhold = Menu.Item("hpthreshold").GetValue<Slider>().Value;


            if (me.Mana > me.Spellbook.Spell4.ManaCost && rSpell.Cooldown == 0 && me.IsAlive)
            {
                //to-Do, lowest life first
                
                var enemies = ObjectMgr.GetEntities<Hero>().Where(x => x.Team != me.Team && x.IsAlive && !x.IsIllusion && Range(me, x) < rSpell.CastRange * rSpell.CastRange && x.Health * 100 / x.MaximumHealth <= healthtreshhold && Utils.SleepCheck(me.Handle + "track") && x.Modifiers.FirstOrDefault(e => e.Name == "modifier_bounty_hunter_track") == null).ToList();

                if (enemies.Count() > 0)
                {
                    rSpell.UseAbility(enemies[0]);
                    Utils.Sleep(700, me.Handle + "track");
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
