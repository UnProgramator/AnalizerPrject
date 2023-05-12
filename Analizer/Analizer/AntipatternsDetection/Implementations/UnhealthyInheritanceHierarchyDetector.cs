using DRSTool.Analizer.AntipatternsDetection.Utilities;
using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

internal class UnhealthyInheritanceHierarchyDetector : IAntipatternDetector
{
    private InheritanceTree tree;

    public const string AntipatternName = "UIH";

    public UnhealthyInheritanceHierarchyDetector(InheritanceTree tree)
    {
        this.tree = tree;
    }

    public UnhealthyInheritanceHierarchyDetector() 
    { 
        this.tree = new InheritanceTree();
    }

    public void detect(AnalizerModel dataModel, ResultModel results)
    {
        if (!tree.isInitialized())
            tree.init(dataModel);

        base_depend_on_derived(dataModel, results);
        class_depends_on_base_and_derived(dataModel, results);
    }

    private void base_depend_on_derived(AnalizerModel dataModel, ResultModel results)
    {
        for(int b=0; b<dataModel.Entities.Length; b++)
        {
            for(int d=0; d< dataModel.Entities.Length; d++)
            {
                //the quick checks
                if (dataModel.SRelations[b, d] == null) continue; // no structural dependency from the "potential" parent to the derived
                if (!dataModel.SRelations[b, d].Properties.ContainsKey("dependency")) continue; //no dependency involved () ??? this may be removed?

                if (dataModel.SRelations[d, b] == null) continue; // no structural relation at all; for i==j M[i,j] is null by default
                
                //the costly checks for indirect inheritance

                if (!tree.inherits(d, b)) continue; // check for indirect inheritance

                var value = new Dictionary<string, object> {
                    { "type", "base-depends-on-derived" },
                    { "participant", "isBase" }
                };

                results.add(b, AntipatternName, value);

                value = new Dictionary<string, object> {
                    { "type", "base-depends-on-derived" },
                    { "participant", "isDerived" }
                };
                results.add(d, AntipatternName, value);

                //break; // there may be problems if it is the case a base depends on multiple deriveds. If ti depends on one or on more is the same
            } 
        }
    }

    private void class_depends_on_base_and_derived(AnalizerModel dataModel, ResultModel results)
    {
        for (int c = 0; c < dataModel.Entities.Length; c++)
        {
            for (int b = 0; b < dataModel.Entities.Length; b++)
            {
                if (dataModel.SRelations[c, b] == null) continue; // no structural dependency of class c on the potential base class

                // class c inherits from b, case already threated
                // but what if a class depends on it's base and on of it's base derivedes from another derivation sub-tree!
                // if (dataModel.SRelations[c, b].Properties.ContainsKey("inheritance")) continue; 
                

                foreach(var d in tree.getChildren(b))
                {
                    //if c and d are the same, we continue; the diagonal must be empty
                    if (dataModel.SRelations[c, b] == null) continue; // no UIH here

                    var _base = dataModel.Entities[b].Name;
                    var _class = dataModel.Entities[c].Name;
                    var _derived = dataModel.Entities[d].Name;

                    var value_base = new Dictionary<string, object> {
                        { "type", "class-depends-on-base-and-derived" },
                        { "participant", "base" },
                        { "class", _class },
                        { "derived-class", _derived }
                    };

                    results.add(b, AntipatternName, value_base);

                    var value_derived = new Dictionary<string, object> {
                        { "type", "class-depends-on-base-and-derived" },
                        { "participant", "derived" },
                        { "class", _class }
                    };

                    results.add(d, AntipatternName, value_derived);

                    var value_class = new Dictionary<string, object> {
                        { "type", "class-depends-on-base-and-derived" },
                        { "participant", "class" },
                        { "base-class", _base },
                        { "derived-class", _derived }
                    };

                    results.add(c, AntipatternName, value_class);

                    //break; //it's enough if it occured for a derived of the analized base class // i changed my mind recently, so... yea, it isn't enoguh for now
                }
            }
        }
    }


}
