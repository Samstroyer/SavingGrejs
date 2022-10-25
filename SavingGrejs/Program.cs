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
        Console.WriteLine("Press 'a' to add a goose\nPress 'l' to list all geese\nPress 's' to save\nPress 'o' to load from Store.json");

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
        else if (ans?.ToLower() == "s")
        {
            Console.Clear();
            if (storeGooses.Any())
            {
                saveToFile();
                Console.WriteLine("Saved content!");
            }
            else
            {
                Console.WriteLine("There is nothing to save!");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        else if (ans?.ToLower() == "o")
        {
            if (File.Exists(fileName))
            {
                LoadStoreItems();
                Console.WriteLine("Store items loaded!");
            }
            else
            {
                Console.WriteLine("There is no Store.json to load from!");
                if (File.Exists("backup.json"))
                {
                    Console.WriteLine("Backup detected, will try to load backup");
                    storeGooses = new();
                    foreach (Goose g in JsonSerializer.Deserialize<List<Goose>>("backup.json"))
                    {
                        storeGooses.Add(g);
                    }
                }
            }
        }
    }
}

void ListGoose()
{
    if (storeGooses.Any())
    {
        foreach (Goose g in storeGooses)
        {
            Console.WriteLine($"Goose {g.Name} is {g.Age} years old. Has owner: {g.IsBought}");
        }
        Console.WriteLine("Press any key to continue");
    }
    else
    {
        Console.WriteLine("There are no geese to list!\nPress any key to continue");
    }
    Console.ReadKey();
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
    } while (owned?.ToLower() != "n" && owned?.ToLower() != "y");

    bool hasOwner = false;
    string ownerText = "It has no owner.";
    if (owned == "y") { hasOwner = true; ownerText = "It has a owner"; }

    Console.WriteLine("How old is your new goose? (0-99)");
    int ageInt = 0;
    bool done = false;
    do
    {
        done = int.TryParse(Console.ReadLine(), out ageInt);

        if (!done)
        {
            Console.WriteLine("Age must be between 0 and 99");
        }
        else
        {
            Console.WriteLine("Your new goose age is {0}", ageInt);
        }

    } while (!done);

    storeGooses.Add(new Goose(ageInt, name, hasOwner));

    Console.WriteLine($"Goose {name}, aged {ageInt} has been added! {ownerText}");
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

void saveToFile()
{
    string fileContent = JsonSerializer.Serialize<List<Goose>>(storeGooses);

    if (CheckForFile())
    {
        if (File.Exists("backup.json"))
        {
            File.Delete("backup.json");
            File.Copy(fileName, "backup.json");
        }
        else
        {
            File.Copy(fileName, "backup.json");
        }
        File.Delete(fileName);
    }
    File.WriteAllText(fileName, fileContent);
}

enum States
{
    Store,
    Editor
}