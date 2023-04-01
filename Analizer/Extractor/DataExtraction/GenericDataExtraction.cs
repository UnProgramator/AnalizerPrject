using DRSTool.CommonModels;
using DRSTool.Extractor.DataExtraction.Utils;
using DRSTool.FileHelper;

namespace DRSTool.Extractor.DataExtraction
{
    class GenericDataExtraction
    {
        public virtual void extract(string name, dynamic details)
        {
            if (name.EndsWith("-srelation"))
            {
                extractRelation(details);
            }
            else if (name.EndsWith("-hrelation"))
            {
                throw new NotImplementedException();
            }
            else if (name.EndsWith("-entity-property"))
            {
                extractEntityProperty(details);
            }
            else
                throw new Exception($"Incorrect property name {name}");
        }

        public GenericDataExtraction(string root, AnalizerModel model)
        {
            this.Root = root;
            this.Model = model;
            this.fieldChecker = new FieldChecker();
        }

        protected virtual void extractRelation(dynamic details)
        {
            string filePath = extractFilePath(details);
            dynamic properties = details["property"];
            bool symmetric = details.ContainsKey("symmetric") && (bool)details["symmetric"];

            string entity1_field = (string)details["row"];
            string entity2_field = (string)details["column"];

            var fileHelper = new FileHelperFactory().getFileHelper(filePath);

            var content = fileHelper.getArrayContent<dynamic>(filePath);

            if (content == null)
                throw new Exception($"could not read file {filePath} or error at extraction using {fileHelper.GetType().Name}");

            foreach (IDictionary<string, object> relation in content)
            {
                string entity1 = (string)relation[entity1_field];
                string entity2 = (string)relation[entity2_field];

                foreach (var propName in properties)
                {
                    var rel = getProperty(propName, relation);
                    if(rel is not null)
                        Model.addStructuralRelation(entity1, entity2, (KeyValuePair<string, dynamic>)rel, symmetric);
                }
            }
        }

        protected virtual void extractEntityProperty(dynamic details)
        {
            string filePath = extractFilePath(details);
            dynamic properties = details["property"];
            string entity_field = (string)details["entity"];

            var fileHelper = new FileHelperFactory().getFileHelper(filePath);

            var content = fileHelper.getArrayContent<dynamic>(filePath);

            if (content == null)
                throw new Exception($"could not read file {filePath} or error at extraction using {fileHelper.GetType().Name}");

            foreach (IDictionary<string, object> relation in content)
            {
                string entity = (string)relation[entity_field];

                foreach (var property in properties)
                {
                    var rel = getProperty(property, relation);
                    if(rel != null)
                        Model.addEntityProperty((string)entity, (KeyValuePair<string, dynamic>)rel);
                }
            }
        }

        protected virtual string extractFilePath(dynamic details)
        {
            return FileUtilities.getFileFromDefaultConfig(Root, details);
        }

        private KeyValuePair<string, object>? getProperty(dynamic property, IDictionary<string, object> relation)
        {
            string field = (string)property["field"];
            var value = relation[field];

            if(!property.ContainsKey("condition") || fieldChecker.checkCondition(value, property["condition"]))
                return new KeyValuePair<string, object>((string)property["property"], value);

            return null;
        }

        protected string Root { get; }
        protected AnalizerModel Model { get; }

        private FieldChecker fieldChecker;
    }
}
