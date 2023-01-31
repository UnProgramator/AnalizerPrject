namespace Analizer.FileHelper
{
    interface FileHelperInterface
    {
        IEnumerable<T>? getArrayContent<T>(string filename);
        T? getContent<T>(string filename);
        public Dictionary<string, object>[]? getContentAsDictArray(string filename);
        public Dictionary<string, object>? getContentAsDict(string filename);
        void writeContent<T>(string filename, object content);
    }
}
