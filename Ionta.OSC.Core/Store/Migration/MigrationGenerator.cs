﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Ionta.OSC.Core.Assemblys;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ionta.OSC.ToolKit.Store;

namespace Ionta.OSC.Core.Store.Migration
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

            if (modelsTable == null) return;

            foreach (var table in modelsTable)
            {
                var tableName = table.Name.ToLower();
                var columnInfoFromModel = GetTableFromModel(table);
                var columnInfoFromDatabase = GetTableInfo(tableName);

                var count = columnInfoFromDatabase.Count();

                if (count <= 0)
                {
                    CreateTable(tableName, columnInfoFromModel.ToArray());
                }
                else
                {
                    AddNewColumnIntoTable(tableName, columnInfoFromModel, columnInfoFromDatabase);
                }
                var links = GetLiks(table);
                CreateForeignKey(links);
            }
        }


        private void AddNewColumnIntoTable(string tableName, IEnumerable<ColumnInfo> columnModel, IEnumerable<ColumnInfo> columnDatabase)
        {
            /*
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
                sqlCommand.Append($"DROP COLUMN \"{column.Name}\" \n");
            }
            sqlCommand.Append(';');
            var comand = sqlCommand.ToString();
            if (!comand.Replace("\n", "").EndsWith(tableName + ";")) 
                ExecuteSqlCommand(comand);
            */
            var sqlCommand = new StringBuilder();

            // Combine the two parts of the query into a single statement
            var alterTable = columnModel.Select(m => ComparisonColumn(m, columnDatabase));
            var dropColumns = columnDatabase.Where(d => columnModel.All(m => m.Name.ToLower() != d.Name.ToLower())).Select(c => $"DROP COLUMN \"{c.Name}\"");
            var combined = alterTable.Concat(dropColumns);

            // Use string interpolation to simplify 
            sqlCommand.Append($"ALTER TABLE {tableName}\n{string.Join("\n", combined)};");

            // Execute the SQL command 
            ExecuteSqlCommand(sqlCommand.ToString());
        }

        private string ComparisonColumn(ColumnInfo columnModel, IEnumerable<ColumnInfo> columnDatabase)
        {
            var column = columnDatabase.FirstOrDefault(c => c.Name.ToLower() == columnModel.Name.ToLower());
            if (column != null)
            {
                if (column.Type == columnModel.Type) return "";
                else return $"ALTER COLUMN \"{columnModel.Name}\" {GetType(columnModel.Type)}";
            }
            return $"ADD COLUMN \"{columnModel.Name}\" {GetType(columnModel.Type)}";
        }

        private void CreateTable(string name, ColumnInfo[] columnsInfo)
        {
            /*
            var result = new StringBuilder();
            result.Append($"CREATE TABLE {name} ( ");
            for (var i = 0; i < columnsInfo.Length; i++)
            {
                var column = columnsInfo[i];
                var primaryKey = column.IsPrimaryKey ? "PRIMARY KEY generated by default as identity" : "";
                result.Append($" \"{column.Name}\" {GetType(column.Type)} {primaryKey}");
                if (i < columnsInfo.Length - 1) result.Append(", ");
            }
            result.Append(')');
            */

            var result = new StringBuilder();
            result.Append($"CREATE TABLE {name} ( ");
            foreach (var column in columnsInfo)
            {
                if (column.Type == ColumnType.None) continue;
                var primaryKey = column.IsPrimaryKey ? "PRIMARY KEY generated by default as identity" : "";
                result.Append($" \"{column.Name}\" {GetType(column.Type)} {primaryKey}, ");
            }
            result.Length -= 2;
            result.Append(')');

            ExecuteSqlCommand(result.ToString());
        }

        private IEnumerable<ColumnInfo> GetTableFromModel(Type entity)
        {
            var properties = entity.GetProperties();
            foreach (var property in properties)
            {
                var typeName = property.PropertyType.Name;
                yield return new ColumnInfo()
                {
                    Name = property.Name,
                    Type = GetType(typeName),
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

        private IEnumerable<DataLink> GetLiks(Type table)
        {
            var properties = table.GetProperties();
            foreach(var property in properties)
            {
                var result = FindLink(table, property);
                if(result != null) yield return result;
            }
        }

        private DataLink FindLink(Type current, PropertyInfo property)
        {
            /*
            if (property.PropertyType.IsClass)
            {
                if (property.PropertyType.IsEnum || property.PropertyType.IsArray)
                {
                    var link = property.PropertyType.GetProperties()
                        .FirstOrDefault(e => e.PropertyType == current);
                    if (link != null)
                    {
                        if (link.PropertyType.IsArray || link.PropertyType.IsEnum)
                            return new DataLink()
                            {
                                Name = property.Name,
                                ModelFirst = current,
                                Type = DataLinkType.ManyToMany,
                                ModelSecond = property.PropertyType
                            };
                    }
                    else
                    {
                        return new DataLink()
                        {
                            Name = property.Name,
                            ModelFirst = current,
                            Type = DataLinkType.OneToMany,
                            ModelSecond = property.PropertyType
                        };
                    }
                }
                else
                {
                    var link = property.PropertyType.GetProperties()
                        .FirstOrDefault(e => e.PropertyType == current);
                    if (link != null)
                    {
                        return new DataLink()
                        {
                            Name = property.Name,
                            ModelFirst = current,
                            Type = DataLinkType.OneToOne,
                            ModelSecond = property.PropertyType
                        };
                    }
                }
                
            }
            */
            if (!typeof(BaseEntity).IsAssignableFrom(property.PropertyType) && !property.PropertyType.IsArray
                || property.PropertyType.IsArray && !typeof(BaseEntity).IsAssignableFrom(property.PropertyType.GetElementType())) return null;
            var link = property.PropertyType.GetProperties()
    .FirstOrDefault(e => current.IsAssignableFrom(e.PropertyType));

            var dataLink = new DataLink();
            dataLink.Name = property.Name;
            dataLink.ModelFirst = current;

            dataLink.Type = link != null
                ? (property.PropertyType.IsArray || property.PropertyType.IsEnum
                    ? DataLinkType.ManyToMany
                    : DataLinkType.OneToOne)
                : DataLinkType.OneToMany;

            dataLink.ModelSecond = property.PropertyType;

            return dataLink;
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
            if (type == "bytea" || type == typeof(byte[]).Name) return ColumnType.Data;

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
            if(type == ColumnType.Data) return "bytea";

            throw new Exception("Тип не опознан!");
        }

        private static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            return
                $"Host={configuration["DB_GLOBAL_HOST"]};Port={configuration["DB_GLOBAL_PORT"]};Database={configuration["DB_GLOBAL_NAME"]};Username={configuration["DB_GLOBAL_USER"]};Password={configuration["DB_GLOBAL_PASSWORD"]}";
        }
      
        private void CreateForeignKey(IEnumerable<DataLink> links)
        {
            var result = new StringBuilder();
            foreach (DataLink link in links)
            {
                var tableFirts = link.ModelFirst.Name.ToLower();
                var tableSecond = link.ModelSecond.GetElementType()?.Name.ToLower() ?? link.ModelSecond.Name.ToLower();
                if (link.Type == DataLinkType.OneToOne)
                {
                    result.AppendLine($"ALTER TABLE \"{tableFirts} \"" +
                        $"ADD FOREIGN KEY (\"{link.Name}\") " +
                        $"REFERENCES \"{tableSecond}\"(\"Id\");");
                }
                else if (link.Type == DataLinkType.OneToMany)
                {
                    result.AppendLine($"ALTER TABLE \"{tableFirts}\" " +
                        $"ADD FOREIGN KEY (\"{link.Name}\") REFERENCES \"{tableSecond}\"(\"Id\") ON DELETE CASCADE;");
                }
                else if (link.Type == DataLinkType.ManyToMany)
                {
                    result.AppendLine($"CREATE TABLE \"{tableFirts}and{tableSecond}\" ( " +
                        $"\"{tableFirts}Id\" INT NOT NULL, " +
                        $"\"{tableSecond}Id\" INT NOT NULL," +
                        $"PRIMARY KEY (\"{tableFirts}Id\", \"{tableSecond}Id)\"," +
                        $"FOREIGN KEY (\"{tableFirts}Id\") REFERENCES \"{tableFirts}\"(\"Id\"), " +
                        $"FOREIGN KEY (\"{tableSecond}Id\") REFERENCES \"{tableSecond}\"(\"Id\")" +
                        $");");
                }
            }
            ExecuteSqlCommand( result.ToString() );
        }
    }
    public enum DataLinkType
    {
        ManyToMany,
        OneToMany,
        OneToOne
    }
    public class DataLink
    {
        public string Name { get; set; }
        public Type ModelFirst { get; set; }
        public DataLinkType Type { get; set; }
        public Type ModelSecond { get; set; }
    }
}