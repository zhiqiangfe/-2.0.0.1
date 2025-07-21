using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic; // Use modern collections
using System.Data;
using System.Linq;

namespace Maticsoft.DBUtility
{
    /// <summary>
    /// A modern data access helper class using Dapper.
    /// This replaces the obsolete Enterprise Library-based DbHelperSQL2.
    /// </summary>
    public static class DbHelperDapper
    {
        // Assumes you have a way to get your connection string.
        // In a real .NET 8 app, this would come from IConfiguration.
        private static readonly string connectionString = PubConstant.ConnectionString;

        private static IDbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        #region Public Methods (matching original API)

        public static int GetMaxID(string FieldName, string TableName)
        {
            string sql = $"SELECT ISNULL(MAX({FieldName}), 0) + 1 FROM {TableName}";
            using (var connection = GetOpenConnection())
            {
                // ExecuteScalar<T> gets a single value and casts it.
                return connection.ExecuteScalar<int>(sql);
            }
        }

        public static bool Exists(string sql, object? param = null)
        {
            // We expect a query like "SELECT COUNT(1) FROM Table WHERE ..."
            // or "IF EXISTS(...) SELECT 1 ELSE SELECT 0"
            string existsSql = $"IF EXISTS ({sql}) SELECT 1 ELSE SELECT 0";
            using (var connection = GetOpenConnection())
            {
                return connection.ExecuteScalar<bool>(existsSql, param);
            }
        }

        #endregion

        #region Execute Simple SQL Statements

        public static int ExecuteSql(string sql, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                // Dapper's Execute returns the number of affected rows.
                return connection.Execute(sql, param);
            }
        }

        public static int ExecuteSqlByTime(string sql, int timeout, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Execute(sql, param, commandTimeout: timeout);
            }
        }

        public static void ExecuteSqlTran(List<string> sqlList)
        {
            using (var connection = GetOpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var sql in sqlList)
                        {
                            if (!string.IsNullOrWhiteSpace(sql))
                            {
                                connection.Execute(sql, transaction: transaction);
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Execute SQL with Parameters

        public static object? GetSingle(string sql, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                // QuerySingleOrDefault returns one row or null.
                return connection.QuerySingleOrDefault<object>(sql, param);
            }
        }

        /// <summary>
        /// Executes a query and returns a generic list of results.
        /// </summary>
        public static IEnumerable<T> Query<T>(string sql, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Query<T>(sql, param);
            }
        }

        /// <summary>
        /// Executes a query and returns a DataSet.
        /// NOTE: DataSet is a legacy type. Prefer returning IEnumerable<T>.
        /// This method is provided for backward compatibility.
        /// </summary>
        public static DataSet Query(string sql, object? param = null)
        {
            var ds = new DataSet();
            // Use the full SqlClient connection for SqlDataAdapter
            using (var connection = new SqlConnection(connectionString))
            {
                using (var adapter = new SqlDataAdapter(sql, connection))
                {
                    // This is the corrected block for handling parameters
                    if (param != null)
                    {
                        // Check if the parameter object is an instance of Dapper's DynamicParameters
                        if (param is DynamicParameters dynamicParams)
                        {
                            // If it is, we can iterate through its parameter names.
                            foreach (var name in dynamicParams.ParameterNames)
                            {
                                // We need to get the value for each parameter name.
                                var value = dynamicParams.Get<object>(name);
                                adapter.SelectCommand.Parameters.Add(new SqlParameter(name, value ?? DBNull.Value));
                            }
                        }
                        else
                        {
                            // If it's not DynamicParameters, assume it's an anonymous object (POCO).
                            // We use reflection to get its properties and values.
                            adapter.SelectCommand.Parameters.AddRange(
                                param.GetType().GetProperties()
                                    .Select(p => new SqlParameter(p.Name, p.GetValue(param, null) ?? DBNull.Value))
                                    .ToArray()
                            );
                        }
                    }

                    // The adapter will open and close the connection automatically.
                    adapter.Fill(ds);
                }
            }
            return ds;
        }

        #endregion

        #region Stored Procedure Operations

        public static int RunProcedure(string storedProcName, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Execute(storedProcName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> RunProcedure<T>(string storedProcName, object? param = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Query<T>(storedProcName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// A helper to work with output parameters in stored procedures.
        /// </summary>
        public static DynamicParameters RunProcedureWithOutput(string storedProcName, DynamicParameters param)
        {
            using (var connection = GetOpenConnection())
            {
                connection.Execute(storedProcName, param, commandType: CommandType.StoredProcedure);
                return param; // The DynamicParameters object will be populated with output values.
            }
        }

        #endregion
    }
}
