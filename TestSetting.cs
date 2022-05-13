using System;
using System.Collections.Generic;

namespace drivers
{
    class TestSetting
    {
        private static String Language = "English";
        private static String EachStepDelay = "";
        private static String WaitForAlert = "";
        private static String TestDataEncode = "UTF-8";
        private static String ShowMouse = "";
        private static Boolean ShowMouseError = false;
        private static String BrowserName = "";
        private static String StopErrorFile = "";
        private static String CurrentFilePath = "";
        private static String ClassName = "";
        private static String MethodName = "";
        private static Dictionary<String, String> Locators = null;
        private static List<String> FilePaths = null;
        private static List<String> Commands = null;
        private static readonly String SettingFile = "TestSetting.txt";
        private static readonly String SettingClass = "drivers.TestSetting";
        private static String ResultFolder = "";
        private static readonly String ResultSubFolder = "results";
        private static readonly String DriversFolder = "drivers";
        private static readonly String LanguageFile = "Language.txt";
        private static readonly String LanguageClass = "drivers.Language";
        private static readonly String LanguageFolder = "languages";
        private static readonly String TemplatePath = "template/ReportTemplate.html";
        private static readonly String LOCATORS_KEY = "LOCATORS";
        private static readonly String COMMANDS_KEY = "COMMANDS";
        private static readonly String SETTING_KEY = "SETTING";
        private static readonly String BROWSERS_KEY = "BROWSERS";
        private static readonly String IMPORT_KEY = "IMPORT";

        public static String GetLanguage()
        {
            return Language;
        }
        public static void SetLanguage(String language)
        {
            TestSetting.Language = language;
        }
        public static String GetEachStepDelay()
        {
            return EachStepDelay;
        }

        public static void SetEachStepDelay(String eachStepDelay)
        {
            TestSetting.EachStepDelay = eachStepDelay;
        }

        public static String GetWaitForAlert()
        {
            return WaitForAlert;
        }

        public static void SetWaitForAlert(String waitForAlert)
        {
            TestSetting.WaitForAlert = waitForAlert;
        }

        public static String GetTestDataEncode()
        {
            if (TestDataEncode == null || TestDataEncode.Trim().Length == 0)
            {
                return "UTF-8";
            }
            else
            {
                return TestDataEncode;
            }
        }

        public static void SetTestDataEncode(String testDataEncode)
        {
            TestSetting.TestDataEncode = testDataEncode;
        }

        public static String GetStopErrorFile()
        {
            return StopErrorFile;
        }

        public static void SetStopErrorFile(String stopErrorFile)
        {
            TestSetting.StopErrorFile = stopErrorFile;
        }

        public static String GetCurrentFilePath()
        {
            return CurrentFilePath;
        }

        public static void SetCurrentFilePath(String filePath)
        {
            TestSetting.CurrentFilePath = filePath;
        }

        public static List<String> GetFilePaths()
        {
            return FilePaths;
        }

        public static void SetFilePaths(List<String> filePaths)
        {
            TestSetting.FilePaths = filePaths;
        }

        public static String GetClassName()
        {
            return ClassName;
        }

        public static void SetClassName(String className)
        {
            TestSetting.ClassName = className;
        }

        public static String GetMethodName()
        {
            return MethodName;
        }

        public static void SetMethodName(String methodName)
        {
            TestSetting.MethodName = methodName;
        }

        public static String GetShowMouse()
        {
            return ShowMouse;
        }

        public static void SetShowMouse(String showMouse)
        {
            TestSetting.ShowMouse = showMouse;
        }

        public static Boolean IsShowMouseError()
        {
            return ShowMouseError;
        }

        public static void SetShowMouseError(Boolean showMouseError)
        {
            TestSetting.ShowMouseError = showMouseError;
        }

        public static String GetBrowserName()
        {
            return BrowserName;
        }

        public static void SetBrowserName(String browserName)
        {
            TestSetting.BrowserName = browserName;
        }

        public static Dictionary<String, String> GetLocators()
        {
            return Locators;
        }

        public static void SetLocators(Dictionary<String, String> locators)
        {
            TestSetting.Locators = locators;
        }

        public static List<String> GetCommands()
        {
            return Commands;
        }

        public static void SetCommands(List<String> commands)
        {
            TestSetting.Commands = commands;
        }

        public static String GetSettingFile()
        {
            return SettingFile;
        }

        public static String GetSettingclass()
        {
            return SettingClass;
        }

        public static String GetResultFolder()
        {
            return ResultFolder;

        }

        public static void SetResultFolder(String folder)
        {
            TestSetting.ResultFolder = folder;
        }

        public static String GetResultSubFolder()
        {
            return ResultSubFolder;

        }

        public static String GetDriversFolder()
        {
            return DriversFolder;

        }

        public static String GetLanguageFile()
        {
            return LanguageFile;
        }

        public static String GetLanguageClass()
        {
            return LanguageClass;
        }

        public static String GetLanguageFolder()
        {
            return LanguageFolder;
        }

        public static String GetTemplatePath()
        {
            return TemplatePath;
        }

        public static String GetLocatorsKey()
        {
            return LOCATORS_KEY;
        }

        public static String GetCommandsKey()
        {
            return COMMANDS_KEY;
        }

        public static String GetSettingKey()
        {
            return SETTING_KEY;
        }

        public static String GetBrowsersKey()
        {
            return BROWSERS_KEY;
        }

        public static String GetImportKey()
        {
            return IMPORT_KEY;
        }

    }
}
