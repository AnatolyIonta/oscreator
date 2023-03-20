namespace Ionta.OSC.Core.Exeption
{
    public class ManyParameters : Exception
    {
        public ManyParameters() : base("Too many parameters") { }
    }
}