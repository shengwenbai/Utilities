using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Utilities
{
    public class BulkInsertHelper
    {
        public static bool BulkInsert<T>(DbContext dbContext, string tableName, IList<T> listToInsert)
        {
            try
            {
                if (listToInsert == null || listToInsert.Count == 0)
                {
                    return true;
                }

                if (dbContext.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    dbContext.Database.GetDbConnection().Open();
                }

                using (var bulkCopy = new SqlBulkCopy(dbContext.Database.GetDbConnection().ConnectionString))
                {
                    bulkCopy.BatchSize = listToInsert.Count;
                    bulkCopy.DestinationTableName = tableName;

                    var table = new DataTable();
                    var props = TypeDescriptor.GetProperties(typeof(T))
                        .Cast<PropertyDescriptor>()
                        .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                        .ToArray();
                    foreach (var propertyDescriptor in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyDescriptor.Name, propertyDescriptor.Name);
                        table.Columns.Add(propertyDescriptor.Name,
                            Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ??
                            propertyDescriptor.PropertyType);
                    }

                    var values = new object[props.Length];
                    foreach (var item in listToInsert)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }

                        table.Rows.Add(values);
                    }

                    bulkCopy.WriteToServer(table);
                    if (dbContext.Database.GetDbConnection().State != ConnectionState.Closed)
                    {
                        dbContext.Database.GetDbConnection().Close();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                if (dbContext.Database.GetDbConnection().State != ConnectionState.Closed)
                {
                    dbContext.Database.GetDbConnection().Close();
                }
                return false;
            }
        }
    }
}