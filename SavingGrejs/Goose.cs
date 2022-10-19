using System;

public class Goose
{
    public bool IsBought { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Goose()
    {

    }

    public Goose(int age, string name)
    {
        Age = age;
        Name = name;
        IsBought = false;
    }

    public void Quack()
    {
        Console.WriteLine("{0} quacked!", Name);
    }
}
