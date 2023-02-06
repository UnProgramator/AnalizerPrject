namespace DRSTool.FileHelper;

class UnsuportedFileTypeException: Exception
{
    public UnsuportedFileTypeException(string fileType) : base("Files of type \"" + fileType + "\" not accepted at the tyme") { }
    public UnsuportedFileTypeException() : base() { }
}
