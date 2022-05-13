using System;
using System.IO;
using System.Collections.Generic;

namespace drivers
{
    public class RefFolder
    {

        List<String> filePaths = new List<String>();

        public List<String> GetFilePaths()
        {
            return filePaths;
        }

        public List<String> GetAllFiles(String fileName)
        {
            if (File.Exists(TestUtil.GetFullPath(fileName)))
            {
                this.filePaths.Add(TestUtil.GetFullPath(fileName));
            }
            else
            {
                GetAllFilesInDirectory(fileName);
            }
            return filePaths;
        }

        public void GetAllFilesInDirectory(String directoryName)
        {
            // directory
            if (Directory.Exists(directoryName))
            {
                // get all the files from a directory
                String[] fList = Directory.GetFiles(directoryName);
                if (fList != null)
                {
                    foreach (String file in fList)
                    {
                        if (file != null)
                        {
                            if (File.Exists(TestUtil.GetFullPath(file)))
                            {
                                this.filePaths.Add(TestUtil.GetFullPath(file));
                            }
                            else if (Directory.Exists(file))
                            {
                                GetAllFilesInDirectory(file);
                            }
                        }
                    }
                }
                // get all sub directorys
                String[] dList = Directory.GetDirectories(directoryName);
                if (dList != null)
                {
                    foreach (String dir in dList)
                    {
                        if (dir != null)
                        {
                            GetAllFilesInDirectory(dir);
                        }
                    }
                }
            }
        }
    }
}
