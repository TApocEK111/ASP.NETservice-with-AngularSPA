namespace MonitoringAPI.Data;

public class StatisticsRecord : Entity
{
    public Guid DeviceId { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Version { get; set; }

    public StatisticsRecord() { }
    public StatisticsRecord(StatisticsRecordDTO dto)
    {
        DeviceId = dto._id;
        Name = dto.name;
        Version = dto.version;
        StartTime = dto.startTime;
        EndTime = dto.endTime;
    }
}