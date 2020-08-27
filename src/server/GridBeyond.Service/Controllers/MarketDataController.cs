using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GridBeyond.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly IMarketDataService _service;
        private readonly IProcessHistoryService _processHistoryService;

        public MarketDataController(IMarketDataService service,
            IProcessHistoryService processHistoryService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _processHistoryService = processHistoryService ?? throw new ArgumentNullException(nameof(processHistoryService));
        }

        [HttpGet]
        public async Task<IEnumerable<DataModel>> GetMarketData()
        {
            return await _service.GetAllData();
        }
        
        [HttpGet("Latest/{recordsCount:min(1):max(50):int?}")]
        public async Task<IEnumerable<DataModel>> GetLatestMarketData(int? recordsCount)
        {
            if (!recordsCount.HasValue) recordsCount = 50;
            return await _service.GetLatest(recordsCount.Value);
        }

        [HttpGet("Report/{start:datetime?}/{end:datetime?}")]
        public async Task<ReportData> GenerateHistoryReport(DateTime? start = null, DateTime? end = null)
        {
            return await _service.GetReport(start, end);
        }

        [HttpPost]
        public async Task<ActionResult> InsertRecords([FromBody] List<string> csv)
        {
            var result = await _service.ValidData(csv);
            if (result.ValidRecord.Any())
            {
                var newRecords = await _service.InsertMultiple(result.ValidRecord);
                await _processHistoryService.SaveProcess(result.ValidRecord.Count,
                    result.MalformedRecordLine.Count, newRecords.Count(),
                    csv.Count);
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
    }
}