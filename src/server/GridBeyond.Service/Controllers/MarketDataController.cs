using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GridBeyond.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly IMarketDataService _service;

        public MarketDataController(IMarketDataService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<DataModel>> GetMarketData()
        {
            return await _service.GetAllData();
        }
        
        [HttpGet("latest")]
        public async Task<IEnumerable<DataModel>> GetLatestMarketData()
        {
            return await _service.GetLatest();
        }

        [HttpPost]
        public async Task<ActionResult> InsertRecords([FromBody] List<string> csv)
        {
            var result = await _service.ValidData(csv);
            if (result.ValidRecord.Any())
            {
                var newRecords = await _service.InsertMultiple(result.ValidRecord);
                return Ok(new
                {
                    ValidRecords = result.ValidRecord, InvalidRecords = result.MalformedRecordLine,
                    NewRecords = newRecords
                });
            }
            else
            {
                return BadRequest("Couldn't process the records.");
            }
        }

        [HttpGet("Report")]
        public async Task<ReportData> GenerateHistoryReport()
        {
            return await _service.GetReportDataHistory();
        }
    }
}