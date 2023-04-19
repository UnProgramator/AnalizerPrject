using DRSTool.Extractor.DataExtraction.CodeCoverage;
using DRSTool.Extractor.InternalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRSTool.Extractor.DataExtraction
{
    class CodeCoverageExtraction
    {
        public void extract(ConstructionModel model, dynamic options)
        {
            string path = options["path"];
            if (!options.ContainsKey("no_root") || options["no_root"] == false)
                path = root + path;

            callExtractionFunction(model, (string)options["version"], path);
        }

        public virtual void callExtractionFunction(ConstructionModel model, string version, string path)
        {
            switch (version)
            {
                case "jacoco":
                    JacocoExtractor.extract(model, path);
                    break;
            }
        }

        private string root;

        private CodeCoverageExtraction(string root) => this.root = root;

        private static CodeCoverageExtraction? _instance = null;

        public static CodeCoverageExtraction createInstance(string root)
        {
            if(_instance == null)
                _instance = new CodeCoverageExtraction(root);
            else
                throw new Exception("Instance already created. Please call getInstance() to get the instance");

            return _instance;
        }

        public static CodeCoverageExtraction getInstance()
        {
            if(_instance is null)
                throw new Exception("Must call create instance first");
            return _instance;
        }
    }

}
