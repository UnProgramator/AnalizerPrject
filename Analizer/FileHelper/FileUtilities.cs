using Newtonsoft.Json.Linq;
using System;

namespace DRSTool.FileHelper;

class FileUtilities
{
    public static string? getFileExtension(string filename)
    {
        string? extension = null;

        extension = Path.GetExtension(filename);

        if (extension.Equals(""))
            extension = null;

        return extension;
    }

    public static string getFileFromDefaultConfig(string root, dynamic config)
    {

        if (config is string)
            return root + config;
        else
        {
            try
            {
                if (config.ContainsKey("ignore-root"))
                {
                    bool value= (bool)config["ignore-root"];
                    if (value)
                        return (string)config["file"];
                }
                else
                    return root + (string)config["file"];
            }
            catch(InvalidCastException)
            {
                throw new Exception($"config was expected string or dictionary-access compatible type, but {config.GetType()} was given");
            }
            catch(KeyNotFoundException)
            {
                throw new Exception($"Dicitonary config had no field \"file\"");
            }
        }

        throw new Exception("config invalid");
    }

    public static readonly string[] acceptedFileExtensions = new string[] { ".csv", ".json" };
}
