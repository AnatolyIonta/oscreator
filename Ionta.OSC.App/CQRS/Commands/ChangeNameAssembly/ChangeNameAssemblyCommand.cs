using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class ChangeNameAssemblyCommand : IRequest<bool> 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long Id { get; set; }
    }
}