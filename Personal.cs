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
        //player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
        gm.LoadPlayers();

        // 아이템 정보 세팅
        gm.LoadItems();
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
        Console.WriteLine($"{Player.Name}({Player.Job})");
        Console.WriteLine($"공격력 :{Player.Atk}");
        Console.WriteLine($"방어력 : {Player.Def}");
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

        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]\n");

        for(int inven_cnt=0; inven_cnt < Player.Inventory.Count; inven_cnt++)
        {
            if(Player.Inventory[inven_cnt].Equip == false)
            {
                Console.WriteLine("{0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt+1, Player.Inventory[inven_cnt].Name, Player.Inventory[inven_cnt].Atk, Player.Inventory[inven_cnt].Def, Player.Inventory[inven_cnt].Annot);
            }
            else
            {
                Console.WriteLine("[E]{0}. {1} | 공격력 +{2} | 방어력 +{3} | {4}", inven_cnt+1, Player.Inventory[inven_cnt].Name, Player.Inventory[inven_cnt].Atk, Player.Inventory[inven_cnt].Def, Player.Inventory[inven_cnt].Annot);
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
                Console.Write("장착 / 해제할 아이템을 입력해주세요\n>>");
                int select_inventroy = CheckValidInput(1, Player.Inventory.Count);
                if(Player.Inventory[select_inventroy-1].Equip == true)
                {
                    Player.Inventory[select_inventroy-1].Equip = false;
                }
                else
                {
                    Player.Inventory[select_inventroy-1].Equip = true;
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

}


public class Item
{
    public bool Equip { get; set; }
    public string Name { get; }
    public int Atk { get; }
    public int Def { get; }
    public string Annot { get; }
    public int Gold { get; }

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
    public int Gold { get; }

    //상점 미구현으로 인해 임시로 아이템을 넣어 두겠습니다.
    public List<Item> Inventory = new List<Item>();

    public void InventroyTest(Item item)
    {
        Inventory.Add(item);
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