using System;
using GridBeyond.Domain.Interfaces.Services;
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
    }
}