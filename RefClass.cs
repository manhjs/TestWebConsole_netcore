using System;
using System.Reflection;

namespace drivers
{
    public class RefClass
    {
        public static void SetFieldOfClass(String className, String fieldName, Object value)
        {
            try
            {
                Type refType = Type.GetType(className);
                Object refObj = Activator.CreateInstance(refType);
                FieldInfo refFieldInfo = refType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
                refFieldInfo.SetValue(refObj, value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Object GetFieldOfClass(String className, String fieldName)
        {
            try
            {
                Type refType = Type.GetType(className);
                Object refObj = Activator.CreateInstance(refType);
                FieldInfo refFieldInfo = refType.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
                return refFieldInfo.GetValue(refObj);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
