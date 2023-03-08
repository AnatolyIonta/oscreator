using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Encrypt.EncryptColumn
{
    public class EncryptionConverter : ValueConverter<string, string>
    {
        public EncryptionConverter(IConfiguration configuration, ConverterMappingHints mappingHints = null)
            : base(x => AesService.EncryptString(configuration["SecretAes"], x), 
                  x => AesService.DecryptString(configuration["SecretAes"], x),
                  mappingHints)
        {
            
        }
    }
}
