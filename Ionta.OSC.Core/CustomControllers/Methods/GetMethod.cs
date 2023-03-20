using System.Reflection;

namespace Ionta.OSC.Core.CustomControllers.Methods
{
    internal class GetMethod : IMethod
    {
        public string Name => "get";

        public async Task<object[]> GetParametrs(RequestInfo request, MethodInfo handler)
        {
            return GetParametersUrl(request, handler).ToArray();
        }

        private IEnumerable<object> GetParametersUrl(RequestInfo request, MethodInfo handler)
        {
            var parametrs = handler.GetParameters();

            if (parametrs.Length > 0)
            {
                var query = request.Query;

                foreach (var parametr in parametrs)
                {
                    object result = query.First(q => q.Key.ToLower() == parametr.Name.ToLower()).Value.ToString();
                    if (parametr.ParameterType != typeof(string))
                    {
                        result = Convert.ChangeType(result, parametr.ParameterType);
                    }
                    yield return result;
                }
            }
        }
    }
}
