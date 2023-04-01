using static Ionta.Utilities.FileUtilite;

using Ionta.OSC.App.CQRS.Commands;
using Ionta.OSC.App.CQRS.Queries;
using Ionta.OSC.App.Dtos;
using Ionta.OSC.Core.AssembliesInformation;
using Ionta.OSC.Core.AssembliesInformation.Dtos;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> SaveAssembly([FromForm(Name = "file")] IFormFile uploadedFile)
        {
            var file = GetBytes(uploadedFile.OpenReadStream());
            var query = new LoadAssemblyCommand() {Name = uploadedFile.FileName, Data = file };
            var result = await _mediator.Send(query);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
            
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
        public async Task<AssemblyInfoDto> GetInfo([FromBody] GetAsembliesInfoQuery query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("changeName")]
        public async Task<bool> ChangeName([FromBody] ChangeNameAssemblyCommand query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("infoCurrentAssembly")]
        public async Task<AssemblyDto> GetInfoCurrentAssembly([FromBody] GetAssemblyQuery query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("refreshModul/{id:int}")]
        public async Task<bool> RefreshModul(
            [FromForm(Name = "file")] IFormFile uploadedFile,
            [FromRoute] int id)
        {
            var file = GetBytes(uploadedFile.OpenReadStream());
            var query = new RefreshModulCommand() 
            { Name = uploadedFile.FileName, 
              Data = file,
              Id = id
            };

            return await _mediator.Send(query);
        }

    }
}