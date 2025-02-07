# ASP.NET + angular 15 example app
Project emulates monitoring service, that can recieve JSON data from external client, store it, and display statistics data with Angular SPA.

## Technologies used:
- C#, ASP.NET Core
- Angular 15

## Backend descrition

### Database
Server stores records in-memory with optional backup to file.

### Requests
- GET:
  - /api/monitoring - returns the ids of all recorded devices
  - /api/monitoring/{id} - returns a list of data on specific device
  - /api/monitoring/data/{id} - returns a list of statistics data on the device
  - /api/monitoring/backup - tells the server to perform a backup
- POST:
  - /api/monitoring - adds record to server
- DELETE:
  - /api/monitoring/{deviceId}&&{recordId} - delete cpecific record on specific device

### JSON schemes
- ids list: 
```
    [ 
      "3fa85f64-5717-4562-b3fc-2c963f66afa6", 
      "f695ea23-8662-4a57-975a-f5afd26655db" 
    ]
```
- records on a device list:  
```
    [
      {
        "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string",
        "startTime": "2024-12-31T12:09:12.934Z",
        "endTime": "2025-01-01T12:09:12.934Z",
        "version": "1.1",
        "id": "467d1e95-c192-4b0f-96b3-d139b2109055"
      }
    ]
```
- statistics data on a device:
```
    {
      "averageSessionDurationMs": number,
      "uniqueNames": [
        "string"
      ],
      "uniqueVersions": [
        "1.1"
      ],
      "latestName": "string",
      "latestVersion": "1.1"
    }
```
- post request json scheme:
```
    {
      "_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "string",
      "startTime": "2025-02-07T13:42:24.152Z",
      "endTime": "2025-02-07T13:42:24.152Z",
      "version": "string"
    }
```
### Backup
Project uses very simple plain text backup strategy.

## Frontend
User can view and delete records with minimalistic GUI made with Angular 15.