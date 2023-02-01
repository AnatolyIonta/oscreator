using Ionta.OSC.Core.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.CustomControllers.Methods
{
    internal class PostMethod : IMethod
    {
        public string Name => "post";

        public async Task<object[]> GetParametrs(RequestInfo request, MethodInfo handler)
        {
            var parameters = await GetParametrJson(request, handler);
            var result = parameters is null ? new object[] { } : new[] { parameters };
            return result;
        }

        private async Task<object> GetParametrJson(RequestInfo request, MethodInfo handler)
        {
            var parametrs = handler.GetParameters();

            if (parametrs.Length == 0) return null;
            if (parametrs.Length > 1) throw new ManyParameters();

            var parameterType = handler.GetParameters()[0].ParameterType;

            var json = await GetJson(request.Body);
            var parameter = JsonSerializer.Deserialize(json, parameterType);

            return parameter;
        }

        private async Task<string> GetJson(System.IO.Stream input)
        {
            using (StreamReader reader
                   = new StreamReader(input, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
