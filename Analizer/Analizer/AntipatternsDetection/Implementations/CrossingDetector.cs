using DRSTool.Analizer.AntipatternsDetection.Config;
using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

internal class CrossingDetector : IAntipatternDetector
{
    public CrossingDetector():this(new CrsTresholds()) {}

    public CrossingDetector(CrsTresholds tresholds)
    {
        this.tresholds = tresholds;
    }

    public void detect(AnalizerModel dataModel, ResultModel results)
    {
        for (int crt = 0; crt < dataModel.Entities.Length; crt++)
        {
            int fanIn = 0;
            int fanOut = 0;
            for (int i = 0; i < dataModel.Entities.Length; i++)
            {
                if (i == crt) continue; // pass the main diagonal of the matrix

                // fan in
                if (dataModel.SRelations[crt, i] != null) // structural dependency with the class: curent class depends on i
                {
                    if (dataModel.HRelations[crt, i] != null && dataModel.HRelations[crt, i].Properties["cochanges"] > tresholds.cochange)
                        fanIn++;
                }

                //fan out
                if (dataModel.SRelations[i, crt] != null) // structural dependency with the class: i depends on curent class
                {
                    if (dataModel.HRelations[i, crt] != null && dataModel.HRelations[i, crt].Properties["cochanges"] > tresholds.cochange)
                        fanOut++;
                }
            }

            if(fanIn > tresholds.crossing && fanOut > tresholds.crossing)
            {
                var entity = dataModel.Entities[crt].Name;

                var value = new Dictionary<string, object> {
                    { "fan-in", fanIn},
                    { "fan-out", fanOut}
                };

                results.add(crt, "crossing", value);
            }
        }

    }

    private CrsTresholds tresholds;

    /*
      "If a file is changed frequently with its
        dependents and the files that it depends on, 
        then we condsider these files to follow a Crossing anti-pattern." 
      [Ran Mo at al., "Architecture Anti-Patterns: Automatically Detectable Violations of Design Principles", 3.2]
     */
}
