using DRSTool.FileHelper;
using DRSTool.CommonModels;
using DRSTool.Analizer.AntipatternsDetection;
using DRSTool.Analizer.Models;

namespace DRSTool.Analizer;

class Analizer
{
    private AnalizerModel model;

    private ResultModel results;


    /// <summary>
    /// to be used only internaly, do not forget to initialize the field "model"
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Analizer()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        results = new ResultModel();
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
        //interfaces/bases change togheter with thir derived
        for (int i = 0; i < model.Entities.Length; i++)
            for (int j = 0; j < model.Entities.Length; j++)
                if (model.Relations[i, j] != null)
                {
                    var rel = model.Relations[i, j].Properties;
                    if (!rel.ContainsKey("inheritance")) continue;
                    if (!rel.ContainsKey("cochanges")) continue;
                    if (rel["cochanges"] < 4) continue;
                    results.add(model.Entities[i].Name, new Dictionary<string, object>
                                                        { { "antipattern-type", "base and derived change too often" },
                                                          { "class", model.Entities[j].Name },
                                                          { "co-change times", rel["cochanges"] }
                                                        });
                }

        new UnstableInterfaceDetection().detect(model, results);
    }

    public void saveResults()
    {
        IFileHelper fileWriter = new FileHelperFactory().getJsonHelper();
        fileWriter.writeContent("analizerResults.json", results);
    }
}

internal class tempModel
{
    public string BaseName { get; set; } = "";
    public string DerivedName { get; set; } = "";
    public int cochangeTimes { get; set; }
}
