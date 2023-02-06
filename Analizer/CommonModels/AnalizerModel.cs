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

    public void addRelation(int index1, int index2, KeyValuePair<string,dynamic> relation)
    {
        if (Relations[index1, index2] is null)
            Relations[index1, index2] = new EntitiesRelations(relation);
        else
            Relations[index1, index2].addProperty(relation);
    }

    public virtual void addEntity(string name, Dictionary<string, dynamic>? properties = null)
    {
        if (lastIndex == entityCount)
            throw new IndexOutOfRangeException("Trying to add more entities than initially declared");
        Entities[lastIndex] = new EntityInformation(name, properties);
        lastIndex++;
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
