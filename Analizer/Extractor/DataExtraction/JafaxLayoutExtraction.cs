using DRSTool.CommonModels;
using DRSTool.FileHelper;

namespace DRSTool.Extractor.DataExtraction;

class JafaxLayoutExtraction
{
    public static void extract(AnalizerModel model, string fileName, string? nameTrim)
    {
        var fileReader = new FileHelperFactory().getJsonHelper();
        var input = fileReader.getArrayContent<Dictionary<string, object>>(fileName);

        if(input == null)
            throw new Exception("Read or parse error or file Empty");

        var classes = input.Where(x => x.ContainsKey("type") && x["type"] is not null && x["type"].Equals("Class"));

        Dictionary<long, string> entitesJafaxIdToName = new Dictionary<long, string>();

        foreach (var x in input)
        {
            string name = (string)x["fileName"];
            if(nameTrim != null)
                name = name.Substring(nameTrim.Length);
            //name = name.Substring(0, name.LastIndexOf('/')) + (string)x["name"];

            entitesJafaxIdToName.Add((long)x["id"], name);

            if (x.ContainsKey("isInterface") && x["isInterface"].Equals(true))
                model.addEntityProperty(name, new KeyValuePair<string, dynamic>("isInterface", true));
        }

        foreach (var x in input)
        {
            string thisName = entitesJafaxIdToName[(long)x["id"]];
            if (x.ContainsKey("superClass"))
            {
                long id = (long)x["superClass"];
                string superName = entitesJafaxIdToName[id]; 
                model.addStructuralRelation(thisName, superName, new KeyValuePair<string, dynamic>("inheritance", true));
            }
            if (x.ContainsKey("interfaces"))
            {
                foreach(long id in (long[])x["interfaces"])
                {
                    string interName = entitesJafaxIdToName[id];
                    model.addStructuralRelation(thisName, interName, new KeyValuePair<string, dynamic>("inheritance", true));
                }
            }
        }
    }
}
