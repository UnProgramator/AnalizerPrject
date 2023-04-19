using DRSTool.FileHelper.Implementation;

namespace DRSTool.FileHelper;

internal class FileHelperFactory
{
    public IFileHelper getCsvHelper()
    {
        return CsvFileHelper.getInstance();
    }

    public IFileHelper getJsonHelper()
    {
        return JsonFileHelper.getInstance();
    }

    public IFileHelper getXmlHelper()
    {
        throw new NotImplementedException();
    }

    public IFileHelper getFileHelper(string file)
    {
        string? fileExtension = FileUtilities.getFileExtension(file);

        if(fileExtension == null)
            throw new UnsuportedFileTypeException("type not specified");

        switch (fileExtension)
        {
            case ".csv":
                return getCsvHelper();
            case ".json":
                return getJsonHelper();
        }

        throw new UnsuportedFileTypeException(fileExtension);
    }
}
