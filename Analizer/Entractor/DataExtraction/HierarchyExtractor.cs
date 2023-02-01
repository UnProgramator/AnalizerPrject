﻿using Analizer.CommonModels;
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
    class HierarchyExtractor
    {
        public static void extract(ConstructionModel model, string filePath)
        {
            var input = CsvFileHelper.getInstance().getArrayContent<HierarchyModel>(filePath);

            if (input == null)
                throw new Exception("hyerarchy file read error");

            foreach(var iter in input)
            {
                model.addRelation(iter.Source, iter.Target, new KeyValuePair<string, dynamic>("inheritance", true));
            }
        }
    }

    internal class HierarchyModel
    {
        //source,target,Hierarchy-Specific Relations

        [Name("source")]
        public string Source { get; set; } = "";

        [Name("target")]
        public string Target { get; set; } = "";

        [Name("Hierarchy-Specific Relations")]
        public int relations { get; set; }
    }
}
