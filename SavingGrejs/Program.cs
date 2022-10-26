using System.IO;
using System.Text.Json;

List<Goose> storeGooses = new();
string fileName = "Store.json";

Game();

void Game()
{
    Greet();
    Store();
}

void Store()
{
    bool inStore = true;
    Console.WriteLine("Welcome to the Goose store!");
    while (inStore)
    {
        GooseEditor();
    }
}

void GooseEditor()
{
    bool inEditor = true;

    while (inEditor)
    {
        DisplayCommands();
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
                Console.WriteLine("Saved geese appended!");
            }
            else
            {
                Console.WriteLine("There is no Store.json to append from!");
                if (File.Exists("backup.json"))
                {
                    Console.WriteLine("Backup detected, will try to append geese from backup");
                    storeGooses = new();
                    string jsonString = File.ReadAllText("backup.json");
                    foreach (Goose g in JsonSerializer.Deserialize<List<Goose>>(jsonString))
                    {
                        storeGooses.Add(g);
                    }
                }
            }
        }
        else if (ans?.ToLower() == "r")
        {
            storeGooses = new();
            Console.WriteLine("All geese removed!");
        }
        else if (ans?.ToLower() == "u")
        {
            Console.WriteLine("Load and replace geese started!");
            storeGooses = new();
            if (File.Exists(fileName))
            {
                LoadStoreItems();
                Console.WriteLine("Saved geese replaced the old ones!");
            }
            else
            {
                Console.WriteLine("There is no Store.json to replace geese from!");
                if (File.Exists("backup.json"))
                {
                    Console.WriteLine("Backup detected, will try to replace current geese with backup geese");
                    storeGooses = new();
                    string jsonString = File.ReadAllText("backup.json");
                    foreach (Goose g in JsonSerializer.Deserialize<List<Goose>>(jsonString))
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

    Console.WriteLine("Press 'a' to add a goose\nPress 'l' to list all geese\nPress 's' to save\nPress 'o' to append geese from Store.json\nPress 'r' to remove all geese\nPress 'u' to load and replace geese from Store.json");
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
    //Extra protection with File.Exists
    if (File.Exists(fileName))
    {
        string jsonString = File.ReadAllText(fileName);
        foreach (Goose g in JsonSerializer.Deserialize<List<Goose>>(jsonString))
        {
            storeGooses.Add(g);
        }
    }
    else
    {
        Console.WriteLine("Something bypassed loading system! Ignore this warning, it is just anti-crashing.");
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