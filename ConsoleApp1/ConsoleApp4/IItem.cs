using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal interface IItem
    {
        string Name { get; }
    }

    interface Potion : IItem
    {
        void Use(IPlayer player);
    }

    class HealthPotion : Potion
    {
        public string Name { get; } = "HeathPotion";

        public void Use(IPlayer player)
        {
            player.Health += 10;
        }
    }

    class StrengthPotion : Potion
    {
        public string Name { get; } = "StrengthPotion";

        public void Use(IPlayer player)
        {
            player.Attack += 10;
        }
    }

    interface BattleItem : IItem
    {
        void Use(IMonster monster);
    }

    class Bomb : BattleItem
    {
        public string Name { get; } = "Bomb";
        public float Damage { get; } = 30.0F;

        public void Use(IMonster monster)
        {
            monster.TakeDamage(Damage);
        }
    }
}
