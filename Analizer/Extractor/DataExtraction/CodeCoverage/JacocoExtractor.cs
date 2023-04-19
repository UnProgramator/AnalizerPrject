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

namespace DRSTool.Extractor.DataExtraction.CodeCoverage;

class JacocoExtractor
{
    public static void extract(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<JacocoModel>(filePath);

        if (input == null)
            throw new Exception("co-change commits number coupling file read error");

        foreach (var iter in input)
        {
            Console.WriteLine(iter.toString());
            try
            {
                int index;


                index = model.getIndexForClass(iter.package + "." + iter.className);
                if (index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The Test case {iter.package + "." + iter.className} was not found. More details: ");
                    Console.ResetColor();
                    continue;
                }

                int temp;

                temp = iter.instructMissed + iter.instructCovered;
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("total-instructions", temp));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("missed-percentage", (double)iter.instructMissed/temp));

                temp= iter.methodMissed + iter.methodCovered;
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("total-functions", temp));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("missed-functions", (double)iter.methodMissed/temp));

                temp = iter.linesMissed + iter.linesCovered;
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("total-lines", temp));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("missed-lines", (double)iter.linesMissed / temp));
            }
            catch (EntityUsedButNotDeclaredException) { }
        }
    }
}

internal class JacocoModel
{
    [Name("PACKAGE")]
    public string package { get; set; }
    [Name("CLASS")]
    public string className { get; set; }
    [Name("INSTRUCTION_MISSED")]
    public int instructMissed { get; set; }
    [Name("INSTRUCTION_COVERED")]
    public int instructCovered { get; set; }
    [Name("METHOD_MISSED")]
    public int methodMissed { get; set; }
    [Name("METHOD_COVERED")]
    public int methodCovered { get; set; }
    [Name("LINE_MISSED")]
    public int linesMissed { get; set; }
    [Name("LINE_COVERED")]
    public int linesCovered { get; set; }

    public string toString()
    {
        return $"{package}/{className} IM {instructMissed}";
    }
}

    //BRANCH_MISSED   BRANCH_COVERED   COMPLEXITY_MISSED   COMPLEXITY_COVERED    
