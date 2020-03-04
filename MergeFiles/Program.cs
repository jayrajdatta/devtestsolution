using System;
using System.IO;

namespace MergeFiles
{
    class Program
    {
        //Merge file is stored in drive
        string SaveFileFolder = @"E:\sdk\";
        static void Main(string[] args)
        {
            Console.WriteLine("Merge File Program Started");

            string splitfilepath = @"E:\sdk\split\";
            Program obj = new Program();
            obj.MergeFile(splitfilepath);

            Console.ReadLine();
        }

        //merge files
        public bool MergeFile(string inputfoldername1)
        {
            bool Output = false;

            try
            {
                string[] tmpfiles = Directory.GetFiles(inputfoldername1, "*.txt");

                FileStream outPutFile = null;
                string PrevFileName = "";

                foreach (string tempFile in tmpfiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(tempFile);
                    string baseFileName = fileName.Substring(0, fileName.IndexOf(Convert.ToChar("-")));
                    string extension = "txt"; //Path.GetExtension(fileName);

                    if (!PrevFileName.Equals(baseFileName))
                    {
                        if (outPutFile != null)
                        {
                            outPutFile.Flush();
                            outPutFile.Close();
                        }
                        outPutFile = new FileStream(SaveFileFolder + "\\" + baseFileName + "." + extension, FileMode.OpenOrCreate, FileAccess.Write);

                    }

                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];
                    FileStream inputTempFile = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Read);

                    while ((bytesRead = inputTempFile.Read(buffer, 0, 1024)) > 0)
                        outPutFile.Write(buffer, 0, bytesRead);

                    inputTempFile.Close();
                    //File.Delete(tempFile);
                    PrevFileName = baseFileName;

                }

                outPutFile.Close();
                Console.WriteLine("Files have been merged and saved ");
            }
            catch
            {

            }

            return Output;

        }
    }
}
