using DRSTool.CommonModels;
using DRSTool.CommonModels.Exceptions;

namespace DRSTool.Extractor.InternalModels;

class ConstructionModel : AnalizerModel
{
    private Dictionary<string, int> entitiesQuickIndex;

    private Dictionary<string, int> entitiesQuickFileName;    

    public ConstructionModel(int entityCount, string[] properties) : base(entityCount, properties)
    {
        entitiesQuickIndex = new Dictionary<string, int>(entityCount);
        entitiesQuickFileName = new Dictionary<string, int>(entityCount);
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
        addQuicKFullyQualified(properties, lastIndex);
        base.addEntity(name, properties);
    }

    private void addQuicKFullyQualified(Dictionary<string, dynamic>? properties, int index)
    {
        //for java
        if (properties != null && !properties["package"].Equals(""))
        {
            var fullyQualifiedName = properties["package"] + "." + properties["filename"].Remove(properties["filename"].IndexOf('.'));
            if (!entitiesQuickFileName.ContainsKey(fullyQualifiedName))
                entitiesQuickFileName.Add(fullyQualifiedName, index );
            else // if there are multiple classes with the same name in the same package we ignore them, because we cannot determin which to choose
                entitiesQuickFileName[fullyQualifiedName] = -1; 
        }
            
    }

    public AnalizerModel getBaseModel()
    {
        return shallowCopy();
    }

    public int getIndexForClass(string className)
    {
        if (entitiesQuickFileName.ContainsKey(className))
            return entitiesQuickFileName[className];
        else
            return -1;
    }
}
