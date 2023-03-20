using Ionta.OSC.App.Dtos;
using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.LogList
{
    public class LogListQueryHandler : IRequestHandler<LogListQuery, DtosList<LogDto>>
    {
        public IOscStorage _storage;
        public LogListQueryHandler(IOscStorage storage) 
        { 
            _storage = storage;
        }
        public async Task<DtosList<LogDto>> Handle(LogListQuery request, CancellationToken cancellationToken)
        {
            var data = _storage.Logs.OrderByDescending(l => l.Time).Select(l => 
            new LogDto { Message = l.Message, Date = l.Time, Type = l.Type, StackTace = l.StackTace });

            var result = new DtosList<LogDto>()
            {
                Dtos = data.ToArray(),
                Count = data.Count()
            };

            return result;
        }
    }
}