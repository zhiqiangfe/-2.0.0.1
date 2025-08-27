using SUNWODA_SEVB.MES.Models;
using SUNWODA_SEVB.MES.Services;

namespace SUNWODA_SEVB.MES.RequestAndResponse
{
    /// <summary>
    /// MES服务使用示例
    /// </summary>
    public class MesServiceUsageExample
    {
        //private readonly IMesManagerService _mesServiceManager;

        //public MesServiceUsageExample(IMesManagerService mesServiceManager)
        //{
        //    _mesServiceManager = mesServiceManager;
        //}

        ///// <summary>
        ///// 使用示例
        ///// </summary>
        //public async Task ExampleUsage()
        //{
        //    // 检查MES是否启用
        //    if (!_mesServiceManager.IsEnabled)
        //    {
        //        Console.WriteLine("MES services are not enabled for this project");
        //        return;
        //    }

        //    // 上传离线数据
        //    var offlineService = _mesServiceManager.GetOfflineDataUploadService();
        //    var testData = new List<TestData>
        //    {
        //        new TestData
        //        {
        //            Name = "Voltage",
        //            Value = "3.7",
        //            Unit = "V",
        //            Result = "OK",
        //            Upper = "4.2",
        //            Lower = "3.0"
        //        }
        //    };

        //    var offlineResult = await offlineService.UploadAsync(
        //        productSn: "SN123456789",
        //        testResult: "OK",
        //        testDatas: testData
        //    );

        //    if (offlineResult.Success)
        //    {
        //        Console.WriteLine("Offline data uploaded successfully");
        //    }

        //    // 上传Marking数据
        //    var markingService = _mesServiceManager.GetMarkingDataUploadService();
        //    var markingResult = await markingService.UploadAsync(
        //        productSn: "SN123456789",
        //        defectList: new List<string> { "Minor scratch", "Color variation" }
        //    );

        //    if (markingResult.Success)
        //    {
        //        Console.WriteLine("Marking data uploaded successfully");
        //    }

        //    // 检查所有服务健康状态
        //    var healthResults = await _mesServiceManager.CheckAllServicesHealthAsync();
        //    foreach (var health in healthResults)
        //    {
        //        Console.WriteLine($"Service: {health.Key}, Healthy: {health.Value}");
        //    }
        //}
    }
}
