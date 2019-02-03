using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimesheetManager.Models;
using TimesheetManager.Services;

namespace TimesheetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly TimesheetService _timesheetService;

        public TimesheetController(TimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpGet]
        public ActionResult<List<TimesheetModel>> Get()
        {
            return _timesheetService.Get();
        }

        [HttpPost]
        public ActionResult<TimesheetModel> Create(TimesheetModel timesheet)
        {
            _timesheetService.Create(timesheet);

            return CreatedAtRoute("GetTimesheets", new { id = timesheet.Id.ToString() }, timesheet);
        }


        //private readonly TimesheetContext _context;

        //public TimesheetController(TimesheetContext context)
        //{
        //    _context = context;
        //}

        //list all 
        // GET: api/Timesheet
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TimesheetModel>>> GetTimesheets()
        //{
        //    return await _context.TimesheetsModel.Include(time => time.timesheetTotals).ToListAsync();
        //}

        ////list one
        //// GET: api/Timesheet/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TimesheetModel>> GetTimesheet(long id)
        //{
        //    var Timesheet =  await _context.TimesheetsModel.Include(time => time.timesheetTotals)
        //    .SingleAsync(timesheet => timesheet.Id == id);

        //    if (Timesheet == null)
        //    {
        //        return NotFound();
        //    }

        //    return Timesheet;
        //}

        ////create
        //// POST: api/Timesheet
        //[HttpPost]
        //public async Task<ActionResult<TimesheetModel>> PostTimesheet(TimesheetModel timesheet)
        //{
        //    _context.TimesheetsModel.Add(timesheet);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetTimesheets), new { id = timesheet.Id }, timesheet);
        //}

        ////update
        //// PUT: api/Timesheet/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTimesheet(long id, TimesheetModel timesheet)
        //{
        //    if (id != timesheet.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(timesheet).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //// DELETE: api/Timesheet/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTodoItem(long id)
        //{
        //    var timesheet = await _context.TimesheetsModel.FindAsync(id);

        //    if (timesheet == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TimesheetsModel.Remove(timesheet);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
