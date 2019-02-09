enum Days {
  Monday,
  Tuesday,
  Wednesday,
  Thursday,
  Friday,
  Saturday,
  Sunday
}

export class TimesModel {
  day: Days;
  Start: string;
  LunchStart: string;
  LunchEnd: string;
  End: string;
}

export class timesheetModel {
  employee: string;
  startPeriod: Date;
  endPeriod: Date;
  payDate: Date;
  timesheetTotals: Array<TimesModel>;
  paidBreaks: boolean;
  signature: string;
  date: Date;
}
