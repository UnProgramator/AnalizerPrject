namespace DRSTool.CommonModels;

class EntityInformation
{
    public string Name { get; }

    public Dictionary<string, dynamic>? Properties { get; private set; } = null;

    public EntityInformation(string name, Dictionary<string, dynamic>? properties = null)
    {
        Name = name;
        if (properties != null)
            Properties = properties;
    }

    public void addProperty(KeyValuePair<string, dynamic> property)
    {
        if (Properties is null)
            Properties = new Dictionary<string, dynamic>();

        if (Properties.ContainsKey(property.Key))
            throw new Exception($"Entity {Name} already containts a property with the name [{property.Key}] with the value "
                                 +  $"[{Properties[property.Key]}] but a new value [{property.Value}] tried to be inserted");

        Properties.Add(property.Key, property.Value);
    }
}
