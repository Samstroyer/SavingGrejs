using System.IO;
using System.Text.Json;

List<Goose> storeGooses = new();
string fileName = "Store.json";

States gameState = States.Store;

Game();

void Game()
{
    Greet();
    Store();
}

void Store()
{
    bool inStore = true;
    while (inStore)
    {
        switch (gameState)
        {
            case States.Store:
                Console.WriteLine("Welcome to the Goose store!");
                Console.WriteLine("Type 'help' to see available commands.");
                string? ans = Console.ReadLine();
                if (ans == "help")
                {
                    DisplayCommands();
                }

                break;

            case States.Editor:
                GooseEditor();
                break;
        }


    }
}

void GooseEditor()
{
    bool inEditor = true;

    while (inEditor)
    {
        Console.WriteLine("Press 'a' to add a goose\nPress 'l' to list all geese");

        string? ans = Console.ReadLine();

        if (ans?.ToLower() == "a")
        {
            Console.Clear();
            AddGoose();
        }
        else if (ans?.ToLower() == "l")
        {
            Console.Clear();
            ListGoose();
        }
    }
}

void ListGoose()
{
    foreach (Goose g in storeGooses)
    {

    }
}

void AddGoose()
{
    string? name = "";
    while (name?.Length <= 0)
    {
        Console.WriteLine("Write the name of your goose:");
        name = Console.ReadLine();

        if (name?.Length > 0)
        {
            Console.WriteLine("The name is now: {0}", name);
        }
        else
        {
            Console.WriteLine("That is not a valid name...");
        }
        System.Threading.Thread.Sleep(2000);
        Console.Clear();
    }

    Console.WriteLine("Is your new goose owned? (y/n)");
    string? owned;
    do
    {
        owned = Console.ReadLine();
    } while (owned?.ToLower() != "n" || owned?.ToLower() != "y");

    bool hasOwner = false;
    string ownerText = "It has no owner.";
    if (owned == "y") { hasOwner = true; ownerText = "It has a owner"; }

    Console.WriteLine("How old is your new goose? (0-99)");
    string age = "";
    do
    {
        string? temp = Console.ReadLine();

        if (temp != null && temp?.Length > 0)
        {
            age = temp;
        }
        else
        {
            Console.WriteLine("Age must be between 0 and 99");
        }


    } while (age.All(char.IsNumber)); //Proud of this Regex, how it is supposed to be used :) //Samme

    storeGooses.Add(new Goose(int.Parse(age), name, hasOwner));

    Console.WriteLine($"Goose {name}, aged {age} has been added! {ownerText}");
    System.Threading.Thread.Sleep(2000);
    Console.Clear();
}

void DisplayCommands()
{
    Console.Clear();
    if (storeGooses.Any())
    {
        foreach (Goose g in storeGooses)
        {
            string temp;
            if (g.IsBought)
            {
                temp = "Has owner";
            }
            else
            {
                temp = "Has no owner";
            }
            Console.WriteLine($"Goose {g.Name}: {temp}, {g.Age} years old.");
        }
    }
    else
    {
        Console.WriteLine("There are no gooses available in the store!\nPress any key to enter the editor.");
        gameState = States.Editor;
    }
}

void Greet()
{
    Console.WriteLine("Welcome to Goose City Trade-Center!\n");

    if (CheckForFile())
    {
        bool completed = false;

        while (!completed)
        {

            Console.WriteLine("Do you want to load your save? (Saved: {0})", File.GetLastAccessTime(fileName));
            Console.WriteLine("(y/n)");

            string? ans = Console.ReadLine();
            if (ans?.ToLower() == "y")
            {
                completed = true;
                LoadStoreItems();
            }
            else if (ans?.ToLower() == "n")
            {
                completed = true;
            }
            Console.Clear();
        }
    }
    else
    {
        Console.WriteLine("No saves found! Press any key to continue.");
        Console.ReadLine();
    }
    Console.Clear();
}

void LoadStoreItems()
{
    foreach (var goose in JsonSerializer.Deserialize<List<Goose>>(fileName))
    {
        storeGooses.Add(goose);
    }
}

bool CheckForFile()
{
    return File.Exists(fileName);
}

enum States
{
    Store,
    Editor
}