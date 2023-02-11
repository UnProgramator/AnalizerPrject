using DRSTool.CommonModels;
using DRSTool.Extractor.DataExtraction.Utils;
using DRSTool.FileHelper;

namespace DRSTool.Extractor.DataExtraction
{
    class GenericDataExtraction
    {
        public virtual void extract(string name, Dictionary<string, object> details)
        {
            if (name.EndsWith("-relation"))
            {
                extractEntityProperty(details);
            }
            else if (name.EndsWith("-entity-property"))
            {
                extractRelation(details);
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

        protected virtual void extractRelation(Dictionary<string, object> details)
        {
            string filePath = extractFilePath(details);
            Dictionary<string, string>[] properties = (Dictionary<string, string>[])details["property"];
            bool symmetric = details.ContainsKey("symmetric") && (bool)details["symmetric"];

            string entity1_field = (string)details["row"];
            string entity2_field = (string)details["column"];

            var fileHelper = new FileHelperFactory().getFileHelper(filePath);

            var content = fileHelper.getArrayContent<dynamic>(filePath);

            if (content == null)
                throw new Exception($"could not read file {filePath} or error at extraction using {fileHelper.GetType().Name}");

            foreach (Dictionary<string, object> relation in content)
            {
                string entity1 = (string)relation[entity1_field];
                string entity2 = (string)relation[entity2_field];

                foreach (var propName in properties)
                {
                    var rel = getProperty(propName, relation);
                    if(rel != null)
                        Model.addRelation(entity1, entity2, (KeyValuePair<string, dynamic>)rel, symmetric);
                }
            }
        }

        protected virtual void extractEntityProperty(Dictionary<string, object> details)
        {
            string filePath = extractFilePath(details);
            Dictionary<string, string>[] properties = (Dictionary<string, string>[])details["property"];
            string entity_field = (string)details["entity"];

            var fileHelper = new FileHelperFactory().getFileHelper(filePath);

            var content = fileHelper.getArrayContent<dynamic>(filePath);

            if (content == null)
                throw new Exception($"could not read file {filePath} or error at extraction using {fileHelper.GetType().Name}");

            foreach (Dictionary<string, object> relation in content)
            {
                string entity = (string)relation[entity_field];

                foreach (var property in properties)
                {
                    var rel = getProperty(property, relation);
                    if(rel != null)
                        Model.addEntityProperty(entity, (KeyValuePair<string, dynamic>)rel);
                }
            }
        }

        protected virtual string extractFilePath(Dictionary<string, object> details)
        {
            string filePath = (string)details["file"];

            if (!details.ContainsKey("ignore-root") || !(bool)details["ignore-root"])
                filePath = Root + filePath;

            return filePath;
        }

        private KeyValuePair<string, dynamic>? getProperty(Dictionary<string, string> property, Dictionary<string, object> relation)
        {
            var value = relation[property["field"]];

            if(!property.ContainsKey("condition") || fieldChecker.checkCondition(value, property["condition"]))
                return new KeyValuePair<string, dynamic>(property["property"], value);

            return null;
        }

        protected string Root { get; }
        protected AnalizerModel Model { get; }

        private FieldChecker fieldChecker;
    }
}
