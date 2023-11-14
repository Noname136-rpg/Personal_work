using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Text;

internal class Personal
{
    //private static Character player;
    public enum SceneStat {
        LoadingScene,
        DisplayGameIntro,
        DisplayMyInfo,
        DisplayInventory,
        DisplayMarket,
        DisplayMarketSell,
        DisplayDungeon,
        SaveScene,
        Error
    }

    private static SceneStat sceneStat = Personal.SceneStat.DisplayGameIntro;
    public static GameManager gm = new GameManager();
    public static Character Player;
    static void Main(string[] args)
    {
        GameDataSetting();
        ChooseSave();

        Player.InventroyTest(gm.item_list[1]);
        Player.InventroyTest(gm.item_list[3]);

        //Console.ReadLine();

        //줄맞추기는 패딩이 필요함
        //stirng을 출력할때 .LPAD RPAD 한쪽에다 글을 채움
        //문제는 콘솔창에서 한글이 2배로 인색된다.
        //또한 폰트가 일정하지 않을 수 있다.
        while (true)
        {
            switch (sceneStat)
            {
                case SceneStat.DisplayGameIntro:
                    {
                        DisplayGameIntro();
                        break;
                    }
                case SceneStat.DisplayMyInfo:
                    {
                        DisplayMyInfo();
                        break;
                    }
                case SceneStat.DisplayInventory:
                    {
                        DisplayInventory();
                        break;
                    }
                case SceneStat.DisplayMarket:
                    {
                        DisplayMarket();
                        break;
                    }
                case SceneStat.DisplayMarketSell:
                    {
                        DisplayMarketSell();
                        break;
                    }
                case SceneStat.DisplayDungeon:
                    {
                        DisplayDungeon();
                        break;
                    }
                case SceneStat.SaveScene:
                    {

                        break;
                    }
            }

        }


        //델리 케이트 > 함수 자체 저장
        //유니티에선 액션
    }

    static void GameDataSetting()
    {
        // 캐릭터 정보 세팅
        gm.LoadPlayers();

        // 아이템 정보 세팅
        gm.LoadItems();

        // 상점 정보 세팅 (gm의 아이템 리스트 copy)
        gm.LoadMarket();
    }

    static void ChooseSave()
    {
        for (int save_cnt = 0; save_cnt < gm.characters.Count; save_cnt++)
        {
            Character print_character = gm.characters[save_cnt];
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}. {5}, {6}, {7}\n", save_cnt+1, print_character.Name, print_character.Job, print_character.Level, print_character.Atk, print_character.Def, print_character.Hp, print_character.Gold);
        }

        Console.WriteLine();
        Console.WriteLine("원하시는 세이브 파일을 선택하세요.");
        int input = CheckValidInput(1, gm.characters.Count);
        Player = gm.characters[input-1];
        Console.WriteLine("Player Select!");
        Thread.Sleep(1000);
    }

    static void DisplayGameIntro()
    {
        Console.Clear();

        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 전전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전");
        Console.WriteLine("5. 저장");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int input = CheckValidInput(1, 5);
        switch (input)
        {
            case 1:
                sceneStat = SceneStat.DisplayMyInfo;
                break;

            case 2:
                sceneStat = SceneStat.DisplayInventory;
                break;

            case 3:
                sceneStat = SceneStat.DisplayMarket;
                break;

            case 4:
                sceneStat = SceneStat.DisplayDungeon;
                break;

            case 5:
                sceneStat = SceneStat.SaveScene;
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine("Lv.{0}",Player.Level.ToString("D2"));
        Console.WriteLine($"{Player.Name} ({Player.Job})");
        if((Player.PlayerAtk() - Player.Atk)==0)
        {
            Console.WriteLine($"공격력 :{Player.Atk}");
        }
        else
        {
            Console.WriteLine($"공격력 :{Player.Atk} (+{Player.PlayerAtk() - Player.Atk})");
        }
        if ((Player.PlayerDef() - Player.Def) == 0)
        {
            Console.WriteLine($"방어력 : {Player.Def}");
        }
        else
        {
            Console.WriteLine($"방어력 : {Player.Def} (+{Player.PlayerDef() - Player.Def})");
        }
        Console.WriteLine($"체력 : {Player.Hp}");
        Console.WriteLine($"Gold : {Player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기\n");
        Console.Write("원하시는 행동을 입력해주세요\n>>");
        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                sceneStat = SceneStat.DisplayGameIntro;
                break;
        }
    }

    static void DisplayInventory()
    {
        Console.Clear();
        //string str = " ";
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]\n");

        if (Player.Inventory.Count == 0)
        {
            Console.WriteLine("인벤토리가 비었습니다.");
        }
        else
        {
            for (int inven_cnt = 0; inven_cnt < Player.Inventory.Count; inven_cnt++)
            {
                if (Player.Inventory[inven_cnt].Equip == false)
                {
                    Console.WriteLine("{0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt + 1, Player.Inventory[inven_cnt].Name.PadRight(10, ' '), (Player.Inventory[inven_cnt].Atk).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Def).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Annot).ToString().PadRight(30, ' '));
                }
                else
                {
                    Console.WriteLine("[E] {0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt + 1, Player.Inventory[inven_cnt].Name.PadRight(10, ' '), (Player.Inventory[inven_cnt].Atk).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Def).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Annot).ToString().PadRight(30, ' '));
                }
            }
        }

        Console.WriteLine("\n1. 장착관리");
        Console.WriteLine("0. 나가기\n");
        Console.Write("원하시는 행동을 입력해주세요\n>>");

        int input = CheckValidInput(0, 1);
        switch (input)
        {
            case 0:
                sceneStat = SceneStat.DisplayGameIntro;
                break;
            case 1:
                if(Player.Inventory.Count==0)
                {
                    Console.Write("장착 / 해제할 아이템이 없습니다.");
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.Write("장착 / 해제할 아이템을 입력해주세요\n>>");
                    int select_inventroy = CheckValidInput(1, Player.Inventory.Count);
                    if (Player.Inventory[select_inventroy - 1].Equip == true)
                    {
                        Player.Inventory[select_inventroy - 1].Equip = false;
                    }
                    else
                    {
                        Player.Inventory[select_inventroy - 1].Equip = true;
                    }
                }
                break;
        }

    }

    static void DisplayDungeon()
    {
        Console.Clear();

        Console.WriteLine("던전 입장");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
        Console.WriteLine("1. 쉬운 던전   | 방어력 5 이상 권장");
        Console.WriteLine("2. 일반던전    | 방어력 11 이상 권장");
        Console.WriteLine("3. 어려운 던전  | 방어력 17 이상 권장");
        Console.WriteLine("0. 나가기\n");
        Console.Write("원하시는 행동을 입력해주세요\n>>");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                sceneStat = SceneStat.DisplayGameIntro;
                break;
        }
    }

    static void DisplayMarket()
    {
        Console.Clear();

        Console.WriteLine("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{Player.Gold} \n");

        Console.WriteLine("[아이템 목록]");
        for (int market_cnt = 0; market_cnt < gm.Market_item.Count; market_cnt++)
        {
            if(gm.Market_item[market_cnt].Gold == -1)
            {
                Console.WriteLine($"- {market_cnt+1} {gm.Market_item[market_cnt].Name} | 공격력 +{gm.Market_item[market_cnt].Atk} | 방어력 +{gm.Market_item[market_cnt].Def} | {gm.Market_item[market_cnt].Annot} | 구매완료");
            }
            else
            {
                Console.WriteLine($"- {market_cnt+1} {gm.Market_item[market_cnt].Name} | 공격력 +{gm.Market_item[market_cnt].Atk} | 방어력 +{gm.Market_item[market_cnt].Def} | {gm.Market_item[market_cnt].Annot} | {gm.Market_item[market_cnt].Gold}G");
            }
        }
        Console.WriteLine("\n1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기\n");
        Console.Write("원하시는 행동을 입력해주세요\n>>");

        int input = CheckValidInput(0, 2);
        switch (input)
        {
            case 1:
                Console.Write("구매하고 싶은 아이템을 입력해주세요\n>>");
                int buy = CheckValidInput(1, gm.Market_item.Count);
                if(Player.Gold < gm.Market_item[buy-1].Gold)
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    Thread.Sleep(400);
                }
                else if(gm.Market_item[buy-1].Gold == -1)
                {
                    Console.WriteLine("이미 판매된 아이템입니다.");
                    Thread.Sleep(400);
                }
                else
                {
                    Player.BuyItem(gm.Market_item[buy - 1]);
                    gm.Market_item[buy - 1].Gold = -1;
                }
                break;

            case 2:
                sceneStat = SceneStat.DisplayMarketSell;
                break;

            case 0:
                sceneStat = SceneStat.DisplayGameIntro;
                break;
        }
    }

    static void DisplayMarketSell()
    {
        Console.Clear();

        Console.WriteLine("상점 - 아이템 판매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{Player.Gold} \n");

        if(Player.Inventory.Count==0)
        {
            Console.WriteLine("인벤토리가 비었습니다.");
        }
        else
        {
            for (int inven_cnt = 0; inven_cnt < Player.Inventory.Count; inven_cnt++)
            {
                if (Player.Inventory[inven_cnt].Equip == false)
                {
                    Console.WriteLine("{0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt + 1, Player.Inventory[inven_cnt].Name.PadRight(10, ' '), (Player.Inventory[inven_cnt].Atk).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Def).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Annot).ToString().PadRight(30, ' '));
                }
                else
                {
                    Console.WriteLine("[E] {0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt + 1, Player.Inventory[inven_cnt].Name.PadRight(10, ' '), (Player.Inventory[inven_cnt].Atk).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Def).ToString().PadRight(3, ' '), (Player.Inventory[inven_cnt].Annot).ToString().PadRight(30, ' '));
                }
            }
        }

        Console.WriteLine("\n0. 나가기\n");
        Console.Write("판매하고 싶은 아이템을 입력해주세요\n>>");

        int input = CheckValidInput(0, Player.Inventory.Count);
        switch (input)
        {
            case 0:
                sceneStat = SceneStat.DisplayGameIntro;
                break;
            default:
                Player.SellItem(input - 1);
                gm.Market_item[input - 1].Gold = gm.item_list[input - 1].Gold;
                break;
        }

    }

    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }
}

public class GameManager
{
    public List<Item> item_list = new List<Item>();
    public List<Character> characters = new List<Character>();
    public List<Item> Market_item = new List<Item>();

    public void LoadPlayers()
    {
        StreamReader Load_players = new StreamReader("save/save_list.txt");
        int total_save_num;

        if (Int32.TryParse(Load_players.ReadLine(), out total_save_num))
        {
            //error handle
        }

        for (int save_cnt = 0; save_cnt < total_save_num; save_cnt++)
        {
            string save_file_path = String.Concat("save/", Load_players.ReadLine());
            StreamReader temp_load_save = new StreamReader(save_file_path);
            string _name = temp_load_save.ReadLine();
            string _job = temp_load_save.ReadLine();
            int _level;
            Int32.TryParse(temp_load_save.ReadLine(), out _level);
            int _atk;
            Int32.TryParse(temp_load_save.ReadLine(), out _atk);
            int _def;
            Int32.TryParse(temp_load_save.ReadLine(), out _def);
            int _hp;
            Int32.TryParse(temp_load_save.ReadLine(), out _hp);
            int _gold;
            Int32.TryParse(temp_load_save.ReadLine(), out _gold);
            Character new_character = new Character(_name, _job, _level, _atk, _def, _hp, _gold);
            characters.Add(new_character);
            temp_load_save.Close();
        }

        /*for (int save_cnt = 0; save_cnt < total_save_num; save_cnt++)
        {
            Character print_character = characters[save_cnt];
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}. {5}, {6}\n", print_character.Name, print_character.Job, print_character.Level, print_character.Atk, print_character.Def, print_character.Hp, print_character.Gold);
        }*/

        Load_players.Close();
    }

    public void LoadItems()
    {
        StreamReader sr = new StreamReader("item/item_list.txt");
        int total_item_num;

        if (Int32.TryParse(sr.ReadLine(), out total_item_num))
        {
            //error handle
        }

        for (int file_cnt = 0; file_cnt < total_item_num; file_cnt++)
        {
            string item_file_path = String.Concat("item/", sr.ReadLine());
            StreamReader temp_load_item = new StreamReader(item_file_path);
            string _name = temp_load_item.ReadLine();
            int _atk;
            Int32.TryParse(temp_load_item.ReadLine(), out _atk);
            int _def;
            Int32.TryParse(temp_load_item.ReadLine(), out _def);
            string _annot = temp_load_item.ReadLine();
            int _gold;
            Int32.TryParse(temp_load_item.ReadLine(), out _gold);
            Item new_item = new Item(_name, _atk, _def, _annot, _gold);
            item_list.Add(new_item);
            temp_load_item.Close();
            //Console.WriteLine(sr.ReadLine());
        }

        /*for (int file_cnt = 0; file_cnt < total_item_num; file_cnt++)
        {
            Item print_item = item_list[file_cnt];
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}\n", print_item.Name, print_item.Atk, print_item.Def, print_item.Annot, print_item.Gold);
        }*/

        sr.Close();
    }

    public void LoadMarket()
    {
        foreach(Item item in item_list)
        {
            Item input_item = new Item(item.Name, item.Atk, item.Def, item.Annot, item.Gold);
            Market_item.Add(input_item);
        }
    }
}


public class Item
{
    public bool Equip { get; set; }
    public string Name { get; }
    public int Atk { get; }
    public int Def { get; }
    public string Annot { get; }
    public int Gold { get; set; }

    public Item(string _name, int _atk, int _def, string _annot, int _gold)
    {
        Equip = false;
        Name = _name;
        Atk = _atk;
        Def = _def;
        Annot = _annot;
        Gold = _gold;
    }
}

public class Character
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; }
    public int Gold { get; set; }

    //상점 미구현으로 인해 임시로 아이템을 넣어 두겠습니다.
    public List<Item> Inventory = new List<Item>();

    public void InventroyTest(Item item)
    {
        Inventory.Add(item);
    }

    public int PlayerAtk()
    {
        int totalAtk = Atk;

        foreach(Item item in Inventory)
        {
            if (item.Equip == true)
            {
                totalAtk += item.Atk;
            }
        }

        return totalAtk;
    }

    public int PlayerDef()
    {
        int totalDef = Def;

        foreach (Item item in Inventory)
        {
            if(item.Equip==true)
            {
                totalDef += item.Def;
            }
        }

        return totalDef;
    }

    public void BuyItem(Item item)
    {
        Gold -= item.Gold;
        Item input_item = new Item(item.Name, item.Atk, item.Def, item.Annot, item.Gold);
        Inventory.Add(input_item);
    }

    public void SellItem(int item_index)
    {
        Gold += Inventory[item_index].Gold*85/100;
        Inventory.RemoveAt(item_index);
    }
    public Character(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
    }
}