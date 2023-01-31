namespace Analizer.CommonModels
{
    class AnalizerModel
    {
        public string[] Properties { get; private set; }
        public EntityInformation[] Entities { get; private set; }
        public EntitiesRelations[,] Relations { get; private set; }

        private int entityCount;

        protected int lastIndex { private set; get; }

        private AnalizerModel() { }

        public AnalizerModel(int entityCount, string[] properties)
        {
            Entities = new EntityInformation[entityCount];

            Relations = new EntitiesRelations[entityCount,entityCount];

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
    }
}
