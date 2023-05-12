using DRSTool.CommonModels;

namespace DRSTool.Analizer.Models;

class ResultEntityModel : EntityInformation
{
    public Dictionary<string, List<Dictionary<string, object>>> antipatterns { private set; get; }

    public ResultEntityModel(string name, Dictionary<string, dynamic>? properties = null) : base(name, properties)
    {
        antipatterns = new Dictionary<string, List<Dictionary<string, object>>>();
    }
    public static ResultEntityModel fromBase(EntityInformation old) => new ResultEntityModel(old.Name, old.Properties);
}