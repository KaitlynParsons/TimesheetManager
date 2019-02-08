using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Gets timesheets
        /// </summary>
        /// <returns></returns>
        public List<TimesheetModel> Get()
        {
            return _timesheet.Find(timesheet => true).ToList();
        }

        /// <summary>
        /// Gets timesheet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TimesheetModel Get(string id)
        {
            return _timesheet.Find(timesheet => timesheet.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Creates timesheet
        /// </summary>
        /// <param name="timesheet"></param>
        /// <returns></returns>
        public TimesheetModel Create([FromBody]TimesheetModel timesheet)
        {
            _timesheet.InsertOne(timesheet);
            return timesheet;
        }

        /// <summary>
        /// updates timesheet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timesheetIn"></param>
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
