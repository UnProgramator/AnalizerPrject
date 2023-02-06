using DRSTool.Extractor.Config;
using DRSTool.Extractor.DataExtraction;
using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper;

namespace DRSTool.Extractor;

class Extractor
{
    public Extractor() {

        var confVal = ConfigValidator.getCofigForDefaultLocation();
        config = confVal.config;
        model = EntitiesExtractor.CreateConstructionModel(config.root + (string)config.Input["struct"]);
        HierarchyExtractor.extract(model, config.root + "/" + config.Input["hierarchy"]);
        CochangeExtractor.extract_number(model, config.root + "/" + config.Input["cochange_number"]);
        CochangeExtractor.extract_percentage(model, config.root + "/" + config.Input["cochange_percent"]);
    }

    public void save()
    {
        IFileHelper writer = new FileHelperFactory().getJsonHelper();
        writer.writeContent(config.OutputFile["file"], model.compress());
    }

    private ConfigModel config;

    public ConstructionModel model { get; private set; }
}
