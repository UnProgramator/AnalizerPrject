using Analizer.CommonModels;
using Analizer.Entractor.Config;
using Analizer.Entractor.InternalModels;
using Analizer.FileHelper.Implementation;

namespace Analizer.Extractor.DataExtraction
{
    class EntitiesExtractor
    {
        private static ConstructionModel initModel(int count)
        {
            return new ConstructionModel(count, new string[] { "" });
        }

        public static ConstructionModel CreateConstructionModel(ConfigModel config)
        {
            ConstructionModel analizerModel;

            string structFile = config.root + (string)config.Input["struct"];

            var input = CsvFileHelper.getInstance().getArrayContent<StructMinerModel>(structFile);

            if (input == null)
                throw new Exception("Error during reading struct file");

            input = input.Where(m => m.Extension.Equals("java")).OrderBy(m => m.PackageName).ThenBy(m => m.Filename);

            //fore some reason, count remove the elements from the IEnumerable implementation
            //because of that, I converted it to an array

            input = input.ToArray();

            Console.WriteLine(input.GetType());

            analizerModel = initModel(input.Count());

            foreach (var file in input)
            {
                analizerModel.addEntity(file.Filename, new Dictionary<string, dynamic>() { { "package", file.PackageName } });
            }

            return analizerModel;
        }
    }
}
