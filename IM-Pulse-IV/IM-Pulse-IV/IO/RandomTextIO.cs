using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IM_Pulse_IV.IO
{
    class RandomTextIO
    {
        public static string ReadTextData()
        {
            string filePath = App.ListOfAllPaths[(int)App.FileNames.RandomTextData];
            string allData = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                allData = sr.ReadToEnd();
            }
            return allData;
        }
        public static void WriteTextData(string dataToWrite, bool appendToOldData)
        {
            string filePath = App.ListOfAllPaths[(int)App.FileNames.RandomTextData];

            using (StreamWriter sw = new StreamWriter(filePath, !appendToOldData))
            {
                sw.Write(dataToWrite);
                sw.Close();
            }
        }
    }
}
