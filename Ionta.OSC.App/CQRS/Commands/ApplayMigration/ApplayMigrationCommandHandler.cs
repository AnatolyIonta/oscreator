using Ionta.OSC.App.CQRS.Commands.LoadAssembly;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.Store;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.ApplayMigration
{
    public class ApplayMigrationCommandHandler : IRequestHandler<ApplayMigrationCommand, bool>
    {
        private readonly IMigrationGenerator _migrationGenerator;
        public ApplayMigrationCommandHandler(IMigrationGenerator migrationGenerator) 
        { 
            _migrationGenerator= migrationGenerator;
        }
        public Task<bool> Handle(ApplayMigrationCommand request, CancellationToken cancellationToken)
        {
            _migrationGenerator.ApplayMigrations();
            return Task.FromResult(true);
        }
    }
}
