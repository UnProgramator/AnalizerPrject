using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRSTool.Analizer.Models
{
    class ResultModel
    {
        public Dictionary<string, List<Dictionary<string, object>>> results { private set; get; }


        public ResultModel()
        {
            results = new Dictionary<string, List<Dictionary<string, object>>>();
        }

        public void add(string entityKey, Dictionary<string, object> value)
        {
            if (results.ContainsKey(entityKey))
                results[entityKey].Add(value);
            else
                results.Add(entityKey, new List<Dictionary<string, object>> { value });
        }
    }
}
