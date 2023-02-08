// See https://aka.ms/new-console-template for more information
using DRSTool.Extractor;
using DRSTool.Analizer;

namespace DRSTool;

internal class Program
{
    public static void Main(string[] args)
    {
        var model = new Extractor.Extractor().model;

        var analizer = new Analizer.Analizer(model);
        analizer.analize();
        analizer.saveResults();

        Console.WriteLine();
    }
}