using CsvHelper.Configuration.Attributes;
using DRSTool.CommonModels.Exceptions;
using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper;
using DRSTool.FileHelper.Implementation;
using System.Dynamic;
using System.Xml.Linq;

namespace DRSTool.Extractor.DataExtraction;

class TestResultExtraction
{
    public void extract(ConstructionModel model, dynamic data)
    {
        string filePath = data["path"];

        switch ((string)data["version"])
        {
            case "aggregate-v1":
                extract_v1_csv(model, root + filePath);
                break;
            default:
                throw new UnsuportedFileTypeException($"Test results extraction option: {data["version"]} - option is not supported");
        }
            
    }
    public void extract_v1_csv(ConstructionModel model, string filePath)
    {
        var input = CsvFileHelper.getInstance().getArrayContent<TestResultModel>(filePath);

        if (input == null)
            throw new Exception("co-change commits number coupling file read error");

        foreach (var iter in input)
        {
            try
            {
                int index;

                index = getIndex(iter, model);
                
                if(index != -1) // found the file
                {
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-total-tests", iter.testNumber));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-errors", iter.errors));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-skipped", iter.skipped));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-failures", iter.failures));
                }
                else // file not found
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"The class tested by the Test case {iter.testClassName} not found. More details: \"" + iter.toString() + "\"");
                    Console.ResetColor();
                }

                index = model.getIndexForClass(iter.testClassName);
                if (index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The Test case {iter.testClassName} was not found. More details: \"" + iter.toString() + "\"");
                    Console.ResetColor();
                }
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class", true));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-total-tests", iter.testNumber));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-errors", iter.errors));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-skipped", iter.skipped));
                model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-failures", iter.failures));
            }
            catch (EntityUsedButNotDeclaredException) { }
        }
    }

    private static int getIndex(TestResultModel element, ConstructionModel model)
    {
        int index;
        if (element.candidateClassName1 is not null) {
            index = model.getIndexForClass(element.candidateClassName1);
            if (index != -1)
                return index;
        }

        if (element.candidateClassName2 is not null)
        {
            index = model.getIndexForClass(element.candidateClassName2);
            if (index != -1)
                return index;
        }

        if (element.candidateClassName3 is not null)
        {
            index = model.getIndexForClass(element.candidateClassName3);
            if (index != -1)
                return index;
        }

        return - 1;
    }

    private string root;

    private TestResultExtraction(string root) => this.root = root;

    private static TestResultExtraction _instance;

    public static TestResultExtraction createInstance(string root)
    {
        if (_instance == null)
            _instance = new TestResultExtraction(root);
        else
            throw new Exception("Instance already created. Please call getInstance() to get the instance");

        return _instance;
    }

    public static TestResultExtraction getInstance()
    {
        if (_instance is null)
            throw new Exception("Must call create instance first");
        return _instance;
    }

}


internal class TestResultModel
{

    [Name("Test Class Name")]
    public string testClassName { get; set; }

    [Name("First candidate name for the tested class")]
    public string? candidateClassName1 { get; set; } // the test class with the "Test" string removed from the test class name

    [Name("Second candidate name for the tested class")]
    public string? candidateClassName2 { get; set; } // not null if there are digits at the end of the first candidate, or the class name if no 1st candidate

    [Name("Third candidate name for the tested class")]
    public string? candidateClassName3 { get; set; } // not null if there is an _* at the end of the name of the first candidate, or the class name if no 1st candidate

    public int testNumber { get; set; } = 0;

    public int errors { get; set; } = 0;
    public int skipped { get; set; } = 0;
    public int failures { get; set; } = 0;
    public double time { get; set; } = 0;

    public String toString()
    {
        return $"{testClassName} posible testing {{\"{candidateClassName1}\", \"{candidateClassName2}\", \"{candidateClassName3}\" }} with {testNumber} tests resulting in {errors} errors, {skipped} skipped," +
            $"{failures} failures; computed in {time} seconds";
    }
}


