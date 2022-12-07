using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Ionta.OSC.ToolKit.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace Ionta.StoreLoader.Migration
{
    public class MigrationGenerator : IMigrationGenerator
    {
        private readonly IAssemblyManager _assemblyManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MigrationGenerator> _logger;
        public MigrationGenerator(IAssemblyManager assemblyManager, IConfiguration configuration, ILogger<MigrationGenerator> logger)
        {
            _assemblyManager = assemblyManager;
            _configuration = configuration;
            _logger = logger;
        }

        public void ApplayMigrations()
        {
            var modelsTable = _assemblyManager.GetEntities();

            foreach (var table in modelsTable)
            {
                var columnInfoFromModel = GetTableFromModel(table);
                var columnInfoFromDatabase = GetTableInfo(table.Name);

                var count = columnInfoFromDatabase.Count();

                if (count <= 0)
                {
                    CreateTable(table.Name, columnInfoFromModel.ToArray());
                }
                else
                {
                    AddNewColumnIntoTable(table.Name, columnInfoFromModel, columnInfoFromDatabase);
                }
            }
        }


        private void AddNewColumnIntoTable(string tableName, IEnumerable<ColumnInfo> columnModel, IEnumerable<ColumnInfo> columnDatabase)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append("ALTER TABLE " + tableName + '\n');
            var comumnsCount = Math.Max(columnModel.Count(), columnDatabase.Count());
            foreach (var column in columnModel)
            {
                sqlCommand.Append(ComparisonColumn(column, columnDatabase) + '\n');
            }
            var dropColumns = columnDatabase.Where(d => columnModel.All(m => m.Name.ToLower() != d.Name.ToLower()));
            foreach (var column in dropColumns)
            {
                sqlCommand.Append($"DROP COLUMN {column.Name} \n");
            }
            sqlCommand.Append(';');
            var comand = sqlCommand.ToString();
            if (!comand.Replace("\n", "").EndsWith(tableName + ";")) 
                ExecuteSqlCommand(comand);
        }

        private string ComparisonColumn(ColumnInfo columnModel, IEnumerable<ColumnInfo> columnDatabase)
        {
            var column = columnDatabase.FirstOrDefault(c => c.Name.ToLower() == columnModel.Name.ToLower());
            if (column != null)
            {
                if (column.Type == columnModel.Type) return "";
                else return $"ALTER COLUMN {columnModel.Name} {GetType(columnModel.Type)}";
            }
            return $"ADD COLUMN {columnModel.Name} {GetType(columnModel.Type)}";
        }

        private void CreateTable(string name, ColumnInfo[] columnsInfo)
        {
            using var conn = new NpgsqlConnection(GetDatabaseConnectionString(_configuration));
            try
            {
                var result = new StringBuilder();
                result.Append($"CREATE TABLE {name} ( ");
                for(var i = 0; i < columnsInfo.Length; i++)
                {
                    var column = columnsInfo[i];
                    var primaryKey = column.IsPrimaryKey ? "PRIMARY KEY" : "";
                    result.Append($"{column.Name} {GetType(column.Type)} {primaryKey}");
                    if(i < columnsInfo.Length - 1) result.Append(", ");
                }
                result.Append(')');

                conn.Open();
                using var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = result.ToString()
                };
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
        }

        private IEnumerable<ColumnInfo> GetTableFromModel(Type entity)
        {
            var properties = entity.GetProperties();
            foreach (var property in properties)
            {
                yield return new ColumnInfo()
                {
                    Name = property.Name,
                    Type = GetType(property.PropertyType.Name),
                    IsPrimaryKey = property.Name == "Id"
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

        private void ExecuteSqlCommand(string sql)
        {
            using var conn = new NpgsqlConnection(GetDatabaseConnectionString(_configuration));
            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
        }

        private ColumnType GetType(string type)
        {
            if (type == "integer" || type == typeof(int).Name) return ColumnType.Int;
            if (type == "bigint" || type == typeof(long).Name) return ColumnType.Long;
            if (type == "decimal" || type == typeof(decimal).Name) return ColumnType.Decimal;
            if (type == "real" || type == typeof(float).Name) return ColumnType.Float;
            if (type == "double precision" || type == typeof(long).Name) return ColumnType.Double;
            if (type == "text" || type == typeof(string).Name) return ColumnType.String;
            if (type == "boolean" || type == typeof(bool).Name) return ColumnType.Boolean;
            if (type == "uuid" || type == typeof(Guid).Name) return ColumnType.Guid;

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
                $"Host={configuration["DB_GLOBAL_HOST"]};Port={configuration["DB_GLOBAL_PORT"]};Database={configuration["DB_GLOBAL_NAME"]};Username={configuration["DB_GLOBAL_USER"]};Password={configuration["DB_GLOBAL_PASSWORD"]}";
        }
    }
}