using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ionta.OSC.ToolKit.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenServiceCreator.Tests;

namespace OpenServiceCreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDataStore _store;
        public WeatherForecastController(IDataStore store)
        {
            _store = store;
        }

        [HttpGet]
        public string Get()
        {
            var x = _store.GetEntity(typeof(Data551));
            return "Hello, world" + x.Count();
        }
    }
}