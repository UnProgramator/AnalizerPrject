namespace Analizer.Entractor.Config
{
    internal class ConfigModel
    {
        public bool DefaultInput { get; set; } = true;
        public Dictionary<string, string> OutputFile { get; set; } = new Dictionary<string,string>();
        public Dictionary<string, object> Input { get; set; } = new Dictionary<string, object>();
        public string root { get; set; } = "";
    }
}
