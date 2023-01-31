using Analizer.Entractor.Config;
using Analizer.Entractor.InternalModels;
using Analizer.Extractor.DataExtraction;

namespace Analizer.Entractor
{
    class Extractor
    {
        public Extractor() {

            var confVal = ConfigValidator.getCofigForDefaultLocation();
            config = confVal.config;
            model = EntitiesExtractor.CreateConstructionModel(config);
        }

        private ConfigModel config;

        public ConstructionModel model { get; private set; }
    }
}
