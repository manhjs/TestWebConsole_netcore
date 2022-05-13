using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace drivers
{
    class Program
    {
        public static void Main(String[] args)
        {
            try
            {
                TestSetting.SetResultFolder(Directory.GetCurrentDirectory().Replace("\\lib", ""));
                //TestSetting.SetResultFolder(Directory.GetCurrentDirectory());
                Dictionary<String, Object> setting = ParserText.ParserTextFile(TestSetting.GetSettingFile(), null, null);
                if (setting != null)
                {
                    // setting conditions
                    Dictionary<String, Object> settingObj = (Dictionary<String, Object>)setting[TestSetting.GetSettingKey()];
                    if (settingObj != null)
                    {
                        foreach (String settingKey in settingObj.Keys)
                        {
                            RefClass.SetFieldOfClass(TestSetting.GetSettingclass(), settingKey, settingObj[settingKey]);
                        }
                    }

                    // set language
                    SetLanguage(TestSetting.GetLanguageFolder() + @"\" + TestSetting.GetLanguage() + ".txt", TestSetting.GetLanguageClass());
					Reporter.StartTestReport();    

                    // check all browser
                    List<Object> browsers = (List<Object>)setting[TestSetting.GetBrowsersKey()];
                    List<String> browserKeys = (List<String>)setting[TestSetting.GetBrowsersKey() + "_KEYS"];
                    Boolean foundBrowser = false;
                    Boolean foundTestcase = false;
                    if (browserKeys != null && (browsers != null))
                    {
                        // run on specific browser
                        for (int i = 0; i < browserKeys.Count; i++)
                        {
                            String key = browserKeys[i];
                            foundBrowser = true;
                            TestSetting.SetShowMouseError(false);
                            Dictionary<String, Object> brObj = (Dictionary<String, Object>)browsers[i];
                            if (settingObj != null)
                            {
                                foreach (String brKey in brObj.Keys)
                                {
                                    if (TestUtil.IsStringList(brObj[brKey] as List<Object>))
                                    {
                                        RefClass.SetFieldOfClass(TestSetting.GetSettingclass(), brKey, TestUtil.ConvertToStringList(brObj[brKey] as List<Object>));
                                    }
                                    else
                                    {
                                        RefClass.SetFieldOfClass(TestSetting.GetSettingclass(), brKey, brObj[brKey]);
                                    }
                                }
                            }
                            TestSetting.SetBrowserName(key);
                            Reporter.StartBrowserReport();

                            // loop for each test case file
                            List<String> filePaths = (List<String>)TestSetting.GetFilePaths();
                            if (filePaths != null && filePaths.Count > 0)
                            {
                                foreach (String filePath in filePaths)
                                {
                                    RefFolder folder = new RefFolder();
                                    List<String> subFilePaths = folder.GetAllFiles(TestUtil.GetFullPath(filePath));
                                    foreach (String subfilePath in subFilePaths)
                                    {
                                        TestSetting.SetCurrentFilePath(subfilePath);
                                        if (TestSetting.GetCurrentFilePath() == null)
                                        {
                                            continue;
                                        }
                                        foundTestcase = true;
                                        if (browserKeys != null)
                                        {
                                            // init reporter
                                            Reporter.StartFileReport();
                                            Boolean result = TestCaseExecution.Execution();
                                            Reporter.SaveLog("\n-------------------------------------------------SUMMARY-------------------------------------------------");
                                            if (result)
                                            {
                                                Reporter.SaveLog("\nTEST FILE [" + TestSetting.GetCurrentFilePath() + "] SUCESS in " + key + "!\n");
                                            }
                                            else
                                            {
                                                foreach (String msg in TestCaseExecution.GetErrorList())
                                                {
                                                    Reporter.SaveLog("<ERROR> " + msg);
                                                }
                                                Reporter.SaveLog("\nTEST FILE [" + TestSetting.GetCurrentFilePath() + "] FAILED in " + key + "!\n");
                                                if ("YES".Equals(TestSetting.GetStopErrorFile()))
                                                {
                                                    EndTestWithError();
                                                    return;
                                                }
                                            }
                                            // save test result
                                            Reporter.EndFileReport(TestSetting.GetCurrentFilePath());
                                        }
                                    }
                                }
                            }
                            Reporter.EndBrowserReport();
                        }
                    }
                    Reporter.EndTestReport();
                    // hold the screen
                    if (!foundBrowser)
                    {
                        Console.WriteLine("NOT FOUND BROWSER! Press any key to quit!");
                    }
                    else if (!foundTestcase)
                    {
                        Console.WriteLine("NOT FOUND TEST CASE FILE! Press any key to quit!");
                    }
                    else
                    {
                        Console.WriteLine("Press any key to quit!");
                    }
                    EndTest();
                }
            }
            catch (Exception e)
            {
                Reporter.SaveLog("<ERROR> " + e.Message);
                EndTestWithError();
            }
        }
        private static void EndTestWithError()
        {
            Reporter.EndFileReport(TestSetting.GetCurrentFilePath());
            Reporter.EndBrowserReport();
            Reporter.EndTestReport();
            TestCaseExecution.TearDown();
        }

        private static void EndTest()
        {
            TestCaseExecution.TearDown();
        }

        private static void SetLanguage(String filePath, String classPath)
        {
            Dictionary<String, Object> data = ParserText.ParserTextFile(TestUtil.GetFullPath(filePath), null, null);
            Dictionary<String, Object> setting = (Dictionary<String, Object>)data[TestSetting.GetSettingKey()];
            if (setting != null)
            {
                foreach (String key in setting.Keys)
                {
                    if (TestUtil.IsStringList(setting[key] as List<Object>))
                    {
                        RefClass.SetFieldOfClass(classPath, key, TestUtil.ConvertToStringList(setting[key] as List<Object>));
                    }
                    else
                    {
                        RefClass.SetFieldOfClass(classPath, key, setting[key]);
                    }
                }
            }
        }
    }
}
