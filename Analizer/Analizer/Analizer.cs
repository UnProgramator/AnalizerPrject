using DRSTool.FileHelper;
using DRSTool.CommonModels;
using DRSTool.Analizer.AntipatternsDetection;
using DRSTool.Analizer.Models;
using DRSTool.Analizer.AntipatternsDetection.Implementations;
using DRSTool.Extractor.Config;

namespace DRSTool.Analizer;

class Analizer
{
    private AnalizerModel model;

    private ResultModel results;


    public Analizer(AnalizerModel model)
    {
        if (model == null)
            throw new Exception("Model cannot be null");
        this.model = model;
        results = new ResultModel(model);
    }

    public Analizer(string modelFilePath)
    {
        AnalizerCompresedModel? cm = new FileHelperFactory().getFileHelper(modelFilePath).getContent<AnalizerCompresedModel>(modelFilePath);

        if (cm == null)
            throw new Exception("File model could not be read");

        model = AnalizerModel.decompress(cm);
        results = new ResultModel(model);
    }

    public void analize()
    {
        new UnstableInterfaceDetection().detect(model, results);
        //new CliqueDetector().detect(model, results);
        new CrossingDetector().detect(model, results);
        new UnhealthyInheritanceHierarchyDetector().detect(model, results);
        new ModularityViolationGroupDetector().detect(model, results);
        //new PackageCycleDetector().detect(model, results);
    }

    public void saveResults()
    {
        IFileHelper fileWriter = new FileHelperFactory().getFileHelper(ConfigValidator.getInstance().config.OutputFile.results_details);
        fileWriter.writeContent(ConfigValidator.getInstance().config.OutputFile.results_details, results.resultEntity);
    }

    public void saveAggregatedResults()
    {
        IFileHelper fileWriter = new FileHelperFactory().getFileHelper(ConfigValidator.getInstance().config.OutputFile.results_aggregate);
        fileWriter.writeContent(ConfigValidator.getInstance().config.OutputFile.results_aggregate, results.aggregate());
    }
}

internal class tempModel
{
    public string BaseName { get; set; } = "";
    public string DerivedName { get; set; } = "";
    public int cochangeTimes { get; set; }
}
