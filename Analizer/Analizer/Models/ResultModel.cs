using DRSTool.CommonModels;
using System.Security.Cryptography;

namespace DRSTool.Analizer.Models;

class ResultModel
{
    ResultEntityModel[] resultEntity;

    private ResultModel(AnalizerModel model)
    {
        resultEntity = new ResultEntityModel[model.entityCount];
        int i = 0;
        foreach (var res in model.Entities)
        {
            resultEntity[i++] = ResultEntityModel.fromBase(res);
        }
    }

    public static ResultModel fromExtractorModel(AnalizerModel model) => new ResultModel(model);

    public void add(int entityID, string antipatternName, Dictionary<string, object> value)
    {
        if (resultEntity[entityID].antipatterns.ContainsKey(antipatternName))
            resultEntity[entityID].antipatterns[antipatternName].Add(value);
        else
            resultEntity[entityID].antipatterns.Add(antipatternName, new List<Dictionary<string, object>> { value });
    }
}
