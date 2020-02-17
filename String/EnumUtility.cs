namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class EnumUtility
    {
        static Dictionary<Type, Dictionary<Int32, String>> codeDescriptionData = new Dictionary<Type, Dictionary<Int32, String>>();
        static Object lockObj = new object();

        private static Dictionary<Int32, String> AddEnum(Type tp)
        {
            Dictionary<Int32, String> enumData = new Dictionary<int, string>();
            foreach (var fieldItem in tp.GetFields())
            {
                var descList = fieldItem.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descList == null)
                {
                    continue;
                }

                try
                {
                    var val = (Int32)Enum.Parse(tp, fieldItem.Name);
                    enumData[val] = ((DescriptionAttribute)descList[0]).Description;
                }
                catch
                {
                    continue;
                }
            }

            lock (lockObj)
            {

                codeDescriptionData[tp] = enumData;
            }

            return enumData;
        }

        public static Dictionary<Int32, String> GetEnumData(Type tp)
        {
            Dictionary<Int32, String> val = null;
            lock (lockObj)
            {
                if (codeDescriptionData.TryGetValue(tp, out val))
                {
                    return val;
                }

                return AddEnum(tp);
            }
        }

        public static String GetDescription<TEnum>(Int32 code)
            where TEnum : struct
        {
            var enumData = GetEnumData(typeof(TEnum));
            if (enumData.ContainsKey(code))
            {
                return enumData[code];
            }

            return Enum.GetName(typeof(TEnum), code);
        }

        public static String GetDescription(Type tp, Int32 code)
        {
            var enumData = GetEnumData(tp);
            if (enumData.ContainsKey(code))
            {
                return enumData[code];
            }

            return Enum.GetName(tp, code);
        }
    }
}
