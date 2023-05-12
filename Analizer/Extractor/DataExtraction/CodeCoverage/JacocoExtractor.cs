using CsvHelper.Configuration.Attributes;
using DRSTool.CommonModels.Exceptions;
using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DRSTool.Extractor.DataExtraction.CodeCoverage;

class JacocoExtractor
{
    public static void extract(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<JacocoModel>(filePath);

        if (input == null)
            throw new Exception("co-change commits number coupling file read error");

        int unmatched = 0, total = 0;

        foreach (var iter in input)
        {
            total++;
            //Console.WriteLine(iter.toString());
            try
            {
                int index;


                index = model.getIndexForClass(iter.package + "." + iter.className);
                if (index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Jacoco data extraction: The class {iter.package + "." + iter.className} was not found. More details: ");
                    Console.ResetColor();
                    unmatched++;
                    continue;
                }

                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("instructions-missed", iter.instructMissed));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("instructions-covered", iter.instructCovered));

                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("functions-missed", iter.methodMissed));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("functions-covered", iter.methodCovered));

                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("branch-missed", iter.branchMissed));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("branch-covered", iter.branchCovered));
                
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("complexity-missed", iter.complexityMissed));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("complexity-covered", iter.complexityCovered));
            }
            catch (EntityUsedButNotDeclaredException) { }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Matched {total - unmatched} files from {total}. {unmatched} files skyped");
        Console.ResetColor();

    }
}

internal class JacocoModel
{
    [Name("PACKAGE")]
    public string package { get; set; } = "";
    [Name("CLASS")]
    public string className { get; set; } = "";
    [Name("INSTRUCTION_MISSED")]
    public int instructMissed { get; set; }
    [Name("INSTRUCTION_COVERED")]
    public int instructCovered { get; set; }
    [Name("METHOD_MISSED")]
    public int methodMissed { get; set; }
    [Name("METHOD_COVERED")]
    public int methodCovered { get; set; }
    [Name("BRANCH_COVERED")]
    public int branchMissed { get; set; }
    [Name("BRANCH_MISSED")]
    public int branchCovered { get; set; }
    [Name("COMPLEXITY_MISSED")]
    public int complexityMissed { get; set; }
    [Name("COMPLEXITY_COVERED")]
    public int complexityCovered { get; set; }


    public string toString()
    {
        return $"{package}/{className} IM {instructMissed}";
    }
}

//LINES_COVERED    LINES_MISSED 
