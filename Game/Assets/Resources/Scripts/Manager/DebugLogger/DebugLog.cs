using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Manager
{
    public class DebugLog
    {
        private string FilePath { get; set; }
        public delegate void WriteInLogDelegate(List<string> messages);
        public WriteInLogDelegate SetDataPath(string _filePath)
        {
            FilePath = _filePath;
            if (File.Exists(FilePath))
            {
                try
                {
                    File.Delete(FilePath);
                }
                catch
                {
                    throw new System.Exception();
                }
            }
            return WriteToFile;
        }
        public void WriteToFile(List<string> messages)
        {
            try
            {
                StreamWriter FileWriter = new StreamWriter(FilePath, true);
                FileWriter.Write("Time: " + Time.fixedTime.ToString());
                foreach(string message in messages)
                {
                    FileWriter.Write(" | " + message);
                }
                FileWriter.Write("\n");
                FileWriter.Close();
            }
            catch
            {
                throw new System.Exception();
            }
        }
    }
}
