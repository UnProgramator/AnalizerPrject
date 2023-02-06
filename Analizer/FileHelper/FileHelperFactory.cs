using DRSTool.FileHelper.Implementation;

namespace DRSTool.FileHelper;

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

    public IFileHelper getXmlHelper()
    {
        throw new NotImplementedException();
    }

    public IFileHelper getFileHelper(string fileExtension)
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
