import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { timesheetModel } from '../../models/timesheetModel';

@Component({
  selector: 'app-timesheets',
  templateUrl: './timesheets.component.html'
})
export class TimesheetsComponent {
  public timesheets: timesheetModel[];
  public times: timesheetModel;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    
  }

  public ngOnInit(): void {
    this.http.get<timesheetModel[]>(this.baseUrl + 'api/timesheet').subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));
  }
}
