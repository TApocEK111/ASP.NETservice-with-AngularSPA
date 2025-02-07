namespace MonitoringAPI.Data;

public class StatisticsData(double duration, List<string> names, List<string> versions, string name, string version)
{
    public double AverageSessionDurationMs { get; set; } = duration;
    public List<string> UniqueNames { get; set; } = names;
    public List<string> UniqueVersions { get; set; } = versions;
    public string LatestName { get; set; } = name;
    public string LatestVersion { get; set; } = version;
}