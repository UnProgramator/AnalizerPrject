using Newtonsoft.Json;

namespace Analizer.FileHelper.Implementation
{
    class JsonFileHelper : FileHelperInterface
    {
        public IEnumerable<T>? getArrayContent<T>(string filename)
        {
            IEnumerable<T>? content = null;
            var _file = File.OpenText(filename);
            JsonSerializer serializer = new JsonSerializer();
            content = (IEnumerable<T>?)serializer.Deserialize(_file, typeof(IEnumerable<T>));

            return content;
        }

        public T? getContent<T>(string filename)
        {
            T? content = default;
            var _file = File.OpenText(filename);
            JsonSerializer serializer = new JsonSerializer();
            content = (T?)serializer.Deserialize(_file, typeof(T));

            return content;
        }

        public Dictionary<string, object>[]? getContentAsDictArray(string filename)
        {
            Dictionary<string, object>[]? content;
            var _file = File.OpenText(filename);
            JsonSerializer serializer = new JsonSerializer();
            content = (Dictionary<string, object>[]?)serializer.Deserialize(_file, typeof(Dictionary<string, object>[]));

            return content;
        }

        public Dictionary<string, object>? getContentAsDict(string filename)
        {
            Dictionary<string, object>? content;
            var _file = File.OpenText(filename);
            JsonSerializer serializer = new JsonSerializer();
            content = (Dictionary<string, object>?)serializer.Deserialize(_file, typeof(Dictionary<string, object>));

            return content;
        }

        public void writeContent(string filename, object content)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, content);
            }
        }

        private JsonFileHelper() { }

        private static JsonFileHelper? __instance;
        private static object __barier = new object();

        public static JsonFileHelper getInstance()
        {
            if (__instance == null)
            {
                lock (__barier)
                {
                    if (__instance == null)
                    {
                        __instance = new JsonFileHelper();
                    }
                }
            }

            return __instance;
        }
    }
}
