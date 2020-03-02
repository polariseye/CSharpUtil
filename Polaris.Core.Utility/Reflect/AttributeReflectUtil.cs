using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.Utility.Reflect
{
    public class AttributeReflectUtil
    {
        public Dictionary<Type, List<Tuple<MethodInfo, Attribute>>> GetAll(Assembly assemblyItem, Type attrType)
        {
            var result = new Dictionary<Type, List<Tuple<MethodInfo, Attribute>>>();

            var allType = assemblyItem.GetExportedTypes();
            foreach (var classType in allType)
            {
                if (classType.Name.ToLower().EndsWith("bll") == false)
                {
                    // 只反射bll结尾的类
                    continue;
                }

                var methodList = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (var methodItem in methodList)
                {
                    var attrItem = methodItem.GetCustomAttribute(attrType);
                    if (attrItem == null)
                    {
                        continue;
                    }

                    if (result.ContainsKey(classType) == false)
                    {
                        result[classType] = new List<Tuple<MethodInfo, Attribute>>();
                    }

                    result[classType].Add(Tuple.Create(methodItem, attrItem));
                }
            }

            return result;
        }

        public void Load(String assemblyName)
        {
            this.Load(Assembly.ReflectionOnlyLoad(assemblyName));
        }

        public void Load(Assembly assemblyItem)
        {
            assemblyItem.GetExportedTypes();
        }
    }
}
