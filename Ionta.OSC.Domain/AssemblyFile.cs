namespace Ionta.OSC.Domain
{
    public class AssemblyFile : BaseEntity
    {
        public string AssemblyName { get; set; }
        public byte[] Data { get; set; }
        public bool IsActive { get; set; }
    }
}