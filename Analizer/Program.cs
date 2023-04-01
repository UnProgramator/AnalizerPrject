// See https://aka.ms/new-console-template for more information
using DRSTool.Extractor;
using DRSTool.Analizer;

namespace DRSTool;

internal class Program
{
    public static void Main(string[] args)
    {
        var extractor = new Extractor.Extractor();
        extractor.save();

        var analizer = new Analizer.Analizer(extractor.model);
        analizer.analize();
        analizer.saveResults();

        Console.WriteLine();
    }
}