using CsvHelper.Configuration.Attributes;

namespace Analizer.Entractor.InternalModels
{
    class StructMinerModel
    {
        [Name("PackageName(only applies to certain filetypes)")]
        public string PackageName { get; set; } = "";
        public string Filename { get; set; } = "";
        public string Extension { get; set; } = "";

        public override string ToString()
        {
            return PackageName + " -> " + Filename + " = " + Extension;
        }
    }
}
