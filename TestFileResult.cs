using System;

namespace drivers
{
    public class TestFileResult
    {
        private String index = "";
        private String fileLink = "";
        private String fileName = "";
        private int totalTc = 0;
        private int mistakeTc = 0;
        private int errorTc = 0;
        private int passedTc = 0;
        private int skipTc = 0;

        public String GetIndex()
        {
            return index;
        }

        public void SetIndex(String index)
        {
            this.index = index;
        }

        public String GetFileLink()
        {
            return fileLink;
        }

        public void SetFileLink(String fileLink)
        {
            this.fileLink = fileLink;
        }

        public String GetFileName()
        {
            return fileName;
        }

        public void SetFileName(String fileName)
        {
            this.fileName = fileName;
        }

        public int GetTotalTc()
        {
            return totalTc;
        }

        public void SetTotalTc(int totalTc)
        {
            this.totalTc = totalTc;
        }

        public int GetMistakeTc()
        {
            return mistakeTc;
        }

        public void SetMistakeTc(int mistakeTc)
        {
            this.mistakeTc = mistakeTc;
        }

        public int GetErrorTc()
        {
            return errorTc;
        }

        public void SetErrorTc(int errorTc)
        {
            this.errorTc = errorTc;
        }

        public int GetPassedTc()
        {
            return passedTc;
        }

        public void SetPassedTc(int passedTc)
        {
            this.passedTc = passedTc;
        }

        public int GetSkipTc()
        {
            return skipTc;
        }

        public void SetSkipTc(int skipTc)
        {
            this.skipTc = skipTc;
        }

        public void Acclerator(TestFileResult obj)
        {
            this.totalTc += obj.GetTotalTc();
            this.mistakeTc += obj.GetMistakeTc();
            this.errorTc += obj.GetErrorTc();
            this.passedTc += obj.GetPassedTc();
            this.skipTc += obj.GetSkipTc();
        }

        public void IncreasePassedTc()
        {
            this.passedTc += 1;
        }

        public void IncreaseErrorTc()
        {
            this.errorTc += 1;
        }

        public void IncreaseMistakeTc()
        {
            this.mistakeTc += 1;
        }

        public String GetReport()
        {
            this.SetSkipTc(this.GetTotalTc() - this.GetMistakeTc() - this.GetPassedTc() - this.GetErrorTc());
            String mistake = "";
            if (this.GetMistakeTc() > 0)
            {
                mistake = "<td class=\"NG\"><center>" + this.GetMistakeTc() + "</center></td>";
            }
            else
            {
                mistake = "<td class=\"OK\"><center>" + this.GetMistakeTc() + "</center></td>";
            }

            String error = "";
            if (this.GetErrorTc() > 0)
            {
                error = "<td class=\"NG\"><center>" + this.GetErrorTc() + "</center></td>";
            }
            else
            {
                error = "<td class=\"OK\"><center>" + this.GetErrorTc() + "</center></td>";
            }

            String skip = "";
            if (this.GetSkipTc() > 0)
            {
                skip = "<td class=\"NG\"><center>" + this.GetSkipTc() + "</center></td>";
            }
            else
            {
                skip = "<td class=\"OK\"><center>" + this.GetSkipTc() + "</center></td>";
            }

            return ("\n<tr><td class=\"OK\"><center>" + index + "</center></td>" + "<td class=\"OK\"><a href=\""
                    + this.fileLink + "\">" + this.fileName + "</a></td>" + "<td class=\"OK\"><center>" + this.GetTotalTc()
                    + "</center></td>" + mistake + "<td class=\"OK\"><center>" + this.GetPassedTc() + "</center></td>"
                    + error + skip + "</tr>");
        }
    }
}
