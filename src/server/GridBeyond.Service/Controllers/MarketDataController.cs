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
        
        [HttpPost]
        public async Task<ActionResult> InsertRecords([FromBody]List<string> csv)
        {
            var result = await _service.ValidData(csv);
            if (result.ValidRecord.Any())
            {
                await _service.InsertMultiple(result.ValidRecord);
                return Ok();
            }
            else
            {
                return BadRequest("Couldn't process the records.");
            }
        }
    }
}