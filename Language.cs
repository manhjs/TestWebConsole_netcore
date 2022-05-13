using System;
using System.Collections.Generic;

namespace drivers
{
    class Language
    {
        // Report
        private static String ReportTitle = "Test Result";
        private static String AssertOK = @"is matched to the expected value";
        private static String AssertNG = @"is not matched to the expected value";
        private static String EndTestMessage = @"Finished execution";
        private static String FileHeader = @"Index, Testcase, Result, Message, Comment, Evidence";
        private static String SummaryHeader = @"No., File Name, Total, Mistake, OK, NG, Remain";

        public static String GetReportTitle()
        {
            return Language.ReportTitle;
        }
        public static void SetReportTitle(String title)
        {
            Language.ReportTitle = title;
        }
        public static String GetAssertOK()
        {
            return Language.AssertOK;
        }
        public static void SetAssertOK(String assert)
        {
            Language.AssertOK = assert;
        }

        public static String GetAssertNG()
        {
            return Language.AssertNG;
        }
        public static void SetAssertNG(String assert)
        {
            Language.AssertNG = assert;
        }

        public static String GetEndTestMessage()
        {
            return Language.EndTestMessage;
        }
        public static void SetEndTestMessage(String message)
        {
            Language.EndTestMessage = message;
        }
        public static String GetFileHeader()
        {
            return Language.FileHeader;
        }
        public static void SetFileHeader(String header)
        {
            Language.FileHeader = header;
        }

        public static String GetSummaryHeader()
        {
            return Language.SummaryHeader;
        }
        public static void SetSummaryHeader(String header)
        {
            Language.SummaryHeader = header;
        }
    }
}
