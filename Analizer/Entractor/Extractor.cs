using Analizer.CommonModels;
using Analizer.Entractor.Config;
using Analizer.Extractor.DataExtraction;

namespace Analizer.Entractor
{
    class Extractor
    {
        public Extractor() {

            var confVal = ConfigValidator.getCofigForDefaultLocation();
            config = confVal.config;
            analizerModel = EntitiesExtractor.initAnalizerModel(config);
        }

        ConfigModel config;

        private AnalizerModel analizerModel;
    }
}
