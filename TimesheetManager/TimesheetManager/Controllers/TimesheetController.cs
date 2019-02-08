using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            _timesheetService.Create(timesheet);
            FillPdf(timesheet);

            return CreatedAtRoute(
                        routeName: "GetTimesheet",
                        routeValues: new { id = timesheet.Id },
                        value: timesheet);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult UpdateTimesheet(string id, TimesheetModel timesheetIn)
        {
            var timesheet = _timesheetService.Get(id);

            if (timesheet == null)
            {
                return NotFound();
            }

            _timesheetService.Update(id, timesheetIn);
            FillPdf(timesheetIn);

            return NoContent();
        }


        public void FillPdf(TimesheetModel timesheet)
        {   //TODO Clean this method up
            string wkStart = timesheet.startPeriod;
            string wkEnd = timesheet.endPeriod;
            string fileName = timesheet.employee.Substring(0, timesheet.employee.LastIndexOf(' ')) + "_" + wkStart.Replace("/", ".") + "_" + wkEnd.Replace("/", ".");
            string inputPath = Path.Combine(hostingEnvironment.ContentRootPath, "timesheets\\blank_timesheet-rotated.pdf");
            string outputPath = Path.Combine(hostingEnvironment.ContentRootPath, "timesheets\\" + fileName + ".pdf");

            //read the timesheet template
            PdfReader reader = new PdfReader(inputPath);

            //Work with only page 1 
            reader.SelectPages("1");
            PdfStamper stamper = new PdfStamper(reader, new FileStream(outputPath, FileMode.Create));

            // PdfContentByte from stamper to add content to the pages over the original content
            PdfContentByte pbfOver = stamper.GetOverContent(1);
            //add content to the page using ColumnText
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.employee), 150, 520, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(wkStart + " - " + wkEnd), 115, 472, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.payDate), 390, 472, 0);
            //start times
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[0].Start), 140, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[1].Start), 200, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[2].Start), 270, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[3].Start), 330, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[4].Start), 390, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[5].Start), 460, 390, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[6].Start), 540, 390, 0);
            //lunch start
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[0].LunchStart), 140, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[1].LunchStart), 200, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[2].LunchStart), 270, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[3].LunchStart), 330, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[4].LunchStart), 390, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[5].LunchStart), 460, 375, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[6].LunchStart), 540, 375, 0);
            //lunch finish
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[0].LunchEnd), 140, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[1].LunchEnd), 200, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[2].LunchEnd), 270, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[3].LunchEnd), 330, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[4].LunchEnd), 390, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[5].LunchEnd), 460, 360, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[6].LunchEnd), 540, 360, 0);
            //finish times
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[0].End), 140, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[1].End), 200, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[2].End), 270, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[3].End), 330, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[4].End), 390, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[5].End), 460, 345, 0);
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.timesheetTotals[6].End), 540, 345, 0);
            //date timesheet was filled
            ColumnText.ShowTextAligned(pbfOver, Element.ALIGN_LEFT, new Phrase(timesheet.date), 520, 74, 0);

            //The below will make sure the fields are not editable in the output PDF.
            stamper.FormFlattening = true;

            //close pdf stamper and reader
            stamper.Close();
            reader.Close();

        }
    }
}
