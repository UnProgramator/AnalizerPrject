using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizer.CommonModels
{
    internal class AnalizerCompresedModel
    {
        public int entityCount { get; private set; }
        public string[] Properties { get; private set; }
        public EntityInformation[] Entities { get; private set; }
        public Dictionary<Tuple<int,int>,EntitiesRelations> Relations { get; private set; }

        public AnalizerCompresedModel(int entityCount, string[] properties, EntityInformation[] entities)
        {
            this.entityCount = entityCount;
            Properties = properties;
            Entities = entities;
            Relations = new Dictionary<Tuple<int, int>, EntitiesRelations>();
        }
    }
}
