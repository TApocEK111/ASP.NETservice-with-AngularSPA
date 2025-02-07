using Microsoft.AspNetCore.Mvc;
using MonitoringAPI.Data;
using MonitoringAPI.Repositories;

namespace MonitoringAPI.Controllers;

[ApiController]
[Route("api/monitoring")]
public class MonitoringController : ControllerBase
{
    private readonly DevicesMemoryRepository deviceRepo;
    private readonly ILogger<MonitoringController> logger;

    public MonitoringController(DevicesMemoryRepository dRepo, ILogger<MonitoringController> lg)
    {
        deviceRepo = dRepo;
        logger = lg;
    }

    [HttpGet]
    public IActionResult GetAllDevices()
    {
        return Ok(deviceRepo.GetAllDevices());
    }
    [HttpGet("backup")]
    public async Task<IActionResult> DoBackup()
    {
        await deviceRepo.DoBackupAsync();

        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetRecords(Guid id)
    {
        try
        {
            return Ok(deviceRepo.GetRecords(id));
        }
        catch (KeyNotFoundException ex)
        {
            return DeviceNotFoundError(ex);
        }
    }

    [HttpGet("data/{id}")]
    public IActionResult GetStatistcsData(Guid id)
    {
        List<StatisticsRecord> recs;
        try
        {
            recs = deviceRepo.GetRecords(id);
        }
        catch (KeyNotFoundException ex)
        {
            return DeviceNotFoundError(ex);
        }

        double time = 0;
        HashSet<string> names = new();
        HashSet<string> vers = new();
        string name = string.Empty;
        string version = string.Empty;
        for (int i = 0; i < recs.Count; i++)
        {
            if (i == recs.Count - 1)
            {
                name = recs[i].Name;
                version = recs[i].Version;
            }

            time += (recs[i].EndTime - recs[i].StartTime).TotalSeconds;
            names.Add(recs[i].Name);
            vers.Add(recs[i].Version);
        }

        return Ok(new StatisticsData(time / recs.Count, names.ToList(), vers.ToList(), name, version));
    }

    [HttpPost]
    public IActionResult AddRecord([FromBody] StatisticsRecordDTO data)
    {
        deviceRepo.Add(new StatisticsRecord(data));

        return Ok();
    }

    [HttpDelete("{deviceId}&&{recordId}")]
    public IActionResult DeleteRecord(Guid deviceId, Guid recordId)
    {
        try
        {
            deviceRepo.DeleteRecord(deviceId, recordId);
        }
        catch (KeyNotFoundException ex)
        {
            return DeviceNotFoundError(ex);
        }
        catch (InvalidOperationException ex)
        {
            return RecordNotFoundError(ex);
        }

        return Ok();
    }


    private IActionResult DeviceNotFoundError(Exception ex)
    {
        logger.LogError($"Device not found: {ex.Message}");
        return StatusCode(400, "Incorrect ID, device not found.");
    }
    private IActionResult RecordNotFoundError(Exception ex)
    {
        logger.LogError($"Record not found: {ex.Message}");
        return StatusCode(400, "Incorrect ID, record not found.");
    }
}