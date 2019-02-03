using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetManager.Models;

namespace TimesheetManager.Services
{
    public class TimesheetService
    {
        private readonly IMongoCollection<TimesheetModel> _timesheet;

        public TimesheetService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TimesheetManager"));
            var database = client.GetDatabase("TimesheetManager");
            _timesheet = database.GetCollection<TimesheetModel>("timesheets");
        }

        public List<TimesheetModel> Get()
        {
            return _timesheet.Find(timesheet => true).ToList();
        }

        public TimesheetModel Get(string id)
        {
            return _timesheet.Find<TimesheetModel>(timesheet => timesheet.Id == id).FirstOrDefault();
        }

        public TimesheetModel Create(TimesheetModel timesheet)
        {
            _timesheet.InsertOne(timesheet);
            return timesheet;
        }

        public void Update(string id, TimesheetModel timesheetIn)
        {
            _timesheet.ReplaceOne(timesheet => timesheet.Id == id, timesheetIn);
        }

        public void Remove(TimesheetModel timesheetIn)
        {
            _timesheet.DeleteOne(timesheet => timesheet.Id == timesheetIn.Id);
        }

        public void Remove(string id)
        {
            _timesheet.DeleteOne(timesheet => timesheet.Id == id);
        }
    }
}
