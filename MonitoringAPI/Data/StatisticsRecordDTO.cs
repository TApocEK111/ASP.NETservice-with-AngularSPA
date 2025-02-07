namespace MonitoringAPI.Data
{
    public class StatisticsRecordDTO
    {
        public Guid _id { get; set; }
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string version { get; set; }
    }
}
