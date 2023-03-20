namespace Ionta.OSC.Core.Store.Migration
{
    internal class ColumnInfo
    {
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}