namespace Ionta.OSC.App.Dtos
{
    public class DtosList<T>
    {
        public int Count { get; set; }
        public T[] Dtos { get; set; }
    }
}