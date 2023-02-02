using Analizer.Entractor.InternalModels;
using Analizer.FileHelper.Implementation;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizer.Entractor.DataExtraction
{
    internal class CochangeExtractor
    {
        public static void extract_number(ConstructionModel model, string filePath)
        {
            var input = CsvFileHelper.getInstance().getArrayContent<CochangeModel>(filePath);

            if (input == null)
                throw new Exception("co-change commits number coupling file read error");

            foreach (var iter in input)
            {
                try
                {
                    model.addRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("cochanges", iter.relations));
                }
                catch (EntityUsedButNotDeclaredException) { }
            }
        }

        public static void extract_percentage(ConstructionModel model, string filePath)
        {
            var input = CsvFileHelper.getInstance().getArrayContent<CochangePercentageModel>(filePath);

            if (input == null)
                throw new Exception("co-change commits percentage file read error");

            foreach (var iter in input)
            {
                try { 
                model.addRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("cochanges_100", iter.relations));
                }
                catch (EntityUsedButNotDeclaredException) { }
            }
        }
    }

    internal class CochangeModel
    {
        //source,target,Hierarchy-Specific Relations

        [Name("source")]
        public string Source { get; set; } = "";

        [Name("target")]
        public string Target { get; set; } = "";

        [Name("Temporal Coupling (commits)")]
        public int relations { get; set; }
    }

    internal class CochangePercentageModel
    {
        //source,target,Hierarchy-Specific Relations

        [Name("source")]
        public string Source { get; set; } = "";

        [Name("target")]
        public string Target { get; set; } = "";

        [Name("Temporal Coupling Percentage (commits)")]
        public int relations { get; set; }
    }
}
