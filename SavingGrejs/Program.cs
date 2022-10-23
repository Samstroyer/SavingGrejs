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
    while (inStore)
    {
        Console.WriteLine("Welcome to the Goose store!");
        Console.WriteLine("Type 'help' to see available commands.");


    }
}



void Greet()
{
    Console.WriteLine("Welcome to Goose City!\n");

    if (CheckForFile())
    {
        bool completed = false;

        while (!completed)
        {

            Console.WriteLine("Do you want to load your save? (Saved: {0})", File.GetLastAccessTime(fileName));
            Console.WriteLine("(y/n)");

            string? answer = Console.ReadLine();
            if (answer?.ToLower() == "y")
            {
                completed = true;
                LoadStoreItems();
            }
            else if (answer?.ToLower() == "n")
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