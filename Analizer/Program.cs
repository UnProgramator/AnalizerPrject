// See https://aka.ms/new-console-template for more information
using DRSTool.Extractor;
using DRSTool.Analizer;
using DRSTool.Extractor.Config;
using System.Diagnostics;

namespace DRSTool;

internal class Program
{
    public static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        string configFile;

        Console.WriteLine(args.Length);

        if (args.Length > 0)
            configFile = args[0];
        else
            configFile = "AnalizerConfig_argouml2.json";
        
        ConfigValidator.creatConfig(configFile);
        var extractor = new Extractor.Extractor();
        extractor.save();

        var analizer = new Analizer.Analizer(extractor.model);
        analizer.analize();
        analizer.saveResults();
        analizer.saveAggregatedResults();
        
        sw.Stop();
        Console.WriteLine($"\nProgram terminated. Duration: {sw.Elapsed}");
    }
}