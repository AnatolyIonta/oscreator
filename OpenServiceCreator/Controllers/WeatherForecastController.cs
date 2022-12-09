using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.ToolKit.Services;
using Ionta.OSC.ToolKit.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenServiceCreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDataStore _store;
        private readonly IMigrationGenerator _migrationGenerator;
        public WeatherForecastController(IDataStore store, IMigrationGenerator migrationGenerator)
        {
            _store = store;
            _migrationGenerator = migrationGenerator;
        }

        [HttpGet]
        public string Get()
        {
            _migrationGenerator.ApplayMigrations();
            return "Ok";
        }
    }
}