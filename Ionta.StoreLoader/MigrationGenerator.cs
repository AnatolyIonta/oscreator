using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Ionta.OSC.ToolKit.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;

namespace Ionta.StoreLoader
{
    internal class ColumnInfo
    {
        public string Name;
        public ColumnType Type;
    }

    internal enum ColumnType
    {
        Int,
        Long,
        Float,
        Decimal,
        Double,
        String,
        Boolean,
        Guid,
        None
    }

    public class MigrationGenerator
    {
        private readonly IAssemblyManager _assemblyManager;
        private readonly IConfiguration _configuration;
        public MigrationGenerator(IAssemblyManager assemblyManager, IConfiguration configuration)
        {
            _assemblyManager = assemblyManager;
            _configuration = configuration;
        }

        private void TableComparison()
        {
            var modelsTable = _assemblyManager.GetEntities();

            foreach(var table in modelsTable)
            {
                var columnInfoFromModel = GetTableFromModel(table);
                var columnInfoFromDatabase = GetTableInfo(table.Name);

                if(columnInfoFromDatabase.Count() == 0)
                {

                }
            }
        }

        private void CreateTable(string name, ColumnInfo[] columnsInfo)
        {
            using var conn = new NpgsqlConnection(GetDatabaseConnectionString(_configuration));
            try
            {
                var result = new StringBuilder();
                result.Append($"CREATE TABLE {name} (");
                foreach(var columnInfo in columnsInfo)
                {

                }
                conn.Open();
                using var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = 
                };
            }
            finally
            {

            }
        }

        private IEnumerable<ColumnInfo> GetTableFromModel(Type entity)
        {
            var properties = entity.GetProperties(BindingFlags.Public);
            foreach(var property in properties)
            {
                yield return new ColumnInfo()
                {
                    Name = property.Name,
                    Type = GetType(property.PropertyType.Name)
                };
            }
        }

        private IEnumerable<ColumnInfo> GetTableInfo(string tableName)
        {
            using var conn = new NpgsqlConnection(GetDatabaseConnectionString(_configuration));
            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = "SELECT c.column_name, c.data_type "
                    + "FROM information_schema.columns c "
                    + $"where c.table_schema = 'public' and c.table_name = '{tableName}'"
                };
                var res = cmd.ExecuteReader();
                
                while (res.Read())
                {
                    yield return new ColumnInfo()
                    {
                        Name = res.GetString(0),
                        Type = GetType(res.GetString(1)),
                    };
                }

            }
            finally
            {
                conn.Close();
            }
        }

        private ColumnType GetType(string type)
        {
            if(type == "integer" && type == typeof(int).Name) return ColumnType.Int;
            if(type == "bigint" && type == typeof(long).Name) return ColumnType.Long;
            if(type == "decimal" && type == typeof(decimal).Name) return ColumnType.Decimal;
            if (type == "real" && type == typeof(float).Name) return ColumnType.Float;
            if (type == "double precision" && type == typeof(long).Name) return ColumnType.Double;
            if (type == "text" && type == typeof(string).Name) return ColumnType.String;
            if (type == "boolean" && type == typeof(bool).Name) return ColumnType.Boolean;
            if (type == "uuid" && type == typeof(Guid).Name) return ColumnType.Guid;

            return ColumnType.None;
        }

        private string GetType(ColumnType type)
        {
            if (type == ColumnType.Int) return "integer";
            if (type == ColumnType.Long) return "bigint";
            if (type == ColumnType.Decimal) return "decimal";
            if (type == ColumnType.Float) return "real";
            if (type == ColumnType.Double) return "double precision";
            if (type == ColumnType.String) return "text";
            if (type == ColumnType.Boolean) return "boolean";
            if (type == ColumnType.Guid) return "uuid";

            throw new Exception("Тип не опознан!");
        }

        private static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            return
                $"Host={configuration["DB_HOST"]};Port={configuration["DB_PORT"]};Database={configuration["DB_NAME"]};Username={configuration["DB_USER"]};Password={configuration["DB_PASSWORD"]}";
        }
    }
}