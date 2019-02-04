using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        [BsonElement("day")]
        public Days Day { get; set; }
        [BsonElement("Start")]
        public string Start { get; set; }
        [BsonElement("LunchStart")]
        public string LunchStart { get; set; }
        [BsonElement("LunchEnd")]
        public string LunchEnd { get; set; }
        [BsonElement("End")]
        public string End { get; set; }
    }

    public class TimesheetModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("employee")]
        public string employee { get; set; }
        [BsonElement("startPeriod")]
        public string startPeriod { get; set; }
        [BsonElement("endPeriod")]
        public string endPeriod { get; set; }
        [BsonElement("payDate")]
        public string payDate { get; set; }
        [BsonElement("timesheetTotals")]
        public List<TimesModel> timesheetTotals { get; set; }
        [BsonElement("paidBreaks")]
        public bool paidBreaks { get; set; }
        [BsonElement("signature")]
        public string signature { get; set; }
        [BsonElement("date")]
        public string date { get; set; }
    }
}
