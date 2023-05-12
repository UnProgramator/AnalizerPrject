using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper.Implementation;
using CsvHelper.Configuration.Attributes;

namespace DRSTool.Extractor.DataExtraction.RebuturiPeMoment;

class HierarchyExtractor
{
    public static void extract(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<HierarchyModel>(filePath);

        if (input == null)
            throw new Exception("hyerarchy file read error");

        foreach (var iter in input)
        {
            model.addStructuralRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("inheritance", true));
        }
    }
}

internal class HierarchyModel
{
    //source,target,Hierarchy-Specific SRelations

    [Name("source")]
    public string Source { get; set; } = "";

    [Name("target")]
    public string Target { get; set; } = "";

    [Name("Hierarchy-Specific Relations")]
    public int relations { get; set; }
}
