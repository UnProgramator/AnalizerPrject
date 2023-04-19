using DRSTool.CommonModels.Exceptions;

namespace DRSTool.CommonModels;

class AnalizerModel
{
    public string[] Properties { get; private set; }
    public EntityInformation[] Entities { get; private set; }
    public EntitiesRelations[,] SRelations { get; private set; }
    public EntitiesRelations[,] HRelations { get; private set; }

    public int entityCount { get; private set; }

    protected int lastIndex { private set; get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// to be used only internaly. Don;t forget to initialize all fields after call
    /// </summary>
    private AnalizerModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public AnalizerModel(int entityCount, string[] properties): this(entityCount, properties, new EntityInformation[entityCount])
    {
    }

    protected AnalizerModel(int entityCount, string[] properties, EntityInformation[] entities)
    {
        Entities = entities;

        SRelations = new EntitiesRelations[entityCount, entityCount];

        HRelations = new EntitiesRelations[entityCount, entityCount];

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



    public void addStructuralRelation(int index1, int index2, KeyValuePair<string,dynamic> relation, bool symmtric = false)
    {
        if (SRelations[index1, index2] is null)
            SRelations[index1, index2] = new EntitiesRelations(relation);
        else
            SRelations[index1, index2].addProperty(relation);

        if (symmtric)
            addStructuralRelation(index2, index1, relation, false);
    }

    public void addStructuralRelation(string entity1, string entity2, KeyValuePair<string, dynamic> relation, bool symmtric = false)
    {
        int index1 = getIndexForEntity(entity1);
        int index2 = getIndexForEntity(entity2);

        addStructuralRelation(index1, index2, relation, symmtric);
    }

    public void addHistoryRelation(int index1, int index2, KeyValuePair<string,dynamic> relation, bool symmtric = false)
    {
        if (HRelations[index1, index2] is null)
            HRelations[index1, index2] = new EntitiesRelations(relation);
        else
            HRelations[index1, index2].addProperty(relation);

        if (symmtric)
            addHistoryRelation(index2, index1, relation, false);
    }

    public void addHistoryRelation(string entity1, string entity2, KeyValuePair<string, dynamic> relation, bool symmtric = false)
    {
        int index1 = getIndexForEntity(entity1);
        int index2 = getIndexForEntity(entity2);

        addHistoryRelation(index1, index2, relation, symmtric);
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
        newObject.SRelations = SRelations;
        newObject.entityCount = entityCount;
        newObject.lastIndex = entityCount;

        return newObject;
    }

    public AnalizerCompresedModel compress()
    {
        AnalizerCompresedModel compresedModel = new AnalizerCompresedModel(entityCount, Properties, Entities);

        for (int i = 0; i < entityCount; i++)
            for (int j = 0; j < entityCount; j++)
                if (SRelations[i, j] != null)
                    compresedModel.StrucutralRelations.Add(Tuple.Create(i, j), SRelations[i, j]);

        for (int i = 0; i < entityCount; i++)
            for (int j = 0; j < entityCount; j++)
                if (HRelations[i, j] != null)
                    compresedModel.HistoryRelations.Add(Tuple.Create(i, j), HRelations[i, j]);

        return compresedModel;
    }

    public static AnalizerModel decompress(AnalizerCompresedModel compressedModel)
    {
        AnalizerModel model = new AnalizerModel(compressedModel.entityCount, compressedModel.Properties, compressedModel.Entities);

        foreach(var x in compressedModel.StrucutralRelations)
        {
            int i = x.Key.Item1;
            int j = x.Key.Item2;
            model.SRelations[i, j] = x.Value;
        }

        foreach (var x in compressedModel.HistoryRelations)
        {
            int i = x.Key.Item1;
            int j = x.Key.Item2;
            model.HRelations[i, j] = x.Value;
        }

        return model;
    }
}
