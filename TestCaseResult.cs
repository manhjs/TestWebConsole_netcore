using System;

namespace drivers
{
    public class TestCaseResult
    {

        private String index = "";
        private String testCaseName = "";
        private String result;
        private String systemMessage;
        private String userMessage = "";
        private String fileLink = "";
        private Boolean rowSpan = false;

        public String GetIndex()
        {
            return index;
        }

        public void SetIndex(String index)
        {
            this.index = index;
        }

        public String SetTestCaseName()
        {
            return testCaseName;
        }

        public void SetTestCaseName(String name)
        {
            this.testCaseName = name;
        }

        public String GetUserMessage()
        {
            return userMessage;
        }

        public void SetUserMessage(String message)
        {
            this.userMessage = message;
        }

        public String GetFileLink()
        {
            return fileLink;
        }

        public void SetFileLink(String fileLink)
        {
            this.fileLink = fileLink;
        }

        public String GetResult()
        {
            return result;
        }

        public void SetResult(String result)
        {
            this.result = result;
        }

        public String GetSystemMessage()
        {
            return this.result;
        }

        public void SetSystemMessage(String message)
        {
            this.systemMessage = message;
        }

        public Boolean IsRowSpan()
        {
            return rowSpan;
        }

        public void SetRowSpan(Boolean rowSpan)
        {
            this.rowSpan = rowSpan;
        }

        public String GetReport()
        {
            String buff = "";

            if (IsRowSpan())
            {
                buff += "\n<tr><td class=\"" + this.GetResult() + "\" rowspan=\"$$$\"><center>" + this.index
                        + "</center></td>";
                buff += "<td class=\"" + this.GetResult() + "\" rowspan=\"$$$\">" + this.testCaseName + "</td>";
            }
            buff += "<td class=\"" + this.GetResult() + "\"><center>" + this.GetResult() + "</center></td>";
            buff += "</td><td class=\"" + this.GetResult() + "\">" + this.systemMessage + "</td>";
            buff += "</td><td class=\"" + this.GetResult() + "\">" + this.userMessage + "</td>";
            buff += "</td>" + this.fileLink + "</tr>";

            return buff;
        }

    }
}
