

namespace Polaris.Utility.Mysql
{
    using MySql.Data.MySqlClient;
    using System;

    public class FieldInfo
    {
        public String FieldName { get; set; }

        public String FieldType { get; set; }

        public Int32 DataLength { get; set; }

        public MySqlDbType DbType { get; set; }

        public FieldInfo(String fieldName, String fieldType)
        {
            this.FieldName = fieldName;
            this.FieldType = fieldType;

            fieldType = fieldType.ToLower();
            if (fieldType.StartsWith("int"))
            {
                if (fieldType.Contains("unsigned"))
                {
                    this.DbType = MySqlDbType.UInt32;
                }
                else
                {
                    this.DbType = MySqlDbType.Int32;
                }
            }
            else if (fieldType.StartsWith("tinyint"))
            {
                if (fieldType.Contains("unsigned"))
                {
                    this.DbType = MySqlDbType.UInt16;
                }
                else
                {
                    this.DbType = MySqlDbType.Int16;
                }
            }
            else if (fieldType.StartsWith("smallint"))
            {
                if (fieldType.Contains("unsigned"))
                {
                    this.DbType = MySqlDbType.UInt16;
                }
                else
                {
                    this.DbType = MySqlDbType.Int16;
                }
            }
            else if (fieldType.StartsWith("bigint"))
            {
                if (fieldType.Contains("unsigned"))
                {
                    this.DbType = MySqlDbType.UInt64;
                }
                else
                {
                    this.DbType = MySqlDbType.Int64;
                }
            }
            else if (fieldType.StartsWith("float"))
            {
                this.DbType = MySqlDbType.Float;
            }
            else if (fieldType.StartsWith("double"))
            {
                this.DbType = MySqlDbType.Double;
            }
            else if (fieldType.StartsWith("char") ||
                fieldType.StartsWith("varchar"))
            {
                this.DbType = MySqlDbType.VarChar;
            }
            else if (fieldType.StartsWith("bool"))
            {
                this.DbType = MySqlDbType.Int16;
            }
            else if (fieldType.StartsWith("bit"))
            {
                this.DbType = MySqlDbType.Bit;
            }
            else if (fieldType.StartsWith("blob"))
            {
                this.DbType = MySqlDbType.Blob;
            }
            else if (fieldType.StartsWith("datetime"))
            {
                this.DbType = MySqlDbType.DateTime;
            }
            else if (fieldType.StartsWith("date"))
            {
                this.DbType = MySqlDbType.Date;
            }
            else if (fieldType.StartsWith("timestamp"))
            {
                this.DbType = MySqlDbType.Timestamp;
            }
            else if (fieldType.StartsWith("text"))
            {
                this.DbType = MySqlDbType.Text;
            }
            else if (fieldType.StartsWith("longtext"))
            {
                this.DbType = MySqlDbType.LongText;
            }
            else if (fieldType.StartsWith("mediumtext"))
            {
                this.DbType = MySqlDbType.MediumText;
            }
            else if (fieldType.StartsWith("tinytext"))
            {
                this.DbType = MySqlDbType.TinyText;
            }
            else
            {
                this.DbType = MySqlDbType.VarChar;
            }
        }

        public MySqlDbType GetDbType()
        {
            return this.DbType;
        }
    }
}
