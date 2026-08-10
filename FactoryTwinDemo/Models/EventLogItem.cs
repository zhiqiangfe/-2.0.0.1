namespace FactoryTwinDemo.Models;

public sealed record EventLogItem(DateTime Time, string Level, string Message)
{
    public string TimeText => Time.ToString("HH:mm:ss");
}
