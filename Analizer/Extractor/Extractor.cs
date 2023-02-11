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

        if (!config.Input.ContainsKey("struct"))
            throw new Exception("No input file for \"structure\" was given");

        model = EntitiesExtractor.CreateConstructionModel(config.root + (string)config.Input["struct"]);

        gde = new GenericDataExtraction(config.root, model);

        parseInput();
    }

    public void save()
    {
        IFileHelper writer = new FileHelperFactory().getJsonHelper();
        writer.writeContent(config.OutputFile["file"], model.compress());
    }

    private void parseInput()
    {
        

        var input = config.Input;

        foreach (var inputType in input.Keys)
        {
            if (inputType.Equals("struct")) // ignore the struct propert. Already used
                continue;
            if (input[inputType] is Dictionary<string, object>)
                gde.extract(inputType, (Dictionary<string, object>)input[inputType]);
            else if (input[inputType] is not string)
                throw new Exception($"Invalid field {inputType}, having type {input[inputType].GetType().Name}");
            switch (inputType)
            {
                case "hierarchy":
                    HierarchyExtractor.extract(model, config.root + "/" + config.Input["hierarchy"]);
                    break;
                case "cochange_number":
                    CochangeExtractor.extract_number(model, config.root + "/" + config.Input["cochange_number"]);
                    break;
                case "cochange_percent":
                    CochangeExtractor.extract_percentage(model, config.root + "/" + config.Input["cochange_percent"]);
                    break;
                default:
                    throw new Exception($"Invalid field {inputType}, having type {input[inputType].GetType().Name}, but no such type is implemented");
            }
        }
        
    }

    public ConstructionModel model { get; private set; }

    private ConfigModel config;

    private GenericDataExtraction gde;
}
