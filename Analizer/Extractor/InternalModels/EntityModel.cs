using CsvHelper.Configuration.Attributes;

namespace DRSTool.Extractor.InternalModels;

class EntityModel
{
    [Name("PackageName(only applies to certain filetypes)")]
    public string? PackageName { get; set; }
    public string Filename { get; set; } = "";
    public string Extension { get; set; } = "";
    public string RawPath { get; set; } = "";
    public string? Module { get; set; }
    public string? SubModule { get; set; }
    public long LinesOfCode { get; set; } = 0;
    [Name("#Changes")]
    public long Changes { get; set; } = 0;

    public string Component { get; set; } = "";
    public override string ToString()
    {
        return PackageName + " -> " + Filename + " = " + Extension;
    }
}
