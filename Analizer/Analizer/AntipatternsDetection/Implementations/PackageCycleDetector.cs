using DRSTool.Analizer.Models;
using DRSTool.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations
{
    internal class PackageCycleDetector: IAntipatternDetector
    {
        public const string AntipatternName = "PKC";

        public void detect(AnalizerModel dataModel, ResultModel results)
        {
            throw new NotImplementedException();
        }
    }
}
