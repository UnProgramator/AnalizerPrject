using DRSTool.CommonModels;

namespace DRSTool.Extractor.InternalModels;

class ConstructionModel : AnalizerModel
{
    private Dictionary<string, int> entitiesQuickIndex;

    public ConstructionModel(int entityCount, string[] properties) : base(entityCount, properties)
    {
        entitiesQuickIndex = new Dictionary<string, int>(entityCount);
    }

    public void addRelation(string entity1, string entity2, KeyValuePair<string, dynamic> relation, bool symmetric = false)
    {
        if (!entitiesQuickIndex.ContainsKey(entity1))
            throw new EntityUsedButNotDeclaredException($"Entity {entity1} not declared prior to adding relation implication");

        if (!entitiesQuickIndex.ContainsKey(entity2))
            throw new EntityUsedButNotDeclaredException($"Entity {entity2} not declared prior to adding relation implication");

        int index1 = entitiesQuickIndex[entity1];
        int index2 = entitiesQuickIndex[entity2];

        addRelation(index1, index2, relation, symmetric);
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

class EntityUsedButNotDeclaredException : Exception
{
    public EntityUsedButNotDeclaredException() : base() { }
    public EntityUsedButNotDeclaredException(string message) : base(message) { }
}
