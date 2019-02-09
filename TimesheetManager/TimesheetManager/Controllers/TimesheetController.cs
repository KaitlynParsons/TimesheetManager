using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using TimesheetManager.Models;
using TimesheetManager.Services;

namespace TimesheetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly TimesheetService _timesheetService;
        private readonly IHostingEnvironment hostingEnvironment;

        public TimesheetController(TimesheetService timesheetService, IHostingEnvironment hostingEnvironment)
        {
            _timesheetService = timesheetService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult<List<TimesheetModel>> GetTimesheets()
        {
            return _timesheetService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetTimesheet")]
        public ActionResult<TimesheetModel> GetTimesheet(string id)
        {
            var timesheet = _timesheetService.Get(id);

            if (timesheet == null)
            {
                return NotFound();
            }

            return timesheet;
        }

        [HttpPost]
        public ActionResult<TimesheetModel> PostTimesheet(TimesheetModel timesheet)
        {
            string fileName = timesheet.employee.Substring(0, timesheet.employee.LastIndexOf(' ')) + "_" + 
                timesheet.startPeriod.Replace("/", ".") + "_" + timesheet.endPeriod.Replace("/", ".");
            string outputPath = Path.Combine(hostingEnvironment.ContentRootPath, "timesheets\\" + fileName + ".pdf");
            if (System.IO.File.Exists(outputPath))
            {
                return BadRequest("File already exists");
            }
            else
            {
                _timesheetService.Create(timesheet);
                FillPdf(timesheet, outputPath);

                return CreatedAtRoute(
                            routeName: "GetTimesheet",
                            routeValues: new { id = timesheet.Id },
                            value: timesheet);
            }
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult UpdateTimesheet(string id, TimesheetModel timesheetIn)
        {
            string fileName = timesheetIn.employee.Substring(0, timesheetIn.employee.LastIndexOf(' ')) + "_" + 
                timesheetIn.startPeriod.Replace("/", ".") + "_" + timesheetIn.endPeriod.Replace("/", ".");
            string outputPath = Path.Combine(hostingEnvironment.ContentRootPath, "timesheets\\" + fileName + ".pdf");
            var timesheet = _timesheetService.Get(id);

            if (timesheet == null)
            {
                return NotFound();
            }

            _timesheetService.Update(id, timesheetIn);
            FillPdf(timesheetIn, outputPath);

            return NoContent();
        }

        /// <summary>
        /// Fills the timesheet template with necessary values
        /// </summary>
        /// <param name="timesheet"></param>
        /// <param name="outputPath"></param>
        public void FillPdf(TimesheetModel timesheet, string outputPath)
        {
            string inputPath = Path.Combine(hostingEnvironment.ContentRootPath, "timesheets\\blank_timesheet-rotated.pdf");
            //initialise x locations on timesheet template
            int[] x = { 140, 200, 270, 330, 390, 460, 540 };
            int count = 0;
            // read the timesheet template
            PdfReader reader = new PdfReader(inputPath);
            // Work with only page 1 
            reader.SelectPages("1");
            // stamper to fill the timesheet template
            PdfStamper stamper = new PdfStamper(reader, new FileStream(outputPath, FileMode.Create));
            // PdfContentByte from stamper to add content to the pages over the original content
            PdfContentByte pbfOver = stamper.GetOverContent(1);
            // add content to the page using ColumnText
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.employee), 150, 520, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.startPeriod + " - " + timesheet.endPeriod), 115, 472, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.payDate), 390, 472, 0);
            // iterate through the x locations on the timesheet template and add each time value to its given position
            foreach (int num in x)
            {
                ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[count].Start), num, 390, 0);
                ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[count].LunchStart), num, 375, 0);
                ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[count].LunchEnd), num, 360, 0);
                ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[count++].End), num, 345, 0);
            }
            // date timesheet was filled
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.date), 520, 74, 0);
            // The below will make sure the fields are not editable in the output PDF.
            stamper.FormFlattening = true;
            // close pdf stamper and reader
            stamper.Close();
            reader.Close();
        }
    }
}
