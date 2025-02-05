using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Inventory(IPlayer player)
    {
        IPlayer Player = player;

        List<Potion> potions = new();
        List<BattleItem> battleItems = new();

        public void AddPotion(Potion item) 
        {
            potions.Add(item);
        }

        public void AddBattleItem(BattleItem battleItem)
        {
            battleItems.Add(battleItem);
        }

        public void Use(int index, IMonster? target)
        {
            if (index < potions.Count)
            {
                potions[index].Use(Player);
                potions.RemoveAt(index);
            }
            else
            {
                battleItems[index - potions.Count].Use(target);
                battleItems.RemoveAt(index - potions.Count);
            }
        }

        public string GetItemInfo()
        {
            string info = "";
            int i = 0, j = 0;
            for(i = 0; i < potions.Count; i++)
            {
                info += $"{i}. {potions[i].Name} ";
            }
            for (j = 0; j < battleItems.Count; j++)
            {
                info += $"{i + j + 1}. {battleItems[j].Name} ";
            }
            Console.WriteLine(potions.Count);
            return info;
        }

    }
}
