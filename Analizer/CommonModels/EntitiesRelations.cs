namespace DRSTool.CommonModels;

class EntitiesRelations
{
    public Dictionary<string, dynamic> Properties { get; }

    public void addProperty(KeyValuePair<string, dynamic> property)
    {
        Properties.Add(property.Key, property.Value);
    }

    public EntitiesRelations()
    {
        Properties = new Dictionary<string, dynamic>();
    }

    public EntitiesRelations(KeyValuePair<string, dynamic> property) : this()
    {
        Properties.Add(property.Key, property.Value);
    }
}
