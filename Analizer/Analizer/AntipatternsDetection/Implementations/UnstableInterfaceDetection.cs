using DRSTool.Analizer.AntipatternsDetection.Config;
using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

class UnstableInterfaceDetection : IAntipatternDetector
{
    UifTresholds tresholds;

    public const string AntipatternName = "UIF";

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
            (int Fset_S, int Fset_H_intersect_S) = computeIndices(dataModel, i); // compute the cardinality of Fset_S and the intersection of Fset_H_intersect_S and Fset_S

            if (Fset_S > tresholds.StructImpact 
                && Fset_H_intersect_S >= tresholds.HistoryImpact)
            {

                var value = new Dictionary<string, object> {
                    { "denpendents-count", Fset_S},
                    { "dependents-cochanges", Fset_H_intersect_S}
                };

                results.add(i, AntipatternName, value);
            }
        }
    }

    private (int,int) computeIndices(AnalizerModel dataModel, int sourceEntity)
    {
        int Fset_S = 0, Fset_H_int_S = 0; // compute the cardinality of Fset_S and the intersection of Fset_H_intersect_S and Fset_S

        for (int j = 0; j < dataModel.Entities.Length; j++)
        {
            if (dataModel.SRelations[j, sourceEntity] is null) continue; // no structural relation present

            Fset_S++;

            if (dataModel.HRelations[j, sourceEntity] is null) continue; // no evolution coupling present

            var rel = dataModel.HRelations[j, sourceEntity].Properties;

            if (rel.ContainsKey("cochanges") && rel["cochanges"] > tresholds.cochange)
            {
                Fset_H_int_S ++;
            }
        }

        return (Fset_S, Fset_H_int_S);
    }

    /*
        - StructImpacth[thr] -> treshold; if more dependents than treshold, than candidate
	    - HistoryImpact[thr] -> the number of dependents whith whom the file cochanged
	    - cochange[thr]      -> the treshold of cochange frequency (number of minimum cochanges with another file in order to be considered frequent cochange => evolutionay coupled)
     Formula:
	    for UIF(f) -> (Fset_S > StructImpact[thr]) && (Fset_S intersect with Fset_H > HisotryImpact[thr])
        where:
		    Fset_S: set of file in relation with the current file
		    Fset_H: files evolutionay coupled, where cochanges(fk, f) > cochange[thr])

      [after Ran Mo at al., "Architecture Anti-Patterns: Automatically Detectable Violations of Design Principles", 3.2]
     */
}


