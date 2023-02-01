// See https://aka.ms/new-console-template for more information
using Analizer.Entractor;

internal class Program
{
    public static void Main(string[] args)
    {
        new Extractor().save();

        Console.WriteLine();
    }
}