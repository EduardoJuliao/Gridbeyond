using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GridBeyond.Service.Controllers
{
    [Route("api/[controller]")]
    public class ProcessHistory : Controller
    {
        private readonly IProcessHistoryService _service;

        public ProcessHistory(IProcessHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<ProcessHistoryModel>> Get()
        {
            return await _service.GetProcessHistory();
        }
    }
}
