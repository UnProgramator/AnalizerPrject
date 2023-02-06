namespace DRSTool.FileHelper;

class FileUtilities
{
    public static string? getFileExtension(string filename)
    {
        string? extension = null;

        extension = Path.GetExtension(filename);

        if (extension.Equals(""))
            extension = null;

        return extension;
    }

    public static readonly string[] acceptedFileExtensions = new string[] { ".csv", ".json" };
}
