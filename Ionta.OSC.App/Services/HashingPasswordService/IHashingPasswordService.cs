using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.HashingPasswordService
{
    public interface IHashingPasswordService
    {
        public string Hash(string password);
    }
}
