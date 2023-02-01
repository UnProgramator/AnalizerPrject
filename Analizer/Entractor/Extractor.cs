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
        }

        public void save()
        {
            FileHelperInterface writer = new FileHelperFactory().getJsonHelper();
            writer.writeContent(config.OutputFile["file"], model);
        }

        private ConfigModel config;

        public ConstructionModel model { get; private set; }
    }
}
