using Ionta.OSC.App.CQRS.Commands.ActivateAssembly;
using Ionta.OSC.App.CQRS.Commands.ApplayMigration;
using Ionta.OSC.App.CQRS.Commands.Auth;
using Ionta.OSC.App.CQRS.Commands.DeleteModul;
using Ionta.OSC.App.CQRS.Commands.LoadAssembly;
using Ionta.OSC.App.CQRS.Queries.GetAsembliesInfo;
using Ionta.OSC.App.CQRS.Queries.GetAssemblies;
using Ionta.OSC.App.Dtos;
using Ionta.OSC.Core.AssembliesInformation.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenServiceCreator.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssemblyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("SaveAssembly")]
        public Task<bool> SaveAssembly([FromForm(Name = "file")] IFormFile uploadedFile)
        {
            var file = GetBytes(uploadedFile.OpenReadStream());
            var query = new LoadAssemblyCommand() {Name = uploadedFile.FileName, Data = file };
            return _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("SetActive")]
        public Task<bool> SetActive([FromBody] ActivateAssemblyCommand command)
        {
            return _mediator.Send(command);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("ApplayMigration")]
        public Task<bool> ApplayMigration([FromBody] ApplayMigrationCommand command)
        {
            return _mediator.Send(command);
        }

        [ProducesResponseType(typeof(DtosList<AssemblyDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("list")]
        public async Task<DtosList<AssemblyDto>> List([FromBody] GetAssembliesQuery query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("delete")]
        public async Task<bool> Delete([FromBody] DeleteModulCommand query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("info")]
        public async Task<ControllerDto[]> GetInfo([FromBody] GetAsembliesInfoQuery query)
        {
            return await _mediator.Send(query);
        }

        private byte[] GetBytes(Stream s)
        {
            s.Position = 0;

            // Now read s into a byte buffer with a little padding.
            byte[] bytes = new byte[s.Length + 10];
            int numBytesToRead = (int)s.Length;
            int numBytesRead = 0;
            do
            {
                // Read may return anything from 0 to 10.
                int n = s.Read(bytes, numBytesRead, 10);
                numBytesRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);

            return bytes;
        }

    }
}
