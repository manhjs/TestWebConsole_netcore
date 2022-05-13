using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace drivers
{
    public class TestUtil
    {
        public static String ConvertSpace(String text)
        {
            char[] chars = text.ToCharArray();
            char jpSpace = '　';
            char enSpace = ' ';
            if (chars != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i] == jpSpace)
                    {
                        chars[i] = enSpace;
                    }
                }
            }
            text = new String(chars); ;
            return text;
        }

        // trim Html white space to space
        public static String TrimHtlmSpace(String text)
        {
            if (text == null)
            {
                return null;
            }
            char[] htmlSpaces = { (char)160, (char)32, (char)9248, (char)9251 };
            char[] chars = text.ToCharArray();
            if (chars != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    for (int j = 0; j < htmlSpaces.Length; j++)
                    {
                        if (chars[i] == htmlSpaces[j])
                        {
                            chars[i] = (char)32;
                        }
                    }
                }
            }
            text = new String(chars);
            text = text.Trim();
            return text;
        }

        // split String
        public static String[] SplitString(String listCommand, String separateKey)
        {
            String[] listValues = null;
            if (listCommand != null)
            {
                listValues = listCommand.Split(new String[] { separateKey }, StringSplitOptions.None);
            }
            if (listValues != null)
            {
                for (int i = 0; i < listValues.Length; i++)
                {
                    if (listValues[i] != null)
                    {
                        listValues[i] = listValues[i].Trim();
                    }
                    else
                    {
                        listValues[i] = "";
                    }
                }
            }

            return listValues;
        }

        public static Dictionary<String, Object> AddAllDictionary(Dictionary<String, Object> dest, Dictionary<String, Object> src)
        {
            if (src == null || dest == null)
            {
                return dest;
            }
            else
            {
                foreach (String key in src.Keys)
                {
                    dest.Add(key, src[key]);
                }
                    return dest;
            }
        }

        public static void WriteTextFile(String filePath, String text)
        {
            try
            {
                File.WriteAllText(TestUtil.GetFullPath(filePath), text, Encoding.GetEncoding(TestSetting.GetTestDataEncode()));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

   
        public static String GetFullPath(String filePath)
        {
            if (filePath == null)
            {
                return null;
            } else if (filePath.Contains(Directory.GetCurrentDirectory()))
            {
                filePath = filePath.Replace(Directory.GetCurrentDirectory(), TestSetting.GetResultFolder());
            } else if (!filePath.Contains(TestSetting.GetResultFolder()))
            {
                filePath = TestSetting.GetResultFolder() + @"\" + filePath;
            }

            return filePath;
        }

        public static String ReadTextFile(String filePath)
        {
            try
            {
                filePath = TestUtil.GetFullPath(filePath);
                filePath = filePath.Replace(@"/", @"\");
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath, Encoding.GetEncoding(TestSetting.GetTestDataEncode()));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Boolean IsStringList(List<Object> src)
        {
            Boolean isString = true;
            if (src == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < src.Count; i++)
                {
                    if (!(src[0] is String))
                    {
                        isString = false;
                        break;
                    }

                }
            }
            return isString;
        }

        public static List<String> ConvertToStringList(List<Object> src)
        {
            Boolean isString = true;
            List<String> dst = new List<String>();
            if (src == null)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < src.Count; i++)
                {
                    if (!(IsList(src[i]) && IsDictionary(src[i])))
                    {
                        dst.Add(src[i] as String);
                    }
                    else
                    {
                        isString = false;
                        break;
                    }

                }
            }

            if (isString)
            {
                return dst;
            }
            else
            {
                return null;
            }
        }

        public static void MakeLog(String[] list)
        {
            try
            {
                if (list != null)
                {
                    String[] values = new string[list.Length];
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (list[i] == null)
                        {
                            values[i] = "null";
                        }
                        else
                        {
                            values[i] = list[i];
                        }
                    }
                    String message = "";
                    if (values.Length == 4)
                    {
                        message = "[CMD/FRAME] : " + values[0] + ", [ITEM] : " + values[1] + ", [X/SET VALUE] : "
                                + values[2] + ", [Y/EXPECTED VALUE] : " + values[3];
                    }
                    else
                    {
                        message = "[CMD/FRAME] : " + values[0] + ", [ITEM] : " + values[1] + ", [X/SET VALUE] : "
                                + values[2] + ", [Y/EXPECTED VALUE] : " + values[3] + ", [ACTION] : " + values[4];
                    };
                    Reporter.SaveLog(message);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<String> GetAllStringList(List<Object> src)
        {
            List<String> dst = new List<String>();
            if (src == null)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < src.Count; i++)
                {
                    if (src[i] is String)
                    {
                        dst.Add(src[i] as String);
                    }
                    else if (src[i] is List<Object>)
                    {
                        List<String> sub = GetAllStringList(src[i] as List<Object>);
                        if (sub != null)
                        {
                            dst.AddRange(sub);
                        }
                    }
                }
            }

            return dst;
        }

        private static bool IsList(Object o)
        {
            try
            {
                List<Object> list = (List<Object>)o;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<List<String>> GetTestCases(List<Object> src)
        {
            List<List<String>> result = new List<List<String>>();
            if (IsStringList(src))
            {
                return null;
            }
            else
            {
                foreach (Object it in src)
                {
                    if (IsList(it))
                    {
                        List<String> list = GetAllStringList(it as List<Object>);
                        if (list != null)
                        {
                            result.Add(list);
                        }
                    }
                }
                return result;
            }
        }

        public static bool IsDictionary(Object o)
        {
            try
            {
                Dictionary<String, Object> list = (Dictionary<String, Object>)o;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean IsStringValueDictionary(Dictionary<String, Object> dict)
        {
            Boolean isString = true;
            if (dict == null)
            {
                return false;
            }
            else
            {
                foreach (String key in dict.Keys)
                {
                    if (!(dict[key] is String))
                    {
                        isString = false;
                        break;
                    }
                }
            }
            return isString;
        }

        public static Dictionary<String, String> ConvertToStringValueDictionary(Dictionary<String, Object> src)
        {
            Boolean isString = true;
            Dictionary<String, String> dest = new Dictionary<String, String>();
            if (src == null)
            {
                return null;
            }
            else
            {
                foreach (String key in src.Keys)
                {
                    if (src[key] is String)
                    {
                        dest.Add(key, src[key] as String);
                    }
                    else
                    {
                        isString = false;
                        break;
                    }
                }
            }

            if (isString)
            {
                return dest;
            }
            else
            {
                return null;
            }
        }

        private static bool isList(Object o)
        {
            try
            {
                List<Object> list = (List<Object>)o;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool isDictionary(Object o)
        {
            try
            {
                Dictionary<String, Object> list = (Dictionary<String, Object>)o;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static List<String> convertToStringList(List<Object> src)
        {
            Boolean isString = true;
            List<String> dst = new List<String>();
            if (src == null)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < src.Count; i++)
                {
                    if (!(isList(src[i]) && isDictionary(src[i])))
                    {
                        dst.Add(src[i] as String);
                    }
                    else
                    {
                        isString = false;
                        break;
                    }

                }
            }

            if (isString)
            {
                return dst;
            }
            else
            {
                return null;
            }
        }
        public static Object getObjectInDictionary(Dictionary<String, Object> obj, String key)
        {
            if (obj != null && obj.ContainsKey(key))
            {
                return obj[key];
            }
            return null;
        }
    }
}
