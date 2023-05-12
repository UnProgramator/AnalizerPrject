using CsvHelper.Configuration.Attributes;
using DRSTool.Analizer.AntipatternsDetection.Implementations;
using DRSTool.Extractor.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRSTool.Analizer.Models
{
    class ResultAggregationModel
    {
        public ResultAggregationModel(ResultEntityModel src)
        {
            filename = src.Name;

            setProjectName();

            //antipatterns
            if (src.antipatterns.ContainsKey(CliqueDetector.AntipatternName)) CLQ = 1;
            if (src.antipatterns.ContainsKey(CrossingDetector.AntipatternName)) CRS = 1;
            if (src.antipatterns.ContainsKey(UnhealthyInheritanceHierarchyDetector.AntipatternName)) UIH = 1;
            if (src.antipatterns.ContainsKey(UnstableInterfaceDetection.AntipatternName)) UIF = 1;
            if (src.antipatterns.ContainsKey(ModularityViolationGroupDetector.AntipatternName)) MVG = 1;
            if (src.antipatterns.ContainsKey(PackageCycleDetector.AntipatternName)) PKC = 1;

            if (src.Properties is null) throw new Exception("Properties cannot be null");

            //properties
            if (src.Properties.ContainsKey("LOC")) LOC = src.Properties["LOC"];
            if (src.Properties.ContainsKey("component")) component = src.Properties["component"];
            if (src.Properties.ContainsKey("functions")) FUN = getLong(src.Properties["functions"]);
            if (src.Properties.ContainsKey("is-interface")) isInterface = 1;
            if (src.Properties.ContainsKey("is-abstract")) isAbstract = 1;

            //testing
            if (src.Properties.ContainsKey("test-class-total-tests")) testNumber = src.Properties["test-class-total-tests"];
            if (src.Properties.ContainsKey("test-class-errors")) testErrors = src.Properties["test-class-errors"];
            if (src.Properties.ContainsKey("test-class-failures")) testFailures = src.Properties["test-class-failures"];
            if (src.Properties.ContainsKey("test-class-skipped")) testSkipped = src.Properties["test-class-skipped"];
            if (src.Properties.ContainsKey("test-time")) testTime = src.Properties["test-time"];
            if (src.Properties.ContainsKey("test-class")) isTestCase = 1;

            //jacoco
            if (src.Properties.ContainsKey("instructions-missed")) instructMissed = src.Properties["instructions-missed"];
            if (src.Properties.ContainsKey("instructions-covered")) instructCovered = src.Properties["instructions-covered"];
            if (src.Properties.ContainsKey("functions-missed")) methodMissed = src.Properties["functions-missed"];
            if (src.Properties.ContainsKey("functions-covered")) methodCovered = src.Properties["functions-covered"];
            if (src.Properties.ContainsKey("branch-missed")) branchMissed = src.Properties["branch-missed"];
            if (src.Properties.ContainsKey("branch-covered")) branchCovered = src.Properties["branch-covered"];
            if (src.Properties.ContainsKey("complexity-missed")) complexityMissed = src.Properties["complexity-missed"];
            if (src.Properties.ContainsKey("complexity-covered")) complexityCovered = src.Properties["complexity-covered"];
        }

        private void setProjectName()
        {
            var prjConfig = ConfigValidator.getInstance().config;
            if (prjConfig.projectName is not null)
                projectName = prjConfig.projectName;
        }

        private long getLong(object o)
        {
            if (o is long) return (long)o;
            if (o is string) return long.Parse((string)o);
            throw new Exception();
        }

        public string projectName { get; set; } = "";
        public string component { get; set; } = "";
        public string filename { get; set; }
        public long LOC { get; set; } = 0;
        public long FUN { get; set; } = 0;
        public long isInterface { get; set; } = 0;
        public long isTestCase { get; set; } = 0;
        public long isAbstract { get; set; } = 0;
        public long CLQ { get; set; } = 0;
        public long CRS { get; set; } = 0;
        public long UIH { get; set; } = 0;
        public long UIF { get; set; } = 0;
        public long MVG { get; set; } = 0;
        public long PKC { get; set; } = 0;
        public long testNumber { get; set; } = 0;
        public long testErrors { get; set; } = 0;
        public long testSkipped { get; set; } = 0;
        public long testFailures { get; set; } = 0;
        public double testTime { get; set; } = 0;
        public long instructMissed { get; set; } = 0;
        public long instructCovered { get; set; } = 0;
        public long methodMissed { get; set; } = 0;
        public long methodCovered { get; set; } = 0;
        public long branchMissed { get; set; } = 0;
        public long branchCovered { get; set; } = 0;
        public long complexityMissed { get; set; } = 0;
        public long complexityCovered { get; set; } = 0;
    }
}
