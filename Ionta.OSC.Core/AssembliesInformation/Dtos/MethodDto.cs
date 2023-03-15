namespace Ionta.OSC.Core.AssembliesInformation.Dtos
{
    public class MethodDto
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public Dictionary<string, object> Parameters { get;set; }
    }
}