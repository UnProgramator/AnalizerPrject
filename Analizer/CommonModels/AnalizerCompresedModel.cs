namespace DRSTool.CommonModels;
internal class AnalizerCompresedModel
{
    public int entityCount { get; private set; }
    public string[] Properties { get; private set; }
    public EntityInformation[] Entities { get; private set; }
    public Dictionary<Tuple<int,int>,EntitiesRelations> StrucutralRelations { get; private set; }
    public Dictionary<Tuple<int,int>,EntitiesRelations> HistoryRelations { get; private set; }

    public AnalizerCompresedModel(int entityCount, string[] properties, EntityInformation[] entities)
    {
        this.entityCount = entityCount;
        Properties = properties;
        Entities = entities;
        StrucutralRelations = new Dictionary<Tuple<int, int>, EntitiesRelations>();
        HistoryRelations = new Dictionary<Tuple<int, int>, EntitiesRelations>();
    }
}
