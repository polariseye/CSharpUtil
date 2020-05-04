namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class EnumUtility
    {
        static Dictionary<Type, Dictionary<Int32, Object>> codeDescriptionData = new Dictionary<Type, Dictionary<Int32, Object>>();
        static Object lockObj = new object();

        private static Dictionary<Int32, Object> AddEnum(Type enumType, Type attrType)
        {
            Dictionary<Int32, Object> enumData = new Dictionary<int, Object>();
            foreach (var fieldItem in enumType.GetFields())
            {
                var descList = fieldItem.GetCustomAttributes(attrType, false);
                if (descList == null)
                {
                    continue;
                }

                try
                {
                    var val = (Int32)Enum.Parse(enumType, fieldItem.Name);
                    enumData[val] = descList[0];
                }
                catch
                {
                    continue;
                }
            }

            lock (lockObj)
            {

                codeDescriptionData[enumType] = enumData;
            }

            return enumData;
        }

        public static Dictionary<Int32, T> GetEnumData<T>(Type enumType)
        {
            Dictionary<Int32, Object> val = null;
            lock (lockObj)
            {
                if (codeDescriptionData.TryGetValue(enumType, out val) == false)
                {
                    val = AddEnum(enumType, typeof(T));
                }
            }

            var result = new Dictionary<Int32, T>();
            foreach (var item in val)
            {
                result[item.Key] = (T)item.Value;
            }

            return result;
        }

        public static Dictionary<Int32, Object> GetEnumData(Type enumType, Type attributeType)
        {
            Dictionary<Int32, Object> val = null;
            lock (lockObj)
            {
                if (codeDescriptionData.TryGetValue(enumType, out val) == false)
                {
                    val = AddEnum(enumType, attributeType);
                }
            }

            return val;
        }

        public static TAttributeType GetAttribute<TEnum, TAttributeType>(Int32 code)
            where TEnum : struct
            where TAttributeType : Attribute
        {
            return (TAttributeType)GetAttribute(typeof(TEnum), typeof(TAttributeType), code);
        }

        public static Object GetAttribute(Type enumType, Type attributeType, Int32 code)
        {
            var enumData = GetEnumData(enumType, attributeType);
            if (enumData.ContainsKey(code))
            {
                return enumData[code];
            }

            return null;
        }
    }
}
