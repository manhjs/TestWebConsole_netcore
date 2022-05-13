using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Reflection;

namespace drivers
{
    public class TestCaseExecution
    {
        private static IWebDriver driver = null;
        private static Dictionary<String, String> locatorsMap = new Dictionary<String, String>();
        private static List<String> nameList = new List<String>();
        private static List<String> errorList = new List<String>();
        private static int totalTestCases = 0;

        public static int GetTotalTestCases()
        {
            return totalTestCases;
        }

        public static Dictionary<String, String> GetLocatorsMap()
        {
            return locatorsMap;
        }

        public static List<String> GetNameList()
        {
            return nameList;
        }

        public static List<String> GetErrorList()
        {
            return errorList;
        }

        public static Boolean Execution()
        {
            Boolean result = true;
            try
            {
                List<List<String>> dataList = null;
                Dictionary<String, Object> data = ParserText.ParserTextFile(TestSetting.GetCurrentFilePath(), null, TestSetting.GetBrowserName());
                if (data != null)
                {
                    String key = TestSetting.GetLocatorsKey();
                    if (data.ContainsKey(key))
                    {
                        if (TestUtil.IsStringValueDictionary((Dictionary<String, Object>)data[key]))
                        {
                            locatorsMap = TestUtil.ConvertToStringValueDictionary((Dictionary<String, Object>)data[key]);
                        }
                    }

                    nameList = new List<String>();
                    key = TestSetting.GetCommandsKey() + "_KEYS";
                    if (data.ContainsKey(key))
                    {
                        nameList = (List<String>)data[key];
                    }
                    key = TestSetting.GetCommandsKey();
                    if (data.ContainsKey(key))
                    {
                        dataList = TestUtil.GetTestCases((List<Object>)data[key]);
                    }
                    if (dataList != null)
                    {
                        totalTestCases = dataList.Count;
                    }
                    else
                    {
                        totalTestCases = 0;
                    }
                    errorList = new List<String>();
                }
                if (dataList == null || !ValidateDataList(dataList))
                {
                    dataList = new List<List<String>>();
                }
                for (int i = 0; i < dataList.Count; i++)
                {
                    // set up
                    SetUp();

                    // test
                    List<String> list = dataList[i];
                    try
                    {
                        TestCase(list);
                        Reporter.EndTestCaseReport();
                    }
                    catch (Exception e)
                    {
                        errorList.Add(nameList[i] + " : " + e.Message);
                        Reporter.EndTestCaseReport();
                        if ("YES".Equals(TestSetting.GetStopErrorFile()))
                        {
                            return false;
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    // tear down
                    TearDown();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return result;
        }

        private static void SetUp()
        {
            try
            {
                // get driver
                Assembly asm = typeof(DriverOptions).Assembly;
                Type type = asm.GetType(TestSetting.GetClassName());
                ConstructorInfo publicConstructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null);
                driver = (IWebDriver)publicConstructor.Invoke(new string[] { TestSetting.GetResultFolder() + @"\" + TestSetting.GetDriversFolder() });

                // full screen
                driver.Manage().Window.Maximize();

                // get mouse cursor script
                if (!"NO".Equals(TestSetting.GetShowMouse()) && !TestSetting.IsShowMouseError())
                {
                    try
                    {
                        TestSetting.SetShowMouse(TestUtil.ReadTextFile("js/mouse.js"));
                    }
                    catch (Exception e)
                    {
                        Reporter.SaveLog("<ERROR> " + e.Message);
                        TestSetting.SetShowMouseError(true);
                    }
                }

                // setting test conditions
                if (TestSetting.GetCommands() != null && TestSetting.GetLocators() != null)
                {
                    for (int i = 0; i < TestSetting.GetCommands().Count; i++)
                    {
                        String[] values = SplitCommand(TestSetting.GetCommands()[i], ",");
                        String[] nextValues = null;
                        if (i < (TestSetting.GetCommands().Count - 1))
                        {
                            nextValues = SplitCommand(TestSetting.GetCommands()[i + 1], ",");
                            if (nextValues.Length < 4)
                            {
                                nextValues = null;
                            }
                        }

                        String locator = TestSetting.GetLocators()[values[1]];
                        if (locator != null)
                        {
                            values[1] = locator;
                        }
                        try
                        {
                            Selenium.ExecutionCommand(driver, values, nextValues);
                        }
                        catch (UnhandledAlertException e)
                        {
                            try
                            {
                                driver.SwitchTo().Alert().Dismiss();
                            }
                            catch (NoAlertPresentException)
                            {
                            }
                            Reporter.SaveLog("<ERROR> " + e.Message);
                            throw new Exception("<ERROR> " + e.Message);
                        }
                        catch (NoAlertPresentException e)
                        {
                            Reporter.SaveLog("<ERROR> " + e.Message);
                        }
                        catch (Exception e)
                        {
                            Reporter.SaveLog("<ERROR> " + e.Message);
                            throw new Exception("<ERROR> " + e.Message);
                        }
                    }
                }
                Reporter.StartTestCaseReport();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void TearDown()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void TestCase(List<String> data)
        {
            try
            {
                String name = "";
                if (nameList != null && nameList.Count > Reporter.GetRowIndex())
                {
                    name = nameList[Reporter.GetRowIndex()];
                }
                String testcaseName = "----------------[" + TestSetting.GetCurrentFilePath().Replace(@"\", "/") + " | " + name.Replace(@"\", "/") + "]----------------";
                Reporter.SaveLog(testcaseName);
                List<String> commands = ParserCommands(data);
                if (commands != null)
                {
                    for (int i = 0; i < commands.Count; i++)
                    {
                        String[] values = SplitCommand(commands[i], ",");
                        if (values != null)
                        {
                            String[] nextValues = null;
                            if (i < (commands.Count - 1))
                            {
                                nextValues = SplitCommand(commands[i + 1], ",");
                                if (nextValues == null || nextValues.Length < 4)
                                {
                                    nextValues = null;
                                }
                            }
                            if (values != null && values.Length >= 4)
                            {
                                try
                                {
                                    if (values[0] != null && "ALERT".Equals(values[0].ToUpper()))
                                    {

                                    }
                                    else
                                    {
                                        TestUtil.MakeLog(values);
                                    }
                                    String locator = null;
                                    if (values[1] != null && locatorsMap.ContainsKey(values[1]))
                                    {
                                        locator = locatorsMap[values[1]];
                                    }
                                    if (locator != null)
                                    {
                                        values[1] = locator;
                                    }
                                    Selenium.ExecutionCommand(driver, values, nextValues);
                                }
                                catch (ElementNotVisibleException e)
                                {
                                    if (values.Length > 4 && "ELEMENT_NOT_VISIBLE".Equals(values[4].ToUpper()))
                                    {
                                        Reporter.TestCaseReport(driver, "OK", "OK", "ELEMENT_NOT_VISIBLE : " + values[1]);
                                    }
                                    else
                                    {
                                        Reporter.TestCaseErrorReport(driver, "ELEMENT NOT VISIBLE : " + values[1], "");
                                        Reporter.SaveLog("<ERROR> " + "ELEMENT NOT VISIBLE : " + values[1]);
                                        throw new Exception(e.Message);
                                    }
                                }
                                catch (UnhandledAlertException e)
                                {
                                    try
                                    {
                                        driver.SwitchTo().Alert().Dismiss();
                                    }
                                    catch (NoAlertPresentException)
                                    {
                                    }
                                    Reporter.TestCaseErrorReport(driver, e.Message, "");
                                    Reporter.SaveLog("<ERROR> " + e.Message);
                                    throw new Exception(e.Message);
                                }
                                catch (Exception e)
                                {
                                    Reporter.TestCaseErrorReport(driver, e.Message, "");
                                    Reporter.SaveLog("<ERROR> " + e.Message);
                                    throw new Exception(e.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private List<String> ParserCommands(List<Object> list)
        {
            try
            {
                if (list != null)
                {
                    List<String> results = new List<String>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] is List<Object>)
                        {
                            List<Object> script = (list[i] as List<Object>);
                            if (script != null && script.Count > 0)
                            {
                                List<String> subResults = ParserCommands(script);
                                results.AddRange(subResults);
                            }
                        }
                        else if (list[i] is String)
                        {
                            results.Add(list[i] as System.String);
                        }
                    }
                    return results;
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

        private static List<String> ParserCommands(List<String> list)
        {
            try
            {
                if (list != null)
                {
                    List<String> results = new List<String>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        results.Add(list[i] as System.String);
                    }
                    return results;
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



        private static String[] SplitCommand(String listCommand, String separateKey)
        {
            try
            {
                String[] listValues = null;
                if (listCommand != null)
                {
                    List<QuoteData> list = QuoteValidateIndexs(QuoteFindIndexs(listCommand));
                    Dictionary<String, String> noneQuoteMap = new Dictionary<String, String>();
                    Dictionary<String, String> quoteMap = new Dictionary<String, String>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        QuoteData data = list[i];
                        String noneQuoteValue = listCommand.Substring(data.GetQuoteEndIndex() + 1, data.GetEndIndex() - data.GetQuoteEndIndex() - 1);
                        String quoteValue = listCommand.Substring(data.GetStartIndex() + 1, data.GetEndIndex() - data.GetStartIndex());
                        noneQuoteMap.Add(QuoteData.KEY_PRIFIX + i, noneQuoteValue);
                        quoteMap.Add(QuoteData.KEY_PRIFIX + i, quoteValue);
                    }
                    foreach (String key in quoteMap.Keys)
                    {
                        listCommand = listCommand.Replace(quoteMap[key], key);
                    }
                    listValues = listCommand.Split(new String[] { separateKey }, StringSplitOptions.None);
                    for (int i = 0; i < listValues.Length; i++)
                    {
                        foreach (String key in quoteMap.Keys)
                        {
                            if (listValues[i].Trim().Equals(key))
                            {
                                listValues[i] = noneQuoteMap[key];
                            }
                        }
                    }
                }
                if (listValues != null)
                {
                    for (int i = 0; i < listValues.Length; i++)
                    {
                        if (listValues[i] != null)
                        {
                            listValues[i] = listValues[i].Trim();
                            if (listValues[i].ToUpper().Equals("NULL"))
                            {
                                listValues[i] = null;
                            }
                            else if (listValues[i].Equals(""))
                            {
                                listValues[i] = "";
                            }
                        }
                    }
                }
                return listValues;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Boolean ValidateDataList(List<List<String>> dataList)
        {
            try
            {
                if (dataList == null)
                {
                    return false;
                }
                else
                {
                    Reporter.GetFileResult().SetTotalTc(dataList.Count);
                    int errs = 0;
                    int waitingTime = 10;
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        if (dataList[i] == null || !(dataList[i] is List<String>))
                        {
                            Reporter.GetFileResult().IncreaseMistakeTc();
                            String message = "<ERROR> testcase[" + (i + errs) + "] has some mistakes!";
                            Reporter.SaveLog(message + " Press any key to quit in " + waitingTime + " seconds!...");
                            if (WaitForTimeOut(waitingTime))
                            {
                                throw new Exception(message);
                            }
                            else
                            {
                                dataList.RemoveAt(i);
                                i--;
                                errs++;
                            }
                            Reporter.SaveLog(" ...run without this test case!");
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Boolean WaitForTimeOut(int sec)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                DateTime timeoutvalue = currentTime.AddSeconds(-sec);
                while (currentTime > timeoutvalue)
                {
                    ConsoleKeyInfo cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.X && cki.Modifiers == ConsoleModifiers.Control)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static List<QuoteData> QuoteValidateIndexs(List<QuoteData> lst)
        {
            try
            {
                Boolean checkStartFlag = false;
                List<QuoteData> lstIndex = new List<QuoteData>();
                QuoteData data = new QuoteData();
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].GetCode().Equals(QuoteData.START_SIGNAL))
                    {
                        checkStartFlag = true;
                        data.SetStartIndex(lst[i].GetStartIndex());
                        data.SetQuoteEndIndex(lst[i].GetEndIndex());
                    }
                    else if (lst[i].GetCode().Equals(QuoteData.END_SIGNAL))
                    {
                        if (checkStartFlag)
                        {
                            data.SetEndIndex(lst[i].GetStartIndex());
                            lstIndex.Add(data);
                            data = new QuoteData();
                            checkStartFlag = false;
                        }
                    }
                }
                return lstIndex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static List<QuoteData> QuoteFindIndexs(String str)
        {
            try
            {
                char[] lst = null;
                Boolean checkStart = false;
                Boolean checkEnd = false;
                QuoteData startPoint = new QuoteData();
                QuoteData endPoint = new QuoteData();
                List<QuoteData> lstIndex = new List<QuoteData>();
                if (str != null)
                {
                    lst = str.Trim().ToCharArray();
                    for (int i = 0; i < lst.Length; i++)
                    {
                        checkStart = QuoteCheckCouple(startPoint, checkStart, i, lst, QuoteData.COMMA, QuoteData.DOUBLE_QUOTE, QuoteData.START_SIGNAL);
                        if (startPoint.GetStartIndex() > -1 && startPoint.GetEndIndex() > -1)
                        {
                            lstIndex.Add(startPoint);
                            startPoint = new QuoteData();
                            continue;
                        }

                        checkEnd = QuoteCheckCouple(endPoint, checkEnd, i, lst, QuoteData.DOUBLE_QUOTE, QuoteData.COMMA, QuoteData.END_SIGNAL);
                        if (endPoint.GetStartIndex() > -1 && endPoint.GetEndIndex() > -1)
                        {
                            lstIndex.Add(endPoint);
                            endPoint = new QuoteData();
                        }
                    }
                }
                return lstIndex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Boolean QuoteCheckCouple(QuoteData point, Boolean check, int i, char[] charArr, char checkCharStart,
                char checkCharEnd, String key)
        {
            try
            {
                if (charArr[i] == checkCharStart)
                {
                    check = true;
                    point.SetStartIndex(i);
                    if (i == charArr.Length - 1)
                    {
                        point.SetEndIndex(i);
                        point.SetCode(key);
                    }
                }
                else if (charArr[i] == checkCharEnd)
                {
                    if (check)
                    {
                        point.SetEndIndex(i);
                        point.SetCode(key);
                        check = false;
                    }
                }
                else if (charArr[i] != ' ')
                {
                    if (check)
                    {
                        check = false;
                    }
                }
                return check;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    class QuoteData
    {
        public static char COMMA = ',';
        public static char DOUBLE_QUOTE = '\"';
        public static String START_SIGNAL = "S";
        public static String END_SIGNAL = "E";
        public static String KEY_PRIFIX = "key_";
        private int startIndex = -1;
        private int endIndex = -1;
        private int quoteStartIndex = -1;
        private int quoteEndIndex = -1;
        private String code;

        public int GetStartIndex()
        {
            return startIndex;
        }

        public void SetStartIndex(int start)
        {
            this.startIndex = start;
        }

        public int GetEndIndex()
        {
            return endIndex;
        }

        public void SetEndIndex(int end)
        {
            this.endIndex = end;
        }

        public String GetCode()
        {
            return code;
        }

        public void SetCode(String code)
        {
            this.code = code;
        }

        public int GetQuoteStartIndex()
        {
            return quoteStartIndex;
        }

        public void SetQuoteStartIndex(int quoteStartIndex)
        {
            this.quoteStartIndex = quoteStartIndex;
        }

        public int GetQuoteEndIndex()
        {
            return quoteEndIndex;
        }

        public void SetQuoteEndIndex(int quoteEndIndex)
        {
            this.quoteEndIndex = quoteEndIndex;
        }
    }
}
