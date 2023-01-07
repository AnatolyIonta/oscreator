using Ionta.OSC.App.Dtos;
using Ionta.OSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.Auth
{
    public interface IAuthService
    {
        public Task<JWTDto> Handle(string login, string password);
    }
}
