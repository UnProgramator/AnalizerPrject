using Analizer.FileHelper;
using Analizer.FileHelper.Implementation;

namespace Analizer.Entractor.Config
{
    /// <summary>
    /// Read the configuration file and validate the data format inside as far as poss+ible. Doesn't check semantics and the existence fo the files
    /// </summary>
    class ConfigValidator
    {
        private ConfigModel _config;
        public ConfigModel config { get => _config; }

        /// <summary>
        /// Read the config file and validates the content. Completes with default values when necessarily.
        /// </summary>
        /// <param name="filePath">The path to the config file.</param>
        public ConfigValidator(string filePath)
        {
            JsonFileHelper jfh = JsonFileHelper.getInstance();
            var temp_config = jfh.getContent<ConfigModel>(filePath);
            _config = validateConfigAndCompleteDefault(temp_config);
        }

        /// <summary>
        /// Returns a Config object, with the input file at the default location and name [./AnalizerConfig.json]
        /// </summary>
        public static ConfigValidator getCofigForDefaultLocation()
        {
            return new ConfigValidator("AnalizerConfig.json");
        }

        /// <summary>
        /// Validates the config file data and completes the default values where and when necessarily
        /// </summary>
        /// <param name="_config">The configuration data read from the config file</param>
        /// <returns>the values, as ConfigModel (guaranteed not null)</returns>
        /// <exception cref="InvalidConfigException">Thrown when file not read or non-caught read error</exception>
        /// <exception cref="InvalidConfigOutputException">When "Output File" input data validation not succesfull</exception>
        private ConfigModel validateConfigAndCompleteDefault(ConfigModel? _config)
        {
            if (_config == null)
                throw new InvalidConfigException("Returned reference was null. File don't exist or error during reading");

            validateOutputFile(_config);

            //validateInput(_config);

            return _config;
        }

        /// <summary>
        /// validate output file info and write default values if necessarily
        /// </summary>
        /// <param name="_config">The content read from the config file</param>
        /// <exception cref="InvalidConfigOutputException">When "Output File" input data validation not succesfull</exception>
        private void validateOutputFile(ConfigModel _config)
        {
            string? fileFormat = null;

            //verify out file; if none specified, then default
            if (!_config.OutputFile.ContainsKey("file"))
                _config.OutputFile.Add("file", "./results");
            else
                fileFormat = FileHelper.FileUtilities.getFileExtension(_config.OutputFile["file"]);

            //verify out file format; if none specified, then take from string
            if (!_config.OutputFile.ContainsKey("format")) {
                if (fileFormat == null)
                {
                    fileFormat = ".json";
                    _config.OutputFile["file"] += fileFormat;
                }
                _config.OutputFile["format"] = fileFormat;
            }
            
            else {
                //if file format is specified and but not added to the file, then it is added
                if (fileFormat == null)
                    _config.OutputFile["file"] += _config.OutputFile["format"];
                //verify if specified file format and the one from the path are the same
                else if (!_config.OutputFile["format"].Equals(fileFormat))
                    throw new InvalidConfigOutputException("File format from param and from file path are different");
            }

            //verify if format is accepted by the current implementation
            if (!FileHelper.FileUtilities.acceptedFileExtensions.Contains(_config.OutputFile["format"]))
                throw new UnsuportedFileTypeException(_config.OutputFile["format"]);
        }

        /// <summary>
        /// Validate the input data. Complete the default file name
        /// </summary>
        /// <param name="_config"></param>
        private void validateInput(ConfigModel _config)
        {
            if (_config.DefaultInput)
            {
                
            }
            //string ...
        }

        private void validateDefaultInput(ConfigModel _config)
        {
            if (!_config.Input.ContainsKey("project_name"))
                throw new InvalidConfigDefaultInputException("Defualt marked as true, but no project name was given");

            //????
            if (_config.Input["project_name"] is not string)
                throw new InvalidConfigDefaultInputException("Project name is not a string");

            string ProjectName = (string)_config.Input["project_name"];

            if(ProjectName.Length == 0)
                throw new InvalidConfigDefaultInputException("Project name cannot be an empty string");
        }
    }
}
