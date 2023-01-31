using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace Analizer.FileHelper.Implementation
{
    class CsvFileHelper : FileHelperInterface
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

        public Dictionary<string, object>[]? getContentAsDictArray(string filename)
        {
            Dictionary<string, object>[]? content;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
            var reader = new StreamReader(filename);
            var csv = new CsvReader(reader, config);
            content = csv.GetRecord<Dictionary<string, object>[]?>();

            return content;
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
        public void writeContent<T>(string filename, object content)
        {
            if (content is not IEnumerable<T>)
                throw new Exception("parameter \"content\" is of type " + content.GetType().Name + ", but expected variable of type IEnumerable<" + typeof(T).Name + ">");

            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords((IEnumerable<T>)content);
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
}
