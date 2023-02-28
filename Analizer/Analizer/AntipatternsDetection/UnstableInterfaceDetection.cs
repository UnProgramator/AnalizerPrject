using DRSTool.Analizer.Models;
using DRSTool.CommonModels;
using System.Reflection;

namespace DRSTool.Analizer.AntipatternsDetection;

class UnstableInterfaceDetection : IAntipatternDetector
{
    public void detect(AnalizerModel dataModel, ResultModel results)
    {
        for (int i = 0; i < dataModel.Entities.Length; i++)
        {
            int dependents = 0, cochanges = 0;
            for (int j = 0; j < dataModel.Entities.Length; j++)
            {
                if (dataModel.Relations[j, i] != null)
                {
                    //some test may be needed in the future
                    dependents++;

                    var rel = dataModel.Relations[j, i].Properties;

                    if (rel.ContainsKey("cochanges"))
                    {
                        cochanges += rel["cochanges"];

                    }
                }
            }

            if (dependents > 4 && ((float)cochanges) / dependents >= 1)
            {
                var entity = dataModel.Entities[i].Name;

                var value = new Dictionary<string, object> {
                    { "antipattern-type", "unstable-interface" },
                    { "dendents-count", dependents},
                    { "average co-changes", ((float)cochanges) / dependents},
                    { "total co-changes", cochanges}};

                results.add(entity, value);
            }
        }
    }
}
