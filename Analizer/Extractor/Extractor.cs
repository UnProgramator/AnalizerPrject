using DRSTool.Extractor.Config;
using DRSTool.Extractor.DataExtraction;
using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper;

namespace DRSTool.Extractor;

class Extractor
{
    public Extractor() {

        var confVal = new ConfigValidator("config/AnalizerConfig - Argouml.json");
        config = confVal.config;

        if (!config.Input.ContainsKey("struct"))
            throw new Exception("No input file for \"structure\" was given");

        model = EntitiesExtractor.CreateConstructionModel(config.root + (string)config.Input["struct"]);

        gde = new GenericDataExtraction(config.root, model);

        CodeCoverageExtraction.createInstance(config.root);
        TestResultExtraction.createInstance(config.root);

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
            if (inputType.Equals("struct")) // ignore the struct propert. Already extracted in constructor
                continue;

            string path;

            switch (inputType)
            {
                case "hierarchy":
                    path = FileUtilities.getFileFromDefaultConfig(config.root, config.Input["hierarchy"]);
                    HierarchyExtractor.extract(model, path);
                    break;
                case "jafax_layout":
                    path = FileUtilities.getFileFromDefaultConfig(config.root, config.Input["jafax_layout"]);
                    JafaxLayoutExtraction.extract(model, path, "cassandra/");
                    break;
                case "cochange_number":
                    path = FileUtilities.getFileFromDefaultConfig(config.root, config.Input["cochange_number"]);
                    CochangeExtractor.extract_number(model, path);
                    break;
                case "cochange_percent":
                    path = FileUtilities.getFileFromDefaultConfig(config.root, config.Input["cochange_percent"]);
                    CochangeExtractor.extract_percentage(model, path);
                    break;
                case "test_results":
                    TestResultExtraction.getInstance().extract(model, config.Input["test_results"]);
                    break;
                case "code_coverage":
                    CodeCoverageExtraction.getInstance().extract(model, config.Input["code_coverage"]);
                    break;
                default:
                    gde.extract(inputType, input[inputType]);
                    break;
            }
        }
        
    }

    public ConstructionModel model { get; private set; }

    private ConfigModel config;

    private GenericDataExtraction gde;
}
