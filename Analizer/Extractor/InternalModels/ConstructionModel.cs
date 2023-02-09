using DRSTool.CommonModels;
using DRSTool.CommonModels.Exceptions;

namespace DRSTool.Extractor.InternalModels;

class ConstructionModel : AnalizerModel
{
    private Dictionary<string, int> entitiesQuickIndex;

    public ConstructionModel(int entityCount, string[] properties) : base(entityCount, properties)
    {
        entitiesQuickIndex = new Dictionary<string, int>(entityCount);
    }

    protected override int getIndexForEntity(string entityName)
    {
        if (!entitiesQuickIndex.ContainsKey(entityName))
            throw new EntityUsedButNotDeclaredException($"Entity {entityName} not declared prior to adding relation implication");
        return entitiesQuickIndex[entityName];
    }

    public override void addEntity(string name, Dictionary<string, dynamic>? properties = null)
    {
        entitiesQuickIndex.Add(name, lastIndex);
        base.addEntity(name, properties);
    }

    public AnalizerModel getBaseModel()
    {
        return shallowCopy();
    }
}
