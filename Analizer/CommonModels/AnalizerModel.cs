using DRSTool.CommonModels.Exceptions;

namespace DRSTool.CommonModels;

class AnalizerModel
{
    public string[] Properties { get; private set; }
    public EntityInformation[] Entities { get; private set; }
    public EntitiesRelations[,] Relations { get; private set; }

    private int entityCount;

    protected int lastIndex { private set; get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// to be used only internaly. Don;t forget to initialize all fields after call
    /// </summary>
    private AnalizerModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public AnalizerModel(int entityCount, string[] properties)
    {
        Entities = new EntityInformation[entityCount];

        Relations = new EntitiesRelations[entityCount,entityCount];

        Properties = properties;

        this.entityCount = entityCount;

        lastIndex = 0;
    }

    protected AnalizerModel(int entityCount, string[] properties, EntityInformation[] entities)
    {
        Entities = entities;

        Relations = new EntitiesRelations[entityCount, entityCount];

        Properties = properties;

        this.entityCount = entityCount;

        lastIndex = 0;
    }

    public virtual void addEntity(string name, Dictionary<string, dynamic>? properties = null)
    {
        if (lastIndex == entityCount)
            throw new IndexOutOfRangeException("Trying to add more entities than initially declared");
        Entities[lastIndex] = new EntityInformation(name, properties);
        lastIndex++;
    }

    public void addRelation(int index1, int index2, KeyValuePair<string,dynamic> relation, bool symmtric = false)
    {
        if (Relations[index1, index2] is null)
            Relations[index1, index2] = new EntitiesRelations(relation);
        else
            Relations[index1, index2].addProperty(relation);

        if (symmtric)
            addRelation(index2, index1, relation, false);
    }

    public void addRelation(string entity1, string entity2, KeyValuePair<string, dynamic> relation, bool symmtric = false)
    {
        int index1 = getIndexForEntity(entity1);
        int index2 = getIndexForEntity(entity2);

        addRelation(index1, index2, relation, symmtric);
    }

    public void addEntityProperty(int entityIndex, KeyValuePair<string, dynamic> property)
    {
        Entities[entityIndex].addProperty(property);
    }

    public void addEntityProperty(string entityName, KeyValuePair<string, dynamic> property)
    {
        int index = getIndexForEntity(entityName);
        addEntityProperty(index, property);
    }

    protected virtual int getIndexForEntity(string entityName)
    {
        for (int i = 0; i < Entities.Length; i++)
        {
            if (Entities[i].Name.Equals(entityName))
                return i;
        }
        throw new EntityUsedButNotDeclaredException($"Entity {entityName} not declared prior to adding relation implication");
    }

    protected AnalizerModel shallowCopy()
    {
        AnalizerModel newObject = new AnalizerModel();

        newObject.Properties = Properties;
        newObject.Entities = Entities;
        newObject.Relations = Relations;
        newObject.entityCount = entityCount;
        newObject.lastIndex = entityCount;

        return newObject;
    }

    public AnalizerCompresedModel compress()
    {
        AnalizerCompresedModel compresedModel = new AnalizerCompresedModel(entityCount, Properties, Entities);
        for (int i = 0; i < entityCount; i++)
            for (int j = 0; j < entityCount; j++)
                if (Relations[i, j] != null)
                    compresedModel.Relations.Add(Tuple.Create(i, j), Relations[i, j]);
        return compresedModel;
    }

    public static AnalizerModel decompress(AnalizerCompresedModel compressedModel)
    {
        AnalizerModel model = new AnalizerModel(compressedModel.entityCount, compressedModel.Properties, compressedModel.Entities);
        foreach(var x in compressedModel.Relations)
        {
            int i = x.Key.Item1;
            int j = x.Key.Item2;
            model.Relations[i, j] = x.Value;
        }

        return model;
    }
}
