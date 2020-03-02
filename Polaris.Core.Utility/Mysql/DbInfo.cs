namespace Polaris.Utility.Mysql
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public class DbInfo
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        public String DbName { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public String ConnectionString { get; private set; }

        /// <summary>
        /// 表数据
        /// </summary>
        public Dictionary<String, TableInfo> TableData { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbInfo(String dbName, String connectionString, Dictionary<String, TableInfo> tableData)
        {
            this.DbName = dbName;
            this.ConnectionString = connectionString;
            this.TableData = tableData;
        }

        public TableInfo GetTable(String tableName, Boolean ifTriggerException = true)
        {
            if (this.TableData.ContainsKey(tableName) == false)
            {
                if (ifTriggerException)
                {
                    throw new Exception($"没有找到表 {tableName}");
                }

                return null;
            }

            return this.TableData[tableName];
        }

        public static DbInfo GetDbInfo(String connectionString)
        {
            Dictionary<String, TableInfo> tableData = new Dictionary<string, TableInfo>();

            var dbName = "";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var tableList = GetAllTableName(conn);
                foreach (var tableName in tableList)
                {
                    var fieldList = GetColList(conn, tableName);
                    tableData[tableName] = new TableInfo() { TableName = tableName, FieldList = fieldList };
                }
                dbName = conn.Database;
            }

            return new DbInfo(dbName, connectionString, tableData);
        }

        private static List<String> GetAllTableName(MySqlConnection connObj)
        {
            var result = new List<String>();
            string sql = "show tables;";
            using (MySqlCommand cmd = new MySqlCommand(sql, connObj))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string t = reader.GetString(0);
                        result.Add(t);
                    }
                }
                reader.Close();
            }

            return result;
        }

        private static List<FieldInfo> GetColList(MySqlConnection connObj, String tableName)
        {
            var result = new List<FieldInfo>();
            MySqlDataReader reader = null;
            string sql = "show columns from `" + tableName + "`;";
            using (var cmd = new MySqlCommand(sql, connObj))
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string fieldName = reader.GetString(0);
                        Type fieldType = reader.GetValue(1) as Type;

                        string fieldTypeString = reader.GetString(1);
                        result.Add(new FieldInfo(fieldName, fieldTypeString));
                    }
                }

                reader.Close();
            }

            return result;
        }

        public static DataTable GetSelectData(String connectionString, String sql)
        {
            DataSet dataset = new DataSet();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataset);

                        if (dataset.Tables.Count <= 0)
                        {
                            return new DataTable();
                        }

                        return dataset.Tables[0];
                    }
                }
            }
        }

        public static Int64 GetInt64Val(String connectionString, String sql)
        {
            return GetValue<Int64>(connectionString, sql, Int64.Parse);
        }

        public static UInt64 GetUInt64Val(String connectionString, String sql)
        {
            return GetValue<UInt64>(connectionString, sql, UInt64.Parse);
        }

        public static T GetValue<T>(String connectionString, String sql, Func<String, T> funcObj)
        {
            var dt = GetSelectData(connectionString, sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return default(T);
            }

            var val = dt.Rows[0][0];
            if (val is DBNull)
            {
                return default(T);
            }

            return funcObj(dt.Rows[0][0].ToString());
        }

        public static List<T> GetList<T>(String connectionString, String sql, Func<String, T> funcObj)
        {
            var dt = GetSelectData(connectionString, sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return new List<T>();
            }

            var result = new List<T>();

            foreach (DataRow item in dt.Rows)
            {
                if (item[0] == null || item[0] == DBNull.Value)
                {
                    continue;
                }

                result.Add(funcObj(item[0].ToString()));
            }

            return result;
        }
    }
}