using Microsoft.Extensions.Primitives;

namespace Ionta.OSC.Core.CustomControllers
{
    public record RequestInfo
    {
        public string Path { get; init; }
        public string Method { get; init; }
        public Stream Body { get; init; }
        public IEnumerable<KeyValuePair<string, StringValues>> Query { get; init; }

        public RequestInfo(string path, string method, Stream body, IEnumerable<KeyValuePair<string, StringValues>> query)
        {
            Path = path;
            Method = method;
            Body = body;
            Query = query;
        }
    }
}
