using DRSTool.FileHelper;
using DRSTool.CommonModels;

namespace DRSTool.Analizer;

class Analizer
{
    private AnalizerModel model;

    private List<tempModel> DerivedBaseChangeToOften;
    private List<Dictionary<string, object>> unstableInterfaces;


    /// <summary>
    /// to be used only internaly, do not forget to initialize the field "model"
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Analizer()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        DerivedBaseChangeToOften = new List<tempModel>();
        unstableInterfaces = new List<Dictionary<string, object>>();
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
                if(model.Relations[i,j] != null){
                    var rel = model.Relations[i, j].Properties;
                    if (!rel.ContainsKey("inheritance")) continue;
                    if (!rel.ContainsKey("cochanges")) continue;
                    if (rel["cochanges"] < 4) continue;
                    DerivedBaseChangeToOften.Add(new tempModel {
                                                DerivedName = model.Entities[i].Name,
                                                BaseName = model.Entities[j].Name,
                                                cochangeTimes = rel["cochanges"]
                                              });
                }

        //classes changes too often with other 

        for(int i=0; i< model.Entities.Length; i++)
        {
            int dependents = 0, cochanges = 0;
            for (int j = 0; j < model.Entities.Length; j++)
            {  
                if (model.Relations[j,i] != null)
                {
                    dependents++;
                    var rel = model.Relations[j,i].Properties;
                    if (rel.ContainsKey("cochanges"))
                    {
                        cochanges+= rel["cochanges"];
                        
                    }
                }
            }
            if (dependents > 4 && ((float)cochanges) / dependents >= 1)
            {
                unstableInterfaces.Add( new Dictionary<string, object> {
                    { "entity", model.Entities[i].Name },
                    { "dendents", dependents},
                    { "average co-changes", ((float)cochanges) / dependents}});
            }
        }
    }

    public void saveResults()
    {
        IFileHelper fileWriter = new FileHelperFactory().getJsonHelper();
        fileWriter.writeContent("analizerResults.csv", new Dictionary<string, object> { 
            { "hieracy-change-to-often", DerivedBaseChangeToOften }, 
            { "unstable-iterfaces", unstableInterfaces } });
    }
}

internal class tempModel
{
    public string BaseName { get; set; } = "";
    public string DerivedName { get; set; } = "";
    public int cochangeTimes { get; set; }
}
