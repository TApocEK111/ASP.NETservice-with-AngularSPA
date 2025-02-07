import { Component, inject } from '@angular/core';
import { catchError } from 'rxjs';
import { ApiService } from './services/api.service';
import { StatRecord } from './model/stat-record.type';
import { StatData } from './model/stat-data.type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
  providers: [ApiService]
})
export class AppComponent {
  devices: Array<string> = [];
  api = inject(ApiService);
  showDevicesList = true;
  records: Array<StatRecord> = [];
  data: StatData | undefined;

    onDeviceClick(guid: string) {
      this.updateRecord(guid);
      this.showDevicesList = false;
    }

    onDeleteButtonClick(deviceId: string, recordId: string) {
      if (confirm('Удалить?')) {
        this.api.deleteRecord(deviceId, recordId)
        .pipe(
        catchError(err => {
          console.log(err);
          throw err;
        })
        )
        .subscribe();
        if (this.records.length > 1) {
          this.updateRecord(deviceId)
          this.rerenderRecords();
        }
        else 
        this.onBackButtonClick();
      }
    }

    onBackButtonClick() {
      this.ngOnInit();
      this.showDevicesList = true;
    }

    ngOnInit(): void {
      this.api.getDevicesFromApi()
      .pipe(
        catchError(err => {
          console.log(err);
          throw err;
        })
      ).subscribe(
        ds => this.devices = ds
      )
    }

    rerenderRecords() {
      setTimeout(() => this.showDevicesList = true, 0),
      setTimeout(() => this.showDevicesList = false, 0)
    }

    updateRecord(guid: string) {
      this.api.getRecordsFromApi(guid)
      .pipe(
        catchError(err => {
          console.log(err);
          throw err;
        })
      ).subscribe(
        r => this.records = r
      )
        
      this.api.getStatDataFromApi(guid)
      .pipe(
        catchError(err => {
          console.log(err);
          throw err;
        })
      ).subscribe(
        d => this.data = d
      )
    }
}