namespace Polaris.Utility.Mysql
{
    using System;
    using System.Collections.Generic;

    public class MysqlTableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public String TableName { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<FieldInfo> FieldList { get; set; }
    }
}
