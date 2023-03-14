using Ionta.OSC.App.Dtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ionta.OSC.Core.AssembliesInformation
{
    public class GetAssemblyQuery : IRequest<AssemblyDto>
    {
        [Required]
        public long Id { get; set; }
    }
}