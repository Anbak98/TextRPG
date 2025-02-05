using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal interface ICharacter
    {
        string Name { get; }
        float Health { get; set; }
        float Attack { get; set; }
        bool IsDead { get; }

        void TakeDamage(float damage);
        string GetInfo();
    }

    interface IPlayer : ICharacter
    {
        float Money { get; set; }
        void Skill(ICharacter target);
        Inventory Inventory { get; }
    }

    class Warrior : IPlayer
    {
        public string Name { get; set; } = "warrior";

        public float Health { get; set; } = 100.0F;

        public float Attack { get; set; } = 10.0F;

        public bool IsDead { get; set; } = false;
        public float Money { get; set; } = 1000.0F;

        public Inventory Inventory { get; set; }

        public Warrior()
        {
            Inventory = new(this);
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                IsDead = true;
            }
        }

        public void Skill(ICharacter target)
        {
            target.TakeDamage(Attack * 2);
        }

        public string GetInfo()
        {
            string info = $"HP: {Health} || ATK: {Attack}";
            return info;
        }
    }

    interface IMonster: ICharacter
    {

    }

    class Goblin : IMonster
    {
        public string Name { get; set; } = "Goblin";

        public float Health { get; set; } = 35.0F;

        public float Attack { get; set; } = 10.0F;

        public bool IsDead { get; set; } = false;

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                IsDead = true;
            }
        }
        public string GetInfo()
        {
            string info = $"HP: {Health} || ATK: {Attack}";
            return info;
        }
    }

    class Dragon : IMonster
    {
        public string Name { get; set; } = "Dragon";

        public float Health { get; set; } = 1000.0F;

        public float Attack { get; set; } = 100.0F;

        public bool IsDead { get; set; } = false;

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                IsDead = true;
            }
        }
        public string GetInfo()
        {
            string info = $"HP: {Health} || ATK: {Attack}";
            return info;
        }
    }
}
