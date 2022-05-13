using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace drivers
{
    public class Selenium
    {

        public static void ExecutionCommand(IWebDriver driver, String[] values, String[] nextValues)
        {
            try
            {
                String topFrame = values[0];
                String locator = values[1];
                String setValue = values[2];

                // add mouse cursor to page
                try
                {
                    if (TestSetting.GetShowMouse() != null && TestSetting.GetShowMouse().Trim().Length > 0
                            && !"NO".Equals(TestSetting.GetShowMouse()) && !TestSetting.IsShowMouseError())
                    {
                        IJavaScriptExecutor jsExe = (IJavaScriptExecutor)driver;
                        jsExe.ExecuteScript(TestSetting.GetShowMouse());
                    }
                }
                catch (Exception e)
                {
                    Reporter.SaveLog("<ERROR> show mouse error" + e.Message);
                    TestSetting.SetShowMouseError(true);
                }

                // waiting by user setting
                String waiting = TestSetting.GetEachStepDelay();

                int time = 1000;
                if (waiting != null && !"".Equals(waiting.Trim()))
                {
                    time = Int32.Parse(waiting);
                }
                Thread.Sleep(time);

                // execute command
                if ((topFrame != null) && "WAIT".Equals(topFrame.ToUpper()))
                {
                    Thread.Sleep(Int32.Parse(setValue));
                }
                else if ((topFrame != null) && ("LOAD".Equals(topFrame.Trim().ToUpper()) || "RELOAD".Equals(topFrame.Trim().ToUpper())))
                {
                    if (values[2] != null)
                    {
                        int tabIndex = Int32.Parse(setValue);
                        List<String> tabs = new List<String>(driver.WindowHandles);
                        driver.SwitchTo().Window(tabs[tabIndex]);
                    }
                    if (values[1] != null && values[1].Trim().Length > 0)
                    {
                        if (File.Exists(TestUtil.GetFullPath(locator)))
                        {
                            locator = "file:///" + TestUtil.GetFullPath(locator).Replace(@"\", "/");
                        }
                        driver.Navigate().GoToUrl(locator);
                    }
                }
                else if ((topFrame != null)
                      && ("ALERT".Equals(topFrame.ToUpper()) || "NONE".Equals(topFrame.ToUpper())))
                {
                }
                else if ((topFrame != null) && "KEY".Equals(topFrame.ToUpper()))
                {
                    VitualKey.HitKeys(values[2]);
                }
                else if ((topFrame != null) && "MOUSE".Equals(topFrame.ToUpper()))
                {
                    VitualKey.MouseAction(values);
                }
                else if ((topFrame != null) && "RUNTIME".Equals(topFrame.ToUpper()))
                {
                    using (Process process = new Process())
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = locator;
                        process.StartInfo = startInfo;
                        process.Start();
                    }
                }
                else
                {
                    IWebElement result = Selenium.FindAndExecItem(driver, values, nextValues);
                    if (result == null)
                    {
                        if (values.Length > 4 && "ASSERT_NULL".Equals(values[4].ToUpper()))
                        {
                            Reporter.TestCaseReport(driver, "OK", "OK", "ASSERT_NULL : " + values[1]);
                        }
                        else
                        {
                            throw new Exception("NOT FOUND ELEMENT : " + values[1]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static IWebElement FindAndExecItem(IWebDriver driver, String[] values, String[] nextValues)
        {
            try
            {
                IWebElement result = null;
                String topFrame = values[0];
                String locator = values[1];

                if (topFrame == null || "".Equals(topFrame.Trim()))
                {
                    // search element in top page
                    try
                    {
                        result = FindIWebElement(driver, locator);
                        if (result != null)
                        {
                            ProcessingItem(driver, result, values, nextValues);
                            return result;
                        }
                    }
                    catch (NoSuchElementException e)
                    {
                        Reporter.SaveLog(e.Message);
                        result = null;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Reporter.SaveLog(e.Message);
                        result = null;
                    }
                }
                else
                {
                    // get all frames in site
                    List<IWebElement> frameList = new List<IWebElement>();
                    IList<IWebElement> frames = driver.FindElements(By.TagName("frame"));
                    IList<IWebElement> iframes = driver.FindElements(By.TagName("iframe"));
                    if (frames != null && frames.Count > 0)
                    {
                        frameList.AddRange(frames);
                    }
                    if (iframes != null && iframes.Count > 0)
                    {
                        frameList.AddRange(iframes);
                    }

                    if (frameList.Count > 0)
                    {
                        // loop in each frame
                        foreach (IWebElement frame in frameList)
                        {
                            String frameName = frame.GetAttribute("name");
                            String frameId = frame.GetAttribute("id");
                            driver.SwitchTo().Frame(frame);
                            try
                            {
                                if ((frameName != null && topFrame.Equals(frameName)) || (frameId != null && topFrame.Equals(frameId)))
                                {
                                    result = FindIWebElement(driver, locator);
                                    if (result != null)
                                    {
                                        ProcessingItem(driver, result, values, nextValues);
                                        driver.SwitchTo().ParentFrame();
                                    }
                                    else
                                    {
                                        driver.SwitchTo().ParentFrame();
                                    }
                                }
                                else
                                {
                                    result = FindAndExecItem(driver, values, nextValues);
                                    driver.SwitchTo().ParentFrame();
                                }
                            }
                            catch (NoSuchElementException)
                            {
                                result = FindAndExecItem(driver, values, nextValues);
                                driver.SwitchTo().ParentFrame();
                            }
                            catch (StaleElementReferenceException)
                            {
                                result = FindAndExecItem(driver, values, nextValues);
                                driver.SwitchTo().ParentFrame();
                            }

                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static IWebElement FindIWebElement(IWebDriver driver, String locator)
        {
            try
            {
                List<IWebElement> result = new List<IWebElement>();
                if (driver == null || locator == null)
                {
                    return null;
                }
                if (locator.IndexOf('|') == -1)
                {
                    // find by Xpath
                    return driver.FindElement(By.XPath(locator));

                }
                else
                {
                    // find by cssSelector
                    String[] commands = TestUtil.SplitString(locator, "|");
                    if (commands.Length > 0)
                    {
                        IList<IWebElement> lstElement = driver.FindElements(By.CssSelector(commands[0]));
                        if (lstElement == null)
                        {
                            return null;
                        }
                        else if (lstElement.Count == 1)
                        {
                            return lstElement[0];
                        }
                        else
                        {
                            // choose one
                            int pos = -1;
                            if (commands.Length > 2)
                            {
                                try
                                {
                                    pos = Int32.Parse(commands[2]);
                                }
                                catch (FormatException e)
                                {
                                    Reporter.SaveLog(e.Message);
                                    pos = -1;
                                }
                            }
                            foreach (IWebElement we in lstElement)
                            {
                                String showValue = we.Text;
                                if (showValue != null)
                                {
                                    showValue = TestUtil.TrimHtlmSpace(showValue);
                                }
                                else
                                {
                                    showValue = "";
                                }
                                if (commands[1].Equals(showValue))
                                {
                                    result.Add(we);
                                }
                            }
                            if (result.Count == 1)
                            {
                                return result[0];
                            }
                            else if (pos >= 0 && result.Count > pos)
                            {
                                return result[pos];
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ProcessingItem(IWebDriver driver, IWebElement element, String[] values, String[] nextValues)
        {
            try
            {
                String message = "";
                if (values.Length > 4)
                {
                    message = values[4];
                }
                if (element == null)
                {
                    return;
                }
                else if ((values[2] == null) && (values[3] == null))
                {
                    // default click element
                    ShowItem(driver, element, values, nextValues);
                }
                else if ((values[2] == null) && (values[3] != null))
                {
                    // validation
                    ValidateItem(driver, element, values[3], message);
                }
                else if ((values[2] != null) && (values[3] == null))
                {
                    // setting value
                    SetValueItem(driver, element, values);
                }
                else if (("OK".Equals(values[2])) && ("OK".Equals(values[3])))
                {
                    // capture selenium screen
                    Reporter.TestCaseReport(driver, values[2], values[3], message);
                }
                else
                {
                    // action in coordinate (x, y)
                    ShowItemCoordinate(driver, element, values, nextValues);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ValidateItem(IWebDriver driver, IWebElement element, String expectedValue, String message)
        {
            try
            {
                switch (element.TagName.ToUpper())
                {
                    case "INPUT":
                        if (expectedValue != null)
                        {
                            String value = element.GetAttribute("value");
                            if (value != null)
                            {
                                value = value.Trim().Replace("\n", "");
                            }
                            expectedValue = expectedValue.Trim();
                            Reporter.TestCaseReport(driver, value, expectedValue, message);
                        }
                        break;
                    case "SELECT":
                        if (expectedValue == null)
                        {
                            element.Click();
                        }
                        else if (expectedValue.IndexOf('|') != -1)
                        {
                            // expected list value
                            Boolean blank = true;
                            String[] expectedArray = expectedValue.Split(new String[] { "|" }, StringSplitOptions.None);
                            StringBuilder sb = new StringBuilder();
                            List<String> expList = new List<String>();
                            for (int i = 0; i < expectedArray.Length; i++)
                            {
                                if (i > 0)
                                {
                                    sb.Append(", ");
                                }
                                String value = expectedArray[i].Trim();
                                if ("".Equals(value))
                                {
                                    sb.Append("");
                                    expList.Add("");
                                }
                                else
                                {
                                    blank = false;
                                    sb.Append(value);
                                    expList.Add(value);
                                }
                            }
                            if (blank)
                            {
                                sb = new StringBuilder();
                                sb.Append("");
                                expList = new List<String>();
                                expList.Add("");
                            }
                            else
                            {
                                expectedValue = sb.ToString();
                            }

                            // get value list
                            StringBuilder act = new StringBuilder();
                            List<String> actList = new List<String>();
                            IList<IWebElement> options = element.FindElements(By.TagName("option"));
                            if (options != null && options.Count > 0)
                            {
                                for (int i = 0; i < options.Count; i++)
                                {
                                    String value = options[i].Text;
                                    value = TestUtil.TrimHtlmSpace(value);
                                    if (i > 0)
                                    {
                                        act.Append(", ");
                                    }
                                    if ((value == null) || ("".Equals(value.Trim())))
                                    {
                                        act.Append("");
                                        actList.Add("");
                                    }
                                    else
                                    {
                                        value = value.Trim().Replace("\n", "");
                                        act.Append(value);
                                        actList.Add(value);
                                    }
                                }
                            }
                            else
                            {
                                act.Append("");
                                actList.Add("");
                            }
                            String actualValues = act.ToString();
                            actualValues = actualValues.Replace("\n", "");

                            // validate
                            Boolean check = CompareTwoListWithoutOrder(expList, actList);
                            if (check)
                            {
                                Reporter.TestCaseReport(driver, expectedValue, expectedValue, message);
                            }
                            else
                            {
                                Reporter.TestCaseReport(driver, actualValues, expectedValue, message);
                            }

                        }
                        else
                        {
                            String value = (new SelectElement(element)).SelectedOption.Text;
                            expectedValue = expectedValue.Trim();
                            Reporter.TestCaseReport(driver, value, expectedValue, message);
                        }
                        break;
                    case "IMG":
                    case "CANVAS":
                        break;
                    default:
                        String text = element.Text;
                        if (text != null)
                        {
                            text = TestUtil.TrimHtlmSpace(text).Replace("\n", "");
                        }
                        expectedValue = expectedValue.Trim();
                        if ((text != null) && (expectedValue != null))
                        {
                            Reporter.TestCaseReport(driver, text, expectedValue, message);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Boolean CompareTwoListWithoutOrder(List<String> expList, List<String> actList)
        {
            if (expList == null || actList == null)
            {
                return false;
            }
            else if (expList.Count != actList.Count)
            {
                return false;
            }
            else
            {
                foreach (String item in expList)
                {
                    if (actList.Contains(item))
                    {
                        actList.Remove(item);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private static void SetValueItem(IWebDriver driver, IWebElement element, String[] values)
        {
            try
            {
                String value = values[2];
                if (values.Length > 4 && values[4] != null)
                {
                    if (("SET_ATTRIBUTE").Equals(values[4].ToUpper()))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].setAttribute(" + value +  ");", element);
                    }
                }
                else
                {
                    switch (element.TagName.ToUpper())
                    {
                        case "INPUT":
                        case "TEXTAREA":
                            element.Clear();
                            element.SendKeys(value);
                            break;
                        case "SELECT":
                            // check item existing
                            Boolean found = false;
                            IList<IWebElement> options = element.FindElements(By.TagName("option"));
                            if (value == null || value.Trim().Length == 0)
                            {
                                found = true;
                            }
                            else if (options != null)
                            {
                                for (int i = 0; i < options.Count; i++)
                                {
                                    String optValue = options[i].Text;
                                    optValue = TestUtil.TrimHtlmSpace(optValue);
                                    if (value.Equals(optValue))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (!found)
                            {
                                throw new Exception("Can not set value : [" + value + "]");
                            }
                            // set value
                            element.SendKeys(value);
                            try
                            {
                                element.SendKeys(Keys.Return);
                            }
                            catch (Exception)
                            {}
                            break;
                        default:
                            // set value
                            element.SendKeys(value);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static void ShowItem(IWebDriver driver, IWebElement element, String[] values, String[] nextValues)

        {
            try
            {

                // action key
                String actionKey = "";
                if (values.Length > 4)
                {
                    actionKey = values[4];
                }

                // take action
                TakeAtion(driver, element, values, actionKey);

                // next values
                NextValuesAction(driver, element, values, nextValues);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ShowItemCoordinate(IWebDriver driver, IWebElement element, String[] values, String[] nextValues)
        {
            try
            {

                // action key
                String actionKey = "";
                if (values.Length > 4)
                {
                    actionKey = values[4];
                }

                // get x, y
                int x = Int32.Parse(values[2]);
                int y = Int32.Parse(values[3]);

                // take action in coordinate
                TakeAtionInCoordinate(driver, element, values, actionKey, x, y);

                // next values
                NextValuesAction(driver, element, values, nextValues);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void TakeAtion(IWebDriver driver, IWebElement element, String[] values, String action)
        {
            try
            {
                if (action == null || action.Trim().Length == 0)
                {
                    element.Click();
                }
                else
                {
                    switch (action.ToUpper())
                    {
                        case "CLICK":
                            new Actions(driver).MoveToElement(element).Click().Perform();
                            break;
                        case "DOUBLE_CLICK":
                            new Actions(driver).MoveToElement(element).DoubleClick().Perform();
                            break;
                        case "CONTEXT_CLICK":
                            new Actions(driver).MoveToElement(element).ContextClick().Perform();
                            break;
                        case "MOVE_TO_ELEMENT":
                            new Actions(driver).MoveToElement(element).Perform();
                            break;
                        case "CLICK_AND_HOLD":
                            new Actions(driver).MoveToElement(element).ClickAndHold().Perform();
                            break;
                        case "RELEASE":
                            new Actions(driver).MoveToElement(element).Release().Perform();
                            break;
                        case "ASSERT_NOT_NULL":
                            new Actions(driver).MoveToElement(element).Perform();
                            Reporter.TestCaseReport(driver, "OK", "OK", "ASSERT_NOT_NULL : " + values[1]);
                            break;
                        case "ASSERT_NULL":
                            new Actions(driver).MoveToElement(element).Perform();
                            throw new Exception("FOUND ELEMENT : " + values[1]);
                        case "ELEMENT_VISIBLE":
                            new Actions(driver).MoveToElement(element).Perform();
                            element.Click();
                            Reporter.TestCaseReport(driver, "OK", "OK", "ELEMENT_VISIBLE : " + values[1]);
                            break;
                        case "ELEMENT_NOT_VISIBLE":
                            new Actions(driver).MoveToElement(element).Perform();
                            element.Click();
                            throw new Exception("ELEMENT VISIBLE : " + values[1]);
                        default:
                            element.Click();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void TakeAtionInCoordinate(IWebDriver driver, IWebElement element, String[] values, String action,
                int x, int y)
        {
            try
            {
                if (action == null || action.Trim().Length == 0)
                {
                    new Actions(driver).MoveToElement(element, x, y).Click().Perform();
                    return;
                }
                else
                {
                    switch (action.ToUpper())
                    {
                        case "MOVE_TO_ELEMENT":
                            new Actions(driver).MoveToElement(element, x, y).Perform();
                            break;
                        case "CLICK":
                            new Actions(driver).MoveToElement(element, x, y).Click().Perform();
                            break;
                        case "DOUBLE_CLICK":
                            new Actions(driver).MoveToElement(element, x, y).DoubleClick().Perform();
                            break;
                        case "CONTEXT_CLICK":
                            new Actions(driver).MoveToElement(element, x, y).ContextClick().Perform();
                            break;
                        case "MOVE_BY_OFFSET":
                            new Actions(driver).MoveToElement(element).MoveByOffset(x, y).Perform();
                            break;
                        case "CLICK_AND_HOLD":
                            new Actions(driver).MoveToElement(element, x, y).ClickAndHold().Perform();
                            break;
                        case "RELEASE":
                            new Actions(driver).MoveToElement(element, x, y).Release().Perform();
                            break;
                        case "ASSERT_NOT_NULL":
                            new Actions(driver).MoveToElement(element).Perform();
                            Reporter.TestCaseReport(driver, "OK", "OK", "ASSERT_NOT_NULL : " + values[1]);
                            break;
                        case "ASSERT_NULL":
                            new Actions(driver).MoveToElement(element).Perform();
                            throw new Exception("FOUND ELEMENT : " + values[1]);
                        case "ELEMENT_VISIBLE":
                            new Actions(driver).MoveToElement(element).Perform();
                            new Actions(driver).MoveToElement(element, x, y).Click().Perform();
                            Reporter.TestCaseReport(driver, "OK", "OK", "ELEMENT_VISIBLE : " + values[1]);
                            break;
                        case "ELEMENT_NOT_VISIBLE":
                            new Actions(driver).MoveToElement(element).Perform();
                            new Actions(driver).MoveToElement(element, x, y).Click().Perform();
                            throw new Exception("ELEMENT VISIBLE : " + values[1]);
                        default:
                            new Actions(driver).MoveToElement(element, x, y).Click().Perform();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void NextValuesAction(IWebDriver driver, IWebElement element, String[] values, String[] nextValues)
        {
            try
            {
                if (nextValues != null && nextValues[0] != null)
                {
                    if ("NONE".Equals(nextValues[0].ToUpper()))
                    {
                        String[] buffValues = new String[nextValues.Length];
                        for (int i = 0; i < nextValues.Length; i++)
                        {
                            buffValues[i] = nextValues[i];
                        }
                        buffValues[0] = values[0];
                        String locator = TestCaseExecution.GetLocatorsMap()[buffValues[1]];
                        if (locator != null)
                        {
                            buffValues[1] = locator;
                        }
                        ExecutionCommand(driver, buffValues, null);
                    }
                    else if ("ALERT".Equals(nextValues[0].ToUpper()))
                    {
                        TestUtil.MakeLog(nextValues);
                        String waiting = TestSetting.GetWaitForAlert();
                        int time = 10000;
                        if (waiting != null && !"".Equals(waiting.Trim()))
                        {
                            time = Int32.Parse(waiting);
                        }
                        if (WaitForAlert(driver, time))
                        {
                            HandledAlertException(driver, nextValues[1], nextValues[2], nextValues[3]);
                        }
                        else
                        {
                            Reporter.SaveLog("<WARNING> No Alert appear during " + time / 1000 + " seconds!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void HandledAlertException(IWebDriver driver, String commands, String setValues,
                String validateValues)
        {
            try
            {
                if (commands != null)
                {
                    String[] listCommand = TestUtil.SplitString(commands, "[|]");
                    String[] listValidateValues = TestUtil.SplitString(validateValues, "[|]");
                    String[] listSetValues = TestUtil.SplitString(setValues, "[|]");
                    try
                    {
                        for (int i = 0; i < listCommand.Length; i++)
                        {
                            if (listCommand[i] != null)
                            {
                                String cmd = listCommand[i].Trim();
                                switch (cmd.ToUpper())
                                {
                                    case "DISMISS":
                                        driver.SwitchTo().Alert().Dismiss();
                                        break;
                                    case "ACCEPT":
                                        driver.SwitchTo().Alert().Accept();
                                        break;
                                    case "ASSERT":
                                        String val = listValidateValues[i].Trim();
                                        val = val.Replace("\n", " ");
                                        IAlert alert = driver.SwitchTo().Alert();
                                        String AlertText = alert.Text;
                                        AlertText = TestUtil.TrimHtlmSpace(AlertText);
                                        alert.Accept();
                                        Reporter.TestCaseReport(driver, AlertText, val, null);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch (NoAlertPresentException e)
                    {
                        Reporter.SaveLog(e.Message);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Boolean WaitForAlert(IWebDriver driver, int time)
        {
            try
            {
                int i = 0;
                while (i < time)
                {
                    try
                    {
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                        try
                        {
                            // Attempt to switch to an alert
                            driver.SwitchTo().Alert();
                            return true;
                        }
                        catch (NoAlertPresentException)
                        {
                        }
                    }
                    catch (TimeoutException e)
                    {
                        Reporter.SaveLog(e.Message);
                    }
                    i += 1000;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
