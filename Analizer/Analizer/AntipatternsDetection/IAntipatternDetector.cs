using DRSTool.Analizer.Models;
using DRSTool.CommonModels;

namespace DRSTool.Analizer.AntipatternsDetection;

interface IAntipatternDetector
{
    void detect(AnalizerModel dataModel, ResultModel results);
}
