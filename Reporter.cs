using System;
using System.IO;
using System.Text;
using OpenQA.Selenium;

namespace drivers
{
    public class Reporter
    {

        private static String testCaseBuff = "";
        private static int rowIndex = 0;
        private static int rowSpanIndex = -1;
        private static int rowSpanNumber = 0;
        private static int fileIndex = 0;
        private static TestFileResult fileResult = new TestFileResult();
        private static TestFileResult totalFileResult = new TestFileResult();
        private static Boolean result = true;
        public static StringBuilder summaryBuff = new StringBuilder();
        public static StringBuilder summaryLogBuff = new StringBuilder();
        public static StringBuilder fileBuff = new StringBuilder();
        private static String fileStartedDate = "";
        private static String testStartedDate = "";
        private static StringBuilder logBuffer = new StringBuilder();
        private static String[] fileHeader = {"Index", "Testcase", "Result", "Message", "Comment", "Evidence"};
        private static String[] summaryHeader = {"No.", "File Name", "Total", "Mistake", "OK", "NG", "Remain"};

        public static String GetTestCaseBuff()
        {
            return testCaseBuff;
        }

        public static void SetTestCaseBuff(String testCaseBuff)
        {
            Reporter.testCaseBuff = testCaseBuff;
        }

        public static int GetRowIndex()
        {
            return rowIndex;
        }

        public static void SetRowIndex(int rowIndex)
        {
            Reporter.rowIndex = rowIndex;
        }

        public static void IncreaseRowIndex()
        {
            Reporter.rowIndex += 1;
        }

        public static int GetRowSpanIndex()
        {
            return rowSpanIndex;
        }

        public static void SetRowSpanIndex(int rowSpanIndex)
        {
            Reporter.rowSpanIndex = rowSpanIndex;
        }

        public static int SetRowSpanNumber()
        {
            return rowSpanNumber;
        }

        public static void IncreaseRowSpanNumber()
        {
            rowSpanNumber += 1;
        }

        public static void SetRowSpanNumber(int rowSpanNumber)
        {
            Reporter.rowSpanNumber = rowSpanNumber;
        }

        public static int GetFileIndex()
        {
            return fileIndex;
        }

        public static void SetFileIndex(int fileIndex)
        {
            Reporter.fileIndex = fileIndex;
        }

        public static void IncreaseFileIndex()
        {
            Reporter.fileIndex += 1;
        }

        public static TestFileResult GetFileResult()
        {
            return fileResult;
        }

        public static void SetFileResult(TestFileResult fileResult)
        {
            Reporter.fileResult = fileResult;
        }

        public static Boolean IsResult()
        {
            return result;
        }

        public static void SetResult(Boolean result)
        {
            Reporter.result = result;
        }

        public static void StartTestReport()
        {
            try
            {
                String template = TestUtil.ReadTextFile(TestSetting.GetTemplatePath());
                template = template.Replace("$ENCODING$", TestSetting.GetTestDataEncode());
                summaryBuff.Append(template);
                summaryBuff.Append("<h1><center>" + Language.GetReportTitle() + "</center></h1>\n");
                testStartedDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
                fileHeader = TestUtil.SplitString(Language.GetFileHeader(), "," );
                summaryHeader = TestUtil.SplitString(Language.GetSummaryHeader(), ",");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static void StartBrowserReport()
        {
            try
            {
                summaryBuff.Append("\n<table>\n<tr>\n<table width=\"80%\" class=\"center\">\n" + "<tr>"
                + "<th colspan= \"7\" class = \"bgcolor_th\"><center>" + TestSetting.GetBrowserName()
                + "</center></th>" + "</tr>\n" + "<tr>" + "<th width=\"5%\" class = \"bgcolor_th\"><center>"
                + summaryHeader[0] + "</center></th>" + "<th class = \"bgcolor_th\"><center>" + summaryHeader[1]
                + "</center></th>" + "<th width=\"10%\"class = \"bgcolor_th\"><center>" + summaryHeader[2]
                + "</center></th>" + "<th width=\"10%\" class = \"bgcolor_th\"><center>" + summaryHeader[3]
                + "</center></th>" + "<th width=\"10%\" class = \"bgcolor_th\"><center>" + summaryHeader[4]
                + "</center></th>" + "<th width=\"10%\" class = \"bgcolor_th\"><center>" + summaryHeader[5]
                + "</center></th>" + "<th width=\"10%\" class = \"bgcolor_th\"><center>" + summaryHeader[6]
                + "</center></th>" + "</tr>");

                summaryLogBuff.Append("</tr>\n<tr>\n<table width=\"80%\" class=\"center\">\n" + "<tr>"
                        + "<th colspan= \"2\" class = \"bgcolor_th\"><center>LOGS</center></th>");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void StartFileReport()
        {
            try
            {
                String template = TestUtil.ReadTextFile(TestSetting.GetTemplatePath());
                template = template.Replace("$ENCODING$", TestSetting.GetTestDataEncode());
                fileBuff.Append(template);
                fileBuff.Append("<h1><center>" + Language.GetReportTitle() + "</center></h1>\n");

                fileBuff.Append("\n<table class=\"center\">\n" + "<tr>" + "<th colspan= \"6\" class = \"bgcolor_th\"><center>"
                        + TestSetting.GetBrowserName() + "</center></th>" + "</tr>\n" + "<tr>"
                        + "<th width=\"3%\" class = \"bgcolor_th\"><center>" + fileHeader[0] + "</center></th>"
                        + "<th width=\"10%\"class = \"bgcolor_th\"><center>" + fileHeader[1] + "</center></th>"
                        + "<th width=\"3%\"class = \"bgcolor_th\"><center>" + fileHeader[2] + "</center></th>"
                        + "<th width=\"20%\" class = \"bgcolor_th\"><center>" + fileHeader[3] + "</center></th>"
                        + "<th width=\"20%\" class = \"bgcolor_th\"><center>" + fileHeader[4] + "</center></th>"
                        + "<th class = \"bgcolor_th\"><center>" + fileHeader[5] + "</center></th>" + "</tr>");
                fileStartedDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
                SetRowIndex(0);
                IncreaseFileIndex();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void StartTestCaseReport()
        {
            try
            {
                SetRowSpanIndex(-1);
                SetRowSpanNumber(0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void EndTestCaseReport()
        {
            try
            {
                IncreaseRowIndex();
                SetTestCaseBuff(GetTestCaseBuff().Replace("$$$", "" + SetRowSpanNumber()));
                fileBuff.Append(GetTestCaseBuff());
                SetTestCaseBuff("");
                if (IsResult())
                {
                    GetFileResult().IncreasePassedTc();
                }
                else
                {
                    GetFileResult().IncreaseErrorTc();
                }

                SetResult(true);
                if (GetRowIndex() >= TestCaseExecution.GetTotalTestCases())
                {
                    SetRowIndex(0);
                    fileBuff.Append("\n</table>\n" + "<br/><br/>");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static void TestCaseReport(IWebDriver driver, String actualValue, String expectedValue, String userMessage)
        {
            try
            {
                if (userMessage == null)
                {
                    userMessage = "";
                }
                TestCaseResult obj = new TestCaseResult();
                obj.SetIndex("" + rowIndex);
                obj.SetTestCaseName(GetTestcaseName(rowIndex));
                obj.SetUserMessage(userMessage);
                if (("OK".Equals(actualValue)) && ("OK".Equals(expectedValue)))
                {
                    obj.SetSystemMessage(Language.GetEndTestMessage());
                    obj.SetResult("OK");
                }

                String messageOK = Language.GetReportTitle() + "[" + actualValue + "]" + Language.GetAssertOK();
                String messageNG = Language.GetReportTitle() + "[" + actualValue + "]" + Language.GetAssertNG() + "[" + expectedValue + "]";



                if (actualValue.Equals(expectedValue))
                {
                    obj.SetSystemMessage(messageOK);
                    obj.SetResult("OK");
                }
                else
                {
                    obj.SetSystemMessage(messageNG);
                    throw new Exception(obj.GetSystemMessage());
                }

                IncreaseRowSpanNumber();
                if (rowIndex != GetRowSpanIndex())
                {
                    SetRowSpanIndex(rowIndex);
                    obj.SetRowSpan(true);
                }
                else
                {
                    obj.SetRowSpan(false);
                }
                obj.SetFileLink(GetImageLink(driver));
                testCaseBuff += obj.GetReport();
                SetResult(true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void TestCaseErrorReport(IWebDriver driver, String systemMessage, String userMessage)

        {
            try
            {
                if (userMessage == null)
                {
                    userMessage = "";
                }
                TestCaseResult obj = new TestCaseResult();
                obj.SetIndex("" + rowIndex);
                obj.SetTestCaseName(GetTestcaseName(rowIndex));
                obj.SetUserMessage(userMessage);
                obj.SetSystemMessage(systemMessage);
                obj.SetResult("NG");

                IncreaseRowSpanNumber();
                if (rowIndex != GetRowSpanIndex())
                {
                    SetRowSpanIndex(rowIndex);
                    obj.SetRowSpan(true);
                }
                else
                {
                    obj.SetRowSpan(false);
                }
                obj.SetFileLink(GetImageLink(driver));
                testCaseBuff += obj.GetReport();

                SetResult(false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void EndFileReport(String testFilePath)
        {
            try
            {
                if (testFilePath == null || testFilePath.Trim().Length == 0)
                {
                    return;
                }
                String testFileName = GetfileName(testFilePath);
                String baseFolder = TestSetting.GetResultFolder() + @"\" + TestSetting.GetResultSubFolder() + @"\" + GetBaseFolder(testFilePath);
                if (!Directory.Exists(baseFolder))
                {
                    Directory.CreateDirectory(baseFolder);
                }

                String fileName = Language.GetReportTitle() + "_" + testFileName + "_" + fileStartedDate + ".html";
                String reportPath = baseFolder + @"\" + fileName;
                fileBuff.Append("\n</body>\n</html>");
                TestUtil.WriteTextFile(reportPath, fileBuff.ToString());
                fileBuff = new StringBuilder();

                String logFileName = "TestResult_" + testFileName + "_" + fileStartedDate + ".log";
                String logFilePath = baseFolder + @"\" + logFileName;
                TestUtil.WriteTextFile(logFilePath, logBuffer.ToString());
                logBuffer = new StringBuilder();

                String linkFolder = TestSetting.GetResultFolder() + @"\" + TestSetting.GetResultSubFolder();
                String link = reportPath.Replace(linkFolder + @"\", "");
                fileResult.SetIndex("" + fileIndex);
                fileResult.SetFileLink(link);
                fileResult.SetFileName(link);
                summaryBuff.Append(fileResult.GetReport());

                String linkLog = logFilePath.Replace(linkFolder + @"\", "");
                summaryLogBuff.Append("\n<tr><td width=\"5%\" class=\"OK\"><center>" + (fileIndex) + "</center></td>"
                            + "<td class=\"OK\"><a href=\"" + linkLog + "\">" + linkLog + "</a></td></tr>");

                totalFileResult.Acclerator(fileResult);
                fileResult = new TestFileResult();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void EndBrowserReport()
        {
            try
            {
                if (totalFileResult == null)
                {
                    totalFileResult = new TestFileResult();
                }
                totalFileResult.SetIndex("Total");
                totalFileResult.SetFileLink("");
                totalFileResult.SetFileName("");
                summaryBuff.Append(totalFileResult.GetReport());
                totalFileResult = new TestFileResult();
                summaryBuff.Append("\n</table>");
                summaryLogBuff.Append("\n</table></tr>\n</table>\n<br/>\n<br/>");
                summaryBuff.Append(summaryLogBuff.ToString());
                summaryLogBuff = new StringBuilder();
                SetFileIndex(0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void EndTestReport()
        {
            try
            {
                String reportPath = TestSetting.GetResultFolder() + @"\" + TestSetting.GetResultSubFolder() + @"\" + Language.GetReportTitle() + "_" + testStartedDate + ".html";
                summaryBuff.Append("\n</body>\n</html>");
                TestUtil.WriteTextFile(reportPath, summaryBuff.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static String GetImageLink(IWebDriver driver)
        {
            try
            {
                String fileName = GetfileName(TestSetting.GetCurrentFilePath());
                String baseFolder = TestSetting.GetResultFolder() + @"\" + TestSetting.GetResultSubFolder() + @"\" + GetBaseFolder(TestSetting.GetCurrentFilePath());
                if (!Directory.Exists(baseFolder))
                {
                    Directory.CreateDirectory(baseFolder);
                }
                String subFolder = Language.GetReportTitle() + "_" + fileName + "_" + fileStartedDate;
                if (!Directory.Exists(baseFolder + @"\" + subFolder))
                {
                    Directory.CreateDirectory(baseFolder + @"\" + subFolder);
                }
                String imgFile = subFolder + @"\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
                try
                {
                    Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                    image.SaveAsFile(baseFolder + @"\" + imgFile, ScreenshotImageFormat.Png);
                }
                catch (Exception e)
                {
                    SaveLog("<WARNING> screen shot error : " + e.Message);
                }

                imgFile = "<td><a href=\"" + imgFile + "\"><img width=\"100%\"  src=\"" + imgFile + "\" /></a></td>";
                return imgFile;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // get file name
        private static String GetfileName(String filePath)
        {
            if ((filePath == null) || filePath.Trim().Length == 0)
            {
                return "";
            }
            filePath = filePath.Replace("/", @"\");
            char[] chars = filePath.ToCharArray();
            char backwordSlash = '\\';
            int startPost = 0;
            if (chars != null)
            {
                for (int i = chars.Length - 1; i >= 0; i--)
                {
                    if (chars[i] == backwordSlash)
                    {
                        startPost = i;
                        break;
                    }
                }
            }
            return filePath.Substring(startPost + 1);
        }

        private static String GetTestcaseName(int index)
        {
            String name = "";
            if (TestCaseExecution.GetNameList() != null && TestCaseExecution.GetNameList().Count > index)
            {
                name = TestCaseExecution.GetNameList()[index];
            }
            return name;
        }

        public static void SaveLog(String message)
        {
            Console.WriteLine(message);
            logBuffer.Append(message + "\n");
        }

        public static String GetBaseFolder(String testFilePath)
        {
            String baseFolder = TestSetting.GetResultFolder();
            if (testFilePath == null || testFilePath.Trim().Length == 0)
            {
                return null;
            }

            if (baseFolder == null || baseFolder.Trim().Length == 0)
            {
                return null;
            }
            String result = null;
            testFilePath = testFilePath.Replace("/", @"\");
            String testFileName = GetfileName(testFilePath);
            if (testFilePath.Contains(baseFolder))
            {
                result = testFilePath.Replace(baseFolder + @"\", ""); ;
                result = result.Replace(@"\" + testFileName, "");
            }
            else
            {
                result = testFileName;
            }
            return result;
        }
    }
}
