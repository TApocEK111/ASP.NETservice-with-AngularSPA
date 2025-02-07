import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { StatRecord } from '../model/stat-record.type';
import { StatData } from '../model/stat-data.type';

@Injectable()
export class ApiService {

    http = inject(HttpClient);
    url = 'http://localhost:5071/api/monitoring/';

    getDevicesFromApi() {
        return this.http.get<Array<string>>(this.url);
    }
    getRecordsFromApi(guid: string) {
        return this.http.get<Array<StatRecord>>(this.url + guid);
    }
    getStatDataFromApi(guid: string) {
        return this.http.get<StatData>(this.url + 'data/' + guid);
    }
    deleteRecord(deviceId: string, recordId: string) {
        return this.http.delete(this.url + deviceId + '&&' + recordId);
    }
}