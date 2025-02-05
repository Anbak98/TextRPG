using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    internal interface IStage
    {
        TextWriter TextWriter { get; }
        IPlayer Player { get; }
        IMonster Monster { get; }
        IStage Start();
    }

    class MainMenu(IPlayer player) : IStage
    {
        public TextWriter TextWriter { get; set; } = new();

        public IPlayer Player { get; private set; } = player;

        public IMonster Monster { get; } = new Goblin();

        public IItem Reward { get; set; }

        public IStage Start()
        {
            ChooseCharacter();
            while (true)
            {
                Console.WriteLine(">>> 1. 던전 진입!");
                Console.WriteLine(">>> 2. 캐릭터 다시 고르기!");
                ConsoleKeyInfo choice = Console.ReadKey(true);
                Console.Clear();
                if (choice.Key == ConsoleKey.D1)
                    return new NormalStage(Player);
                else if (choice.Key == ConsoleKey.D2) 
                    ChooseCharacter();
            }            
        }

        void ChooseCharacter()
        {
            Console.WriteLine("# 전체 화면 안 하면 망가지는 게임!");
            TextWriter.WaitSymbol(5, 50, 50);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================================");
            Console.WriteLine("||            캐릭터를 선택해주세요           ||");
            Console.Write("||    ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("1. 전사");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                                 ||");
            Console.WriteLine("================================================\n");
            Console.ResetColor();
            ConsoleKeyInfo choice = Console.ReadKey(true);
            switch (choice.Key)
            {
                case ConsoleKey.D1:
                    Player = new Warrior();
                    TextWriter.OneByOne("전사를 선택하셨습니다!", 50);
                    Console.WriteLine();
                    TextWriter.OneByOne(Player.GetInfo(), 30);
                    Console.WriteLine();
                    break;
            }
        }
    }

    class NormalStage(IPlayer player) : IStage
    {
        public List<KeyValuePair<TEXT_TYPE, string>> texts { get; private set; }

        public TextWriter TextWriter { get; set; } = new();

        public IPlayer Player { get; set; } = player;

        public IMonster Monster { get; private set; } = new Goblin();


        public IStage Start()
        {
            int monsterCursorLeft, monsterCursorTop;
            int playerCursorLeft, playerCursorTop;

            TextWriter.WaitSymbol(2, 30, 50);
            Console.Clear();
            Console.WriteLine("       -..-~..,.       \r\n    -;:~~::~....    \r\n   .!*;~~:;!;:;;.   \r\n  .~!*!;;;*=!;*;    \r\n  ;~,~==!!*=!--;.   \r\n :!  .!*;!!!~  !;.  \r\n *: ,!=$*!!!,  -;;  \r\n!- ~;**==***!,  ;!;.\r\n: ~;*!!*=$==;:.~.:;;\r\n  :!*;=**=*;,  ~ :! \r\n  .;; -;;!!;-    -! \r\n   .!,., .;!;-   ,  \r\n   ~!~    ,:::.     \r\n -~:-.     .,;~     \r\n !;.         -:-    \r\n ,.           ~!;   \r\n              :!~-  \r\n                    \r\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("================================================");
            Console.WriteLine("||                고블린 등장!!               ||");
            Console.Write("||    ");
            monsterCursorLeft = Console.CursorLeft;
            monsterCursorTop = Console.CursorTop;
            Console.Write($"      HP: {Monster.Health}         ATK: {Monster.Attack}      ");
            Console.WriteLine("    ||");
            Console.WriteLine("================================================\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================================");
            Console.WriteLine("||                이것은 당신!!               ||");
            Console.Write("||    ");
            playerCursorLeft = Console.CursorLeft; 
            playerCursorTop = Console.CursorTop;
            Console.Write($"      HP: {Player.Health}         ATK: {Player.Attack}      ");
            Console.WriteLine("    ||");
            Console.WriteLine("================================================\n");
            Console.WriteLine("# 출력 로그가 콘솔 윈도우 사이즈를 넘어가면 안 되요!");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("1. 공격  2. 스킬  3. 아이템  4. 도망");
            Console.ResetColor();

            Action<int, int, ICharacter> UpdateHP = (x, y, target) =>
            {
                int lastCursorLeft = Console.CursorLeft;
                int lastCursorTop = Console.CursorTop;

                Console.SetCursorPosition(x, y);
                Console.Write("                                       "); // 기존 값 덮어쓰기

                Console.SetCursorPosition(x, y);
                Console.Write($"    HP: {target.Health}         ATK: {target.Attack}    ");

                Console.SetCursorPosition(lastCursorLeft, lastCursorTop);
            };

            while (true)
            {
                Console.WriteLine();
                if (!TakePlayerAction()) break;
                UpdateHP(monsterCursorLeft, monsterCursorTop, Monster);

                Console.WriteLine();
                TakeMonsterAction();
                UpdateHP(playerCursorLeft, playerCursorTop, Player);

                if (Player.IsDead)
                {
                    return new Exit();
                }
                if (Monster.IsDead)
                {
                    HealthPotion Reward = new HealthPotion();
                    Console.WriteLine();
                    TextWriter.OneByOne($"빰빠ㅏ빠빰! {Monster.Name}을 물리쳤습니다!", 30);
                    Console.WriteLine();
                    TextWriter.OneByOne($"돈 + 500, 아이템 획득: {Reward.Name} ", 30);
                    Player.Money += 500.0F;
                    Player.Inventory.AddPotion(Reward);
                    Console.WriteLine();
                    Console.WriteLine("1. 상점");
                    Console.WriteLine("2. 고블린 사냥");
                    Console.WriteLine("3. 드래곤 사냥");
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                return new Store(Player);
                            case ConsoleKey.D2:
                                return new NormalStage(Player);
                            case ConsoleKey.D3:
                                return new HardStage(Player);
                        }
                    }
                }
            }

            return new MainMenu(Player);
        }

        bool TakePlayerAction()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) 
            {
                case ConsoleKey.D1:
                    Monster.TakeDamage(Player.Attack);
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextWriter.OneByOne($"{Player.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"는 ", 50);
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextWriter.OneByOne($"{Monster.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"에게 ", 50);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    TextWriter.OneByOne($"{Player.Attack}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"의 데미지를 입혔다!! ", 50);
                    break;
                case ConsoleKey.D2:
                    Player.Skill(Monster);
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextWriter.OneByOne($"{Player.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"는 ", 50);
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextWriter.OneByOne($"{Monster.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"에게 스킬을 사용해서", 50);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    TextWriter.OneByOne($"{Player.Attack*2}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"의 데미지를 입혔다!! ", 50);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("보유한 아이템: " + Player.Inventory.GetItemInfo());
                    Console.WriteLine("사용할 아이템의 번호를 입력해라!");
                    Player.Inventory.Use(int.Parse(Console.ReadLine()), Monster);
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine();
                    TextWriter.OneByOne($"당신은 도망쳤다!", 50);
                    Console.ReadKey();
                    return false;
            }
            return true;
        }

        bool TakeMonsterAction()
        {
            Player.TakeDamage(Monster.Attack);
            Console.ForegroundColor = ConsoleColor.Red;
            TextWriter.OneByOne($"{Monster.Name}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"는 ", 50);
            Console.ForegroundColor = ConsoleColor.Green;
            TextWriter.OneByOne($"{Player.Name}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"에게 ", 50);
            Console.ForegroundColor = ConsoleColor.Yellow;
            TextWriter.OneByOne($"{Monster.Attack}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"의 데미지를 입혔다!! ", 50);
            return true;
        }
    }
    class HardStage(IPlayer player) : IStage
    {
        public List<KeyValuePair<TEXT_TYPE, string>> texts { get; private set; }

        public TextWriter TextWriter { get; set; } = new();

        public IPlayer Player { get; set; } = player;

        public IMonster Monster { get; private set; } = new Dragon();

        public IItem Reward { get; } = new HealthPotion();

        public IStage Start()
        {
            int monsterCursorLeft, monsterCursorTop;
            int playerCursorLeft, playerCursorTop;

            TextWriter.WaitSymbol(2, 30, 50);
            Console.Clear();
            Console.WriteLine("\r\n\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,-!*!:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:*;:!:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:*;::;!,,,,,,,,,,,,-,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;=:::::*,,,,,,,-:;!=#;,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,-!!:::::;*:,,-:;!;;::;!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,;*::::;!!!$~!=;~---:;*~,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,~*::::;!!!!==:--~:!**!=-,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,-~;*-,-$;:::;!!!*=;-~;!!~-~:;!,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,;*-,,~*~-*-,;;:::;!!!**~-;!~.  ,::;;,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,-!~;-,-!. :~,-*::::;!!!=~-:;     -::;*,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,-*,.;,~*, -;,,:!::::!!!$:--;-     ,::;=-,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,::.~;:!*:-!-,,!!::::!!!*---~!~,.   ~:;*~,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,!#=!*!~,,-;*-,,!;::::!!*;----~:!=$$$#=:!;,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,-;!:,,,,,,----:;,,!;::::!!*:--~::;;!;;;;;!!*-,,,,,,,,,,,,,,,,,,\r\n,,,,,,-!;--------~~---*-,;!::::;!*:-:*;-,-;;:;**;~~,,,,,,,,,,,,,,,,,,,\r\n,,,,,,!:-----------~--~;,-*::::;!*;-*,   ~::!*~-,,,,,,,,,,,,,,,,,,,,,,\r\n,,,,~=:-~$;------------**$$=;::;!*!-!~  .::;=-,,,,,,,,,,,,,,,,,,,,,,,,\r\n,,-!;---~$*-------------!,~$!:::!!!-~!~ .::*~,,,,,,,,,,,,,,,,,,,,,,,,,\r\n,,;;----:=:~~~----------~*!~*:::!!*--~!!~:;*-,,,,,,,,,,,,,,,,,,,,,,,,,\r\n,:;------~-~~~-----------:!,;;::!!*--::;!!;!,,,,,,,,,,,,,-,,,-!-,,,,,,\r\n-*~-----------------------;!;=!;!*!-:::;!**;,,,,,,,,,,,,;*:~!*$~,-~-,,\r\n~;--------------,...,:-----:$~,:$*:~::*=:--,,,,,,,,,-,,:!***!***!!!*,,\r\n~:=~--------~:.     .!------~!;**$~~;*;-,,,,,,,,,,,;!~:=:~--,---:!;:,,\r\n::--------~~=,      :!----~~-,*=*=~;=:,-~~-,,,--,,::~=;,,----~~---$-,,\r\n~!!!;;!!!!!:.      ,$;----~:~--~=;~!;-;;~*~,,:!=~-!:!~----~;!!!!;:-!-,\r\n,-!-..::;.        .;*:------~---!~:$:!~ -;,,::.!~!*;--~--~*;-,,-:!!!;,\r\n,,~!  .!-        ,:*::------~--~!-*:~;*=#;:;=!=*:---~~~-~*~,,,,,,,-;~,\r\n,,,;;-----------~!=;:~---------~~~*-,,,-~:;::~-,,-------;;,,,,,,,,,,,,\r\n,,,,~!*********!=!:::~------------~--------------------~!-,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,~!:::--------------------~~~~----------!:,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,!~:~---------------------------------:*,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,!-:--------------------------------,-!-,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,;~~--------------------------------,;:,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,~!!-------------------------------,~*-,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,-*=------------------------------,,!~,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,;=------:~---------------------,,;;,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,-=------!;----------~--~-----~:,~*-,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,!---,-~*;---------;!~:~~-----*~!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,-!--~~::=:--------~*~---------:$;,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,-!--~:;**~--------;!----------~!;,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,-!--~:!!:--------~*;,---~~~---~;*,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,~;--~:*-..,------~!;,-~:;;;:~-~:!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,~:--~*;-.   .....,~!-~::::::::~:!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,::--:$*-,.       .-*~~::::::::::!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,;:-~!:!=;-,,     .,;;:::::::::::!-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,~;-~!~;;**!:~--,,--~$;::::::::::*-,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,*~-;*~!::!=;!*===***=!::::::::;!,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,-!*!$~*;::*;,--!*:::;**!;::;::!;,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,-;:,:=;;;*,,,,;*;::::*$#$=:;*~,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,~:;;:,,,,,~!!!:::*;~=:!!,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:*::;!,~=;=-,,,,,,,,,,,,,,,,,,,,\r\n,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,!;:;*-,-!=~,,,,,,,,,,,,,,,,,,,,,\r\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("================================================");
            Console.WriteLine("||                드래곤 등장!!               ||");
            Console.Write("||    ");
            monsterCursorLeft = Console.CursorLeft;
            monsterCursorTop = Console.CursorTop;
            Console.Write($"      HP: {Monster.Health}         ATK: {Monster.Attack}      ");
            Console.WriteLine("    ||");
            Console.WriteLine("================================================\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================================");
            Console.WriteLine("||                이것은 당신!!               ||");
            Console.Write("||    ");
            playerCursorLeft = Console.CursorLeft;
            playerCursorTop = Console.CursorTop;
            Console.Write($"      HP: {Player.Health}         ATK: {Player.Attack}      ");
            Console.WriteLine("    ||");
            Console.WriteLine("================================================\n");
            Console.WriteLine("# 출력 로그가 콘솔 윈도우 사이즈를 넘어가면 안 되요!");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("1. 공격  2. 스킬  3. 아이템  4. 도망");
            Console.ResetColor();

            Action<int, int, ICharacter> UpdateHP = (x, y, target) =>
            {
                int lastCursorLeft = Console.CursorLeft;
                int lastCursorTop = Console.CursorTop;

                Console.SetCursorPosition(x, y);
                Console.Write("                                       "); // 기존 값 덮어쓰기

                Console.SetCursorPosition(x, y);
                Console.Write($"    HP: {target.Health}         ATK: {target.Attack}    ");

                Console.SetCursorPosition(lastCursorLeft, lastCursorTop);
            };

            while (true)
            {
                Console.WriteLine();
                TakePlayerAction();
                UpdateHP(monsterCursorLeft, monsterCursorTop, Monster);

                Console.WriteLine();
                TakeMonsterAction();
                UpdateHP(playerCursorLeft, playerCursorTop, Player);

                if (Player.IsDead)
                {
                    return new Exit();
                }
                if (Monster.IsDead)
                {
                    Console.WriteLine();
                    TextWriter.OneByOne($"빰빠ㅏ빠빰! {Monster.Name}을 물리쳤습니다!", 30);
                    Console.WriteLine();
                    TextWriter.OneByOne($"돈 + 500, 아이템 획득: {Reward.Name} ", 30);
                    Player.Money += 500.0F;
                    Console.WriteLine();
                    Console.WriteLine("1. 상점");
                    Console.WriteLine("2. 고블린 사냥");
                    Console.WriteLine("3. 드래곤 사냥");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            return new Store(Player);
                        case ConsoleKey.D2:
                            return new NormalStage(Player);
                        case ConsoleKey.D3:
                            return new HardStage(Player);
                    }
                }
            }

            return new MainMenu(Player);
        }

        bool TakePlayerAction()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Monster.TakeDamage(Player.Attack);
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextWriter.OneByOne($"{Player.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"는 ", 50);
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextWriter.OneByOne($"{Monster.Name}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"에게 ", 50);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    TextWriter.OneByOne($"{Player.Attack}", 50);
                    Console.ResetColor();
                    TextWriter.OneByOne($"의 데미지를 입혔다!! ", 50);
                    break;
            }
            return true;
        }

        bool TakeMonsterAction()
        {
            Player.TakeDamage(Monster.Attack);
            Console.ForegroundColor = ConsoleColor.Red;
            TextWriter.OneByOne($"{Monster.Name}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"는 ", 50);
            Console.ForegroundColor = ConsoleColor.Green;
            TextWriter.OneByOne($"{Player.Name}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"에게 ", 50);
            Console.ForegroundColor = ConsoleColor.Yellow;
            TextWriter.OneByOne($"{Monster.Attack}", 50);
            Console.ResetColor();
            TextWriter.OneByOne($"의 데미지를 입혔다!! ", 50);
            return true;
        }
    }

    class Store(IPlayer player) : IStage
    {
        public TextWriter TextWriter { get; } = new();

        public IPlayer Player { get; set; } = player;

        public IMonster Monster => throw new NotImplementedException();

        public IItem Reward => throw new NotImplementedException();

        public IStage Start()
        {
            Console.Clear();
            TextWriter.OneByOne("어서오게 모험가요, 무얼 사고 싶은가?", 30);
            Console.WriteLine("1. 회복 물약 100$");
            Console.WriteLine("2. 힘의 물약 500$");
            Console.WriteLine("3. 폭탄 700$");
            Console.WriteLine("4. 사냥가자~");
            Console.WriteLine($"당신의 소지금: {Player.Money}");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        if (Player.Money > 100)
                        {
                            Player.Money -= 100;
                            Console.WriteLine("회복 물약 구매! -100$");
                            Player.Inventory.AddPotion(new HealthPotion());
                        }
                        else
                        {
                            TextWriter.OneByOne("소지금이 부족하구만~", 30);
                        }
                        break;
                    case ConsoleKey.D2:
                        if (Player.Money > 500)
                        {
                            Player.Money -= 500;
                            Console.WriteLine("힘의 물약 구매! -500$");
                            Player.Inventory.AddPotion(new StrengthPotion());
                        }
                        else
                        {
                            TextWriter.OneByOne("소지금이 부족하구만~", 30);
                        }
                        break;
                    case ConsoleKey.D3:
                        if (Player.Money > 700)
                        {
                            Player.Money -= 700;
                            Console.WriteLine("폭탄 구매! -700$");
                            Player.Inventory.AddBattleItem(new Bomb());
                        }
                        else
                        {
                            TextWriter.OneByOne("소지금이 부족하구만~", 30);
                        }
                        break;
                    case ConsoleKey.D4:
                        return new NormalStage(Player);
                    default:
                        break;
                }

            }
        }
    }

    class Exit : IStage
    {

        public TextWriter TextWriter => throw new NotImplementedException();

        public IPlayer Player => throw new NotImplementedException();

        public IMonster Monster => throw new NotImplementedException();

        public IItem Reward => throw new NotImplementedException();

        public IStage Start()
        {
            throw new NotImplementedException();
        }
    }
}
