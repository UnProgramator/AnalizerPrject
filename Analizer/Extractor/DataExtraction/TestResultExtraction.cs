using CsvHelper.Configuration.Attributes;
using DRSTool.CommonModels.Exceptions;
using DRSTool.Extractor.InternalModels;
using DRSTool.FileHelper;
using DRSTool.FileHelper.Implementation;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security;
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

        int unmatchedClasses = 0, unmatchedTestcases = 0, total = 0;

        foreach (var iter in input)
        {
            total++;
            try
            {
                int index;

                index = getIndex(iter, model);
                
                if(index != -1) // found the file
                {
                    Action<int, string, int> addTesting = (i, k, val) => { 
                                    if (model.Entities[i].Properties != null && model.Entities[i].Properties.ContainsKey(k)) 
                                        model.Entities[i].Properties[k] += val; 
                                    else
                                        model.addEntityProperty(i, new KeyValuePair<string, dynamic>(k, val));
                    };

                    addTesting(index, "test-class-total-tests", iter.testNumber);
                    addTesting(index, "test-class-errors", iter.errors);
                    addTesting(index, "test-class-skipped", iter.skipped);
                    addTesting(index, "test-class-failures", iter.failures);
                    addTesting(index, "test-time", iter.failures);
                }
                else // file not found
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Surefire: The class tested by the Test case {iter.testClassName} not found. More details: \"" + iter.toString() + "\"");
                    Console.ResetColor();
                    unmatchedClasses++;
                }

                index = model.getIndexForClass(iter.testClassName);
                if (index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Surefire: The Test case {iter.testClassName} was not found. More details: \"" + iter.toString() + "\"");
                    Console.ResetColor();
                    unmatchedTestcases++;
                }
                else
                {
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class", true));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-total-tests", iter.testNumber));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-errors", iter.errors));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-skipped", iter.skipped));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-class-failures", iter.failures));
                    model.addEntityProperty(index, new KeyValuePair<string, dynamic>("test-time", iter.failures));
                }

            }
            catch (EntityUsedButNotDeclaredException) { }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Matched {total - unmatchedClasses} class from a total of {total}. {unmatchedClasses} files skyped");
        Console.WriteLine($"Matched {total - unmatchedTestcases} test cases from a total of {total}. {unmatchedTestcases} files skyped");
        Console.ResetColor();
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

    private static TestResultExtraction? _instance;

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


