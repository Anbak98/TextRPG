namespace ConsoleApp4
{
    static class Program
    {
        public static GameManager gameManager = new();
        static void Main(string[] args)
        {
            gameManager.Play();
        }
    }

    class GameManager
    {
        public IPlayer Player { get; set; } = new Warrior();

        public void Play()
        {
            IStage stage = new MainMenu(Player);

            Player.Inventory.AddPotion(new HealthPotion());
            Player.Inventory.AddBattleItem(new Bomb());

            while (true)
            {
                stage = stage.Start();
                if (stage.GetType() == typeof(Exit)) 
                {
                    return;
                }
            }
        }
    }
}
