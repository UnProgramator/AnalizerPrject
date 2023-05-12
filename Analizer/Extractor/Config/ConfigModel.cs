namespace DRSTool.Extractor.Config;

class ConfigModel
{
    public bool DefaultInput { get; set; } = true;
    public OutputModel OutputFile { get; set; } = new OutputModel();
    public Dictionary<string, object> Input { get; set; } = new Dictionary<string, object>();
    public string root { get; set; } = "";
    public string? projectName { get; set; }

    public class OutputModel
    {
        public string model { get; set; } = "model.json";
        public string results_details { get; set; } = "results.json";
        public string results_aggregate { get; set; } = "results.csv";
    }
}

