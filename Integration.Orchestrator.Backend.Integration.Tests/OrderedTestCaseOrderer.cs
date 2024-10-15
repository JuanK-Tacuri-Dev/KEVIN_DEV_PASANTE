using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Integration.Orchestrator.Backend.Integration.Tests
{
    public class OrderedTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(tc => ((TestOrderAttribute)tc.TestMethod.Method.GetCustomAttributes(typeof(TestOrderAttribute)).First()).Order);

        }
    }
}
