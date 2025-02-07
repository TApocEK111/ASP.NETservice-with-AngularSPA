using MonitoringAPI.Controllers;
using MonitoringAPI.Data;
using System.Data;
using System.Text;
using System.Text.Json;

namespace MonitoringAPI.Repositories;

public class DevicesMemoryRepository
{
    private Dictionary<Guid, List<StatisticsRecord>> data { get; } = new();
    private readonly string backupPath = "monitoringDevicesBackup.txt";
    private readonly ILogger<DevicesMemoryRepository> logger;

    public DevicesMemoryRepository(ILogger<DevicesMemoryRepository> lg)
    {
        logger = lg;
        LoadBackup();
    }

    public Dictionary<Guid, List<StatisticsRecord>>.KeyCollection GetAllDevices()
    {
        return data.Keys;
    }

    public List<StatisticsRecord> GetRecords(Guid id)
    {
        return data[id];
    }

    public void Add(StatisticsRecord item)
    {
        if (!data.ContainsKey(item.DeviceId)) data.Add(item.DeviceId, new List<StatisticsRecord>());
        
        data[item.DeviceId].Add(item);
    }

    public void DeleteDevice(Guid id)
    {
        data.Remove(id);
    }

    public void DeleteRecord(Guid deviceId, Guid recordId)
    {
        data[deviceId].Remove(data[deviceId].Where(x => x.Id == recordId).First());
        if (data[deviceId].Count == 0) DeleteDevice(deviceId);
        DoBackupAsync();
    }

    public async Task DoBackupAsync()
    {
        var recs = data.Values.ToList();
        var recsJson = JsonSerializer.Serialize(recs);

        await File.WriteAllTextAsync(backupPath, recsJson);
    }

    public void LoadBackup()
    {
        if (File.Exists(backupPath))
        {
            var json = File.ReadAllText(backupPath);
            var recs = JsonSerializer.Deserialize<List<List<StatisticsRecord>>>(json);

            foreach (var rec in recs)
            {
                if (!data.TryAdd(rec[0].DeviceId, rec))
                {
                    var list = new List<StatisticsRecord>(rec.Count + data[rec[0].DeviceId].Count);
                    list.AddRange(data[rec[0].DeviceId]);
                    list.AddRange(rec);
                    data[rec[0].DeviceId] = list;
                }
            }
        }
    }
}