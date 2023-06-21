using DRSTool.CommonModels;
using System.Security.Cryptography;

namespace DRSTool.Analizer.Models;

class ResultModel
{
    public ResultEntityModel[] resultEntity { get; private set; }

    public ResultModel(AnalizerModel model)
    {
        resultEntity = new ResultEntityModel[model.entityCount];
        int i = 0;
        foreach (var res in model.Entities)
        {
            resultEntity[i++] = ResultEntityModel.fromBase(res);
        }
    }

    public void add(int entityID, string antipatternName, Dictionary<string, object> value)
    {
        if (resultEntity[entityID].Antipatterns.ContainsKey(antipatternName))
            resultEntity[entityID].Antipatterns[antipatternName].Add(value);
        else
            resultEntity[entityID].Antipatterns.Add(antipatternName, new List<Dictionary<string, object>> { value });
    }

    public ResultAggregationModel[] aggregate()
    {
        ResultAggregationModel[] result = new ResultAggregationModel[resultEntity.Count()];

        int i=0;

        foreach(var file in resultEntity)
        {
            result[i++] = new ResultAggregationModel(file);
        }

        return result;
    }
}
