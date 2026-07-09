using System.Net.NetworkInformation;

namespace HTHIUM.Tool.Helper
{
    public static class NetworkHelper
    {
        public class NetworkInfo
        {
            public string? MacAddress { get; set; }
            public string? IpAddress { get; set; }
        }

        public static NetworkInfo GetNetworkInfo(string ipPrefix = "10.")
        {
            var result = new NetworkInfo();

            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                 ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                 !string.IsNullOrEmpty(ni.GetPhysicalAddress().ToString()));

                foreach (var ni in networkInterfaces)
                {
                    var ipProperties = ni.GetIPProperties();
                    var unicastAddresses = ipProperties.UnicastAddresses
                        .Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    foreach (var address in unicastAddresses)
                    {
                        var ip = address.Address.ToString();
                        if (ip.StartsWith(ipPrefix))
                        {
                            result.IpAddress = ip;
                            result.MacAddress = ni.GetPhysicalAddress().ToString().ToLower();
                            return result;
                        }
                    }
                }

                // 如果没找到指定前缀的IP，返回第一个可用的
                var firstInterface = networkInterfaces.FirstOrDefault();
                if (firstInterface != null)
                {
                    var firstIp = firstInterface.GetIPProperties().UnicastAddresses
                        .FirstOrDefault(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    if (firstIp != null)
                    {
                        result.IpAddress = firstIp.Address.ToString();
                        result.MacAddress = firstInterface.GetPhysicalAddress().ToString().ToLower();
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出
                Console.WriteLine($"获取网络信息失败: {ex.Message}");
            }

            return result;
        }
    }
}
