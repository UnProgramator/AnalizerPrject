using Analizer.FileHelper.Implementation;

namespace Analizer.FileHelper
{
    internal class FileHelperFactory
    {
        public CsvFileHelper getCsvHelper()
        {
            return CsvFileHelper.getInstance();
        }

        public JsonFileHelper getJsonHelper()
        {
            return JsonFileHelper.getInstance();
        }

        public FileHelperInterface getXmlHelper()
        {
            throw new NotImplementedException();
        }

        public FileHelperInterface getFileHelper(string fileExtension)
        {
            switch (fileExtension)
            {
                case "csv":
                    return getCsvHelper();
                case ".json":
                    return getJsonHelper();
            }

            throw new UnsuportedFileTypeException(fileExtension);
        }
    }
}
