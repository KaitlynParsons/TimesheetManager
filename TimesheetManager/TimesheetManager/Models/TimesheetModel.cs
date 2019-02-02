using System.Collections.Generic;

namespace TimesheetManager.Models
{
    public enum Days
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    public class TimesModel
    {
        public long Id { get; set; }
        public Days Day { get; set; }
        public string Start { get; set; }
        public string LunchStart { get; set; }
        public string LunchEnd { get; set; }
        public string End { get; set; }
    }

    public class TimesheetModel
    {
        public long Id { get; set; }
        public string employee { get; set; }
        public string startPeriod { get; set; }
        public string endPeriod { get; set; }
        public string payDate { get; set; }
        public List<TimesModel> timesheetTotals { get; set; }
        public bool paidBreaks { get; set; }
        public string signature { get; set; }
        public string date { get; set; }
    }
}
