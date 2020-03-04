using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SplitFile
{
    class Program
    {
        List<string> Packets = new List<string>();
        public FileStream fs;
        string mergeFolder;
        static void Main(string[] args)
        {
            Console.WriteLine("Split File Program Started");

            Program obj = new Program();
            string filepath = "E://sdk/salesorders.txt";
            //obj.SplitFile(filepath, 3);
            var lineCount = File.ReadLines(@"E://sdk/salesorders.txt").Count();
            int splitSize = (int)Math.Ceiling((double)lineCount / 5);
            using (var lineIterator = File.ReadLines("E://sdk/salesorders.txt").GetEnumerator())
            {
                bool stillGoing = true;
                for (int chunk = 0; stillGoing; chunk++)
                {
                    stillGoing = WriteChunk(lineIterator, splitSize, chunk);
                }
            }
            Console.ReadLine();
        }

        private static bool WriteChunk(IEnumerator<string> lineIterator,
                               int splitSize, int chunk)
        {
            using (var writer = File.CreateText("E://sdk/file-" + chunk + ".txt"))
            {
                for (int i = 0; i < splitSize; i++)
                {
                    if (!lineIterator.MoveNext())
                    {
                        return false;
                    }
                    writer.WriteLine(lineIterator.Current);
                }
            }
            return true;
        }

        //split files
        public bool SplitFile(string SourceFile, int nNoofFiles)
        {
            bool Split = false;
            try
            {
                FileStream fs = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
                int SizeofEachFile = (int)Math.Ceiling((double)fs.Length / nNoofFiles);

                for (int i = 0; i < nNoofFiles; i++)
                {
                    string baseFileName = Path.GetFileNameWithoutExtension(SourceFile);
                    string Extension = Path.GetExtension(SourceFile);

                    FileStream outputFile = new FileStream(Path.GetDirectoryName(SourceFile) + "\\" + baseFileName + "." +
                        i.ToString().PadLeft(5, Convert.ToChar("0")) + Extension + ".tmp", FileMode.Create, FileAccess.Write);

                    mergeFolder = Path.GetDirectoryName(SourceFile);

                    int bytesRead = 0;
                    byte[] buffer = new byte[SizeofEachFile];

                    if ((bytesRead = fs.Read(buffer, 0, SizeofEachFile)) > 0)
                    {
                        outputFile.Write(buffer, 0, bytesRead);
                        //outp.Write(buffer, 0, BytesRead);

                        string packet = baseFileName + "." + i.ToString().PadLeft(3, Convert.ToChar("0")) + Extension.ToString();
                        Packets.Add(packet);
                    }

                    outputFile.Close();

                }
                fs.Close();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

            return Split;
        }
    }
}
