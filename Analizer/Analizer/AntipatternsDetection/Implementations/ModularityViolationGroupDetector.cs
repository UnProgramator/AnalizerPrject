using DRSTool.Analizer.AntipatternsDetection.Config;
using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

internal class ModularityViolationGroupDetector : IAntipatternDetector
{
    public const string AntipatternName = "MVC";

    public ModularityViolationGroupDetector(MvgTresholds treshold) => this.treshold = treshold;

    public ModularityViolationGroupDetector() : this(new MvgTresholds()) { }

    public void detect(AnalizerModel dataModel, ResultModel results)
    {

        SortedSet<int> fileSet = new SortedSet<int>();

        for(int core=0; core< dataModel.entityCount; core++)
        {
            if (fileSet.Contains(core)) continue; // it's already in set, and other files which form a pair will be detected later, as the relation is bidirectional

            //this antipatern contains "comutative" pairs,
            //and for the current index j, the pair (core,j<core) were already tested
            for (int j= core+1; j < dataModel.entityCount; j++) 
            {
                if (dataModel.SRelations[core, j] is not null || dataModel.SRelations[j, core] is not null) continue; // the two files must not in a relation

                if (dataModel.HRelations[core, j] is null) continue; // no structural or historical relation

                if (dataModel.HRelations[core, j].Properties["cochanges"] < treshold.cochange) continue; // not conected enough (?) - this is how it's defined

                fileSet.Add(core);
                fileSet.Add(j);
            }
        }

        // in the end, it's not really stated if it makes a difference if a file is the core of a MVC or not, or which are the properties which really defines a core
        //so I considered all the files which are in a MVG relation with another file
        foreach (int index in fileSet)
        {
            var entity = dataModel.Entities[index].Name;

            var value = new Dictionary<string, object>(); // no values, here only to not produce errors 

            results.add(index, AntipatternName, value);
        }
    }

    private MvgTresholds treshold;
}
