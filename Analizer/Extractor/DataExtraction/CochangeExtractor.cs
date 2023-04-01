using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper.Implementation;
using CsvHelper.Configuration.Attributes;
using DRSTool.CommonModels.Exceptions;

namespace DRSTool.Extractor.DataExtraction;

internal class CochangeExtractor
{
    public static void extract_number(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<CochangeModel>(filePath);

        if (input == null)
            throw new Exception("co-change commits number coupling file read error");

        foreach (var iter in input)
        {
            try
            {
                model.addHistoryRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("cochanges", iter.relations), true);
            }
            catch (EntityUsedButNotDeclaredException) { }
        }
    }

    public static void extract_percentage(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<CochangePercentageModel>(filePath);

        if (input == null)
            throw new Exception("co-change commits percentage file read error");

        foreach (var iter in input)
        {
            try { 
            model.addHistoryRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("cochanges_%", iter.relations), true);
            }
            catch (EntityUsedButNotDeclaredException) { }
        }
    }
}

internal class CochangeModel
{
    //source,target,Hierarchy-Specific SRelations

    [Name("source")]
    public string Source { get; set; } = "";

    [Name("target")]
    public string Target { get; set; } = "";

    [Name("Temporal Coupling (commits)")]
    public int relations { get; set; }
}

internal class CochangePercentageModel
{
    //source,target,Hierarchy-Specific SRelations

    [Name("source")]
    public string Source { get; set; } = "";

    [Name("target")]
    public string Target { get; set; } = "";

    [Name("Temporal Coupling Percentage (commits)")]
    public int relations { get; set; }
}
