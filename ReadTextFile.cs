using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace drivers
{
    public class ParserText
    {

        public static Dictionary<String, Object> ParserTextFile(String filePath, Dictionary<String, Object> result, String browserName)
        {
            try
            {
                if ((result == null))
                {
                    result = new Dictionary<String, Object>();
                }

                Dictionary<String, Object> hm = null;
                List<Object> lst = null;
                List<String> keys = null;
                String key = null;

                String line = null;
                using (StreamReader sr = new StreamReader(TestUtil.GetFullPath(filePath), Encoding.GetEncoding(TestSetting.GetTestDataEncode())))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (((line == null) || (line.Trim().Length == 0)))
                        {
                            continue;
                        }
                        else
                        {
                            line = line.Trim();
                        }

                        if ((line.StartsWith("/") || line.StartsWith("#")))
                        {
                            continue;
                        }
                        else if (line.StartsWith("["))
                        {
                            if (((key != null) && ((hm == null) && (lst != null))))
                            {
                                if (TestSetting.GetImportKey().Equals(key))
                                {
                                    for (int i = 0; i < lst.Count; i++)
                                    {
                                        RefFolder folder = new RefFolder();
                                        List<String> subFilePaths = folder.GetAllFiles(((String)(lst[i])));
                                        foreach (String subfilePath in subFilePaths)
                                        {
                                            result = ParserText.ParserTextFile(subfilePath, result, browserName);
                                        }
                                    }
                                }
                                else 
                                {
                                    if (lst.Count > 0)
                                    {
                                        result[key] = lst;
                                    }
                                    if ((keys != null) && (keys.Count > 0))
                                    {
                                        result[(key + "_KEYS")] = keys;
                                    }
                                }

                            }
                            else if (((key != null) && ((hm != null) && (lst == null))))
                            {
                                Dictionary<String, Object> exHm = null;
                                Boolean exsit = false;
                                if (result.ContainsKey(key))
                                {
                                    exHm = ((Dictionary<String, Object>)(result[key]));
                                    exsit = true;
                                }
                                if (((exHm != null) && TestSetting.GetLocatorsKey().Equals(key)))
                                {
                                    foreach (String hmKey in hm.Keys)
                                    {
                                        exHm.Add(hmKey, hm[hmKey]);
                                    }
                                    if (exsit)
                                    {
                                        result[key] = exHm;
                                    }
                                    else
                                    {
                                        result.Add(key, exHm);
                                    }
                                }
                                else
                                {
                                    if (exsit)
                                    {
                                        result[key] = hm;
                                    }
                                    else
                                    {
                                        result.Add(key, hm);
                                    }
                                }
                            }

                            key = line;
                            key = key.Replace("[", "");
                            key = key.Replace("]", "");
                            key = ParserText.CleanUpKey(key);
                            hm = new Dictionary<String, Object>();
                            lst = new List<Object>();
                            keys = new List<String>();
                        }
                        else
                        {
                            String[] tokens = line.Split(new String[] { "==" }, StringSplitOptions.None);
                            for (int i = 0; (i < tokens.Length); i++)
                            {
                                if ((tokens[i] != null))
                                {
                                    tokens[i] = tokens[i].Trim();
                                }
                                else
                                {
                                    tokens[i] = "";
                                }

                            }

                            if ((tokens.Length == 1))
                            {
                                if ((lst != null))
                                {
                                    Object obj = null;
                                    String extKey = null;
                                    if (browserName != null)
                                    {
                                        extKey = tokens[0] + "(" + browserName.Trim() + ")";
                                    }
                                    if ((browserName != null) && result.ContainsKey(extKey))
                                    {
                                        obj = result[extKey];
                                    }

                                    if ((obj == null) && result.ContainsKey(tokens[0]))
                                    {
                                        obj = result[tokens[0]];
                                    }

                                    if ((obj == null))
                                    {
                                        lst.Add(tokens[0]);
                                    }
                                    else
                                    {
                                        lst.Add(obj);
                                        keys.Add(tokens[0]);
                                    }
                                }
                                hm = null;
                            }
                            else if ((tokens.Length == 2))
                            {
                                if ((hm != null))
                                {
                                    Object obj = null;
                                    String extKey = null;
                                    if (browserName != null)
                                    {
                                        extKey = tokens[1] + "(" + browserName.Trim() + ")";
                                    }
                                    if ((browserName != null) && result.ContainsKey(extKey))
                                    {
                                        obj = result[extKey];
                                    }

                                    if ((obj == null) && result.ContainsKey(tokens[1]))
                                    {
                                        obj = result[tokens[1]];
                                    }

                                    if ((obj == null) && hm.ContainsKey(tokens[0]))
                                    {
                                        hm.Remove(tokens[0]);
                                    }

                                    if ((obj == null))
                                    {
                                        hm.Add(tokens[0], tokens[1]);
                                    }
                                    else
                                    {
                                        hm.Add(tokens[0], obj);
                                    }
                                }

                                lst = null;
                                keys = null;
                            }
                        }
                    }
                }

                ParserObject(result, result);
                
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ParserObject(Object obj, Dictionary<String, Object> result)
        {
            if ((obj is List<Object>))
            {
                List<Object> list = obj as List<Object>;
                List<String> keyList = new List<String>();
                for (int i = 0; i < list.Count; i++)
                {
                    if ((list[i] is String))
                    {
                        if (result.ContainsKey(list[i] as String))
                        {
                            String key = list[i] as String;
                            list[i] = result[key];
                            keyList.Add(key);
                        }
                    }
                    else
                    {
                         ParserObject(list[i], result);
                    }
                    if (keyList.Count > 0)
                    {
                        String supKey = GetObjectKey(result, obj);
                        if (supKey != null)
                        {
                            supKey = supKey + "_KEYS";
                            if (result.ContainsKey(supKey))
                            {
                                result[supKey] = keyList;
                            }
                            else
                            {
                                result.Add(supKey, keyList);
                            }
                        }
                    }
                }
            }
            else if ((obj is Dictionary<String, Object>))
            {
                Dictionary<String, Object> dict = obj as Dictionary<String, Object>;
                List<String> keyList = new List<String>(dict.Keys);
                foreach (String key in keyList)
                {
                    if ((dict[key] is String))
                    {
                        if (result.ContainsKey(dict[key] as String))
                        {
                            dict[key] = result[dict[key] as String];
                        }
                    }
                    else if (!key.EndsWith("_KEYS"))
                    {
                        ParserObject(dict[key], result);
                    }
                }
            }
        }

        public static String GetObjectKey(Dictionary<String, Object> map, Object value)
        {
            List<String> keyList = new List<String>(map.Keys);
            foreach (String key in keyList)
            {
                if (map[key].Equals(value))
                {
                    return key;
                }
            }

            return null;
        }

        private static String CleanUpKey(String key)
        {
            if (((key == null)
                        || (key.Trim().Length == 0)))
            {
                return "";
            }

            if (((key.IndexOf('(') != -1)
                        && (key.IndexOf(')') != -1)))
            {
                char[] chars = key.ToCharArray();
                bool foundStart = false;
                bool foundEnd = false;
                int startPost = 0;
                int endPost = 0;
                if ((chars != null))
                {
                    for (int i = (chars.Length - 1); (i >= 0); i--)
                    {
                        if ((chars[i] == ')'))
                        {
                            endPost = i;
                            foundEnd = true;
                            break;
                        }

                    }

                    for (int i = (chars.Length - 1); (i >= 0); i--)
                    {
                        if ((chars[i] == '('))
                        {
                            startPost = i;
                            foundStart = true;
                            break;
                        }
                    }

                }

                if ((foundStart && foundEnd))
                {
                    String startkey = key.Substring(0, startPost).Trim();
                    String endKey = key.Substring((startPost + 1), endPost - startPost).Trim();
                    key = (startkey + ("(" + (endKey + ")")));
                }
            }

            return key.Trim();
        }
    }
}
