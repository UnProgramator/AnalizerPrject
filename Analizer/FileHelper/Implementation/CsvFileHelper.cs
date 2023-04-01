using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Collections;

namespace DRSTool.FileHelper.Implementation;

class CsvFileHelper : IFileHelper
{
    public IEnumerable<T>? getArrayContent<T>(string filename)
    {
        IEnumerable<T>? content = null;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        var reader = new StreamReader(filename);
        var csv = new CsvReader(reader, config);
        content = csv.GetRecords<T>();

        return content;
    }

    public T? getContent<T>(string filename)
    {
        T? content = default;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        var reader = new StreamReader(filename);
        var csv = new CsvReader(reader, config);
        content = csv.GetRecord<T>();

        return content;
    }

    public IEnumerable<Dictionary<string, object>>? getContentAsDictArray(string filename)
    {
        //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //{
        //    HasHeaderRecord = true,
        //};
        //var reader = new StreamReader(filename);
        //var csv = new CsvReader(reader, config);
        //var content = csv.GetRecords<Dictionary<string, object>>

        return getArrayContent<Dictionary<string, object>>(filename);
    }

    public Dictionary<string, object>? getContentAsDict(string filename)
    {
        Dictionary<string, object>? content;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        var reader = new StreamReader(filename);
        var csv = new CsvReader(reader, config);
        content = csv.GetRecord<Dictionary<string, object>?>();

        return content;
    }
    public void writeContent(string filename, object content)
    {
        using (var writer = new StreamWriter(filename))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            if (content is not IEnumerable)
                csv.WriteRecords((IEnumerable)content);
            else
                csv.WriteRecord(content);
        }
    }

    private CsvFileHelper() { }

    private static CsvFileHelper? __instance;
    private static object __barier = new object();

    public static CsvFileHelper getInstance()
    {
        if (__instance == null)
        {
            lock (__barier)
            {
                if (__instance == null)
                {
                    __instance = new CsvFileHelper();
                }
            }
        }

        return __instance;
    }
}
