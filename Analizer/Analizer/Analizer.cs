using DRSTool.FileHelper;
using DRSTool.CommonModels;

namespace DRSTool.Analizer;

class Analizer
{
    private AnalizerModel model;

    private List<tempModel> results;

    /// <summary>
    /// to be used only internaly, do not forget to initialize the field "model"
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Analizer()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        results = new List<tempModel>();
    }

    public Analizer(AnalizerModel model) : this()
    {
        if (model == null)
            throw new Exception("Model cannot be null");
        this.model = model;
    }

    public Analizer(string modelFilePath) : this()
    {
        AnalizerCompresedModel? cm = new FileHelperFactory().getFileHelper(modelFilePath).getContent<AnalizerCompresedModel>(modelFilePath);

        if (cm == null)
            throw new Exception("File model could not be read");

        model = AnalizerModel.decompress(cm);
    }

    public void analize()
    {
        for (int i = 0; i < model.Entities.Length; i++)
            for (int j = 0; j < model.Entities.Length; j++)
                if(model.Relations[i,j] != null){
                    var rel = model.Relations[i, j].Properties;
                    if (!rel.ContainsKey("inheritance")) continue;
                    if (!rel.ContainsKey("cochanges")) continue;
                    //if (rel["cochanges"] < 4) continue;
                    results.Add(new tempModel {
                                                DerivedName = model.Entities[i].Name,
                                                BaseName = model.Entities[j].Name,
                                                cochangeTimes = rel["cochanges"]
                                              });
                }
    }

    public void saveResults()
    {
        IFileHelper fileWriter = new FileHelperFactory().getCsvHelper();
        fileWriter.writeContent("analizerResults.csv", results);
    }

    public void writeInConsole()
    {
        foreach(var model in results)
        {
            Console.WriteLine($"base: {model.BaseName}, Derived: {model.DerivedName} changed together {model.cochangeTimes} times");
        }
    }
}

internal class tempModel
{
    public string BaseName { get; set; } = "";
    public string DerivedName { get; set; } = "";
    public int cochangeTimes { get; set; }
}
