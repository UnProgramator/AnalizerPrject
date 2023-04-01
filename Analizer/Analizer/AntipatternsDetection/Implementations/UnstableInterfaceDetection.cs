using DRSTool.Analizer.AntipatternsDetection.Config;
using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

class UnstableInterfaceDetection : IAntipatternDetector
{
    UifTresholds tresholds;

    public UnstableInterfaceDetection()
    {
        tresholds = new UifTresholds();
    }

    public UnstableInterfaceDetection(UifTresholds tresholds)
    {
        this.tresholds = tresholds;
    }

    public void detect(AnalizerModel dataModel, ResultModel results)
    {
        for (int i = 0; i < dataModel.Entities.Length; i++)
        {
            (int Fset_S, int Fset_H) = computeIndices(dataModel, i);

            if (Fset_S > tresholds.StructImpact 
                && Fset_S >= tresholds.HistoryImpact)
            {
                var entity = dataModel.Entities[i].Name;

                var value = new Dictionary<string, object> {
                    { "antipattern-type", "unstable-interface" },
                    { "denpendents-count", Fset_S},
                    { "average-cochanges", (float)Fset_H / Fset_S},
                    { "total-cochanges", Fset_H}};

                results.add(entity, value);
            }
        }
    }

    private (int,int) computeIndices(AnalizerModel dataModel, int colIdx)
    {
        int Fset_S = 0, Fset_H = 0;

        for (int j = 0; j < dataModel.Entities.Length; j++)
        {
            if (dataModel.SRelations[j, colIdx] != null)
            {
                //some test may be needed in the future
                Fset_S++;

                var rel = dataModel.SRelations[j, colIdx].Properties;

                if (rel.ContainsKey("cochanges"))
                {
                    Fset_H += rel["cochanges"];
                }
            }
        }

        return (Fset_S, Fset_H);
    }
}


