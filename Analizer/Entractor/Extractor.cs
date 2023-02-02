using Analizer.Entractor.Config;
using Analizer.Entractor.DataExtraction;
using Analizer.Entractor.InternalModels;
using Analizer.Extractor.DataExtraction;
using Analizer.FileHelper;

namespace Analizer.Entractor
{
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
            FileHelperInterface writer = new FileHelperFactory().getJsonHelper();
            writer.writeContent(config.OutputFile["file"], model.compress());
        }

        private ConfigModel config;

        public ConstructionModel model { get; private set; }
    }
}
