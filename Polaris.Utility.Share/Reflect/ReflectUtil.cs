using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.Utility
{
    public class ReflectUtil
    {
        public static List<Type> GetByInheritedInterface(Assembly assemblyItem, Type interfaceType)
        {
            var result = new List<Type>();

            var allType = assemblyItem.GetExportedTypes();
            foreach (var classType in allType)
            {
                if (classType.IsClass == false)
                {
                    continue;
                }

                if (classType.GetInterfaces().Any(tmp => tmp == interfaceType) == false)
                {
                    continue;
                }

                result.Add(classType);
            }

            return result;
        }
    }
}
