using System.IO;
using System.Text.Json;

List<Goose> storeGooses = new();
string fileName = "Store.json";

Game();

void Game()
{
}

void Greet()
{
    Console.WriteLine("Welcome to Goose City!\n");

    if (CheckForFile())
    {
        Console.WriteLine("Do you want to load your save? (Saved: {0})", File.GetLastAccessTime(fileName));
        Console.WriteLine("(y/n)");

        bool completed = false;

        while (!completed)
        {
            string? answer = Console.ReadLine();
            if (answer?.ToLower() == "y")
            {
                completed = true;
            }
            else if (answer?.ToLower() == "n")
            {
                completed = true;
            }
        }
    }
    else
    {
        Console.WriteLine("No saves found! Press any key to continue.");
        Console.ReadLine();
    }
}

bool CheckForFile()
{
    return File.Exists(fileName);
}