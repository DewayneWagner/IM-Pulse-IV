using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IM_Pulse_IV.Services
{
    public class ListOfAllPaths : List<string>
    {
        private string[] _fileNamesArray;
        public ListOfAllPaths(string folderPath)
        {
            _fileNamesArray = new string[(int)App.FileNames.Total];
            LoadFileNamesList();
            CreatePathList(folderPath);
        }
        private void LoadFileNamesList()
        {
            _fileNamesArray[(int)App.FileNames.DB] = "ImPulseIV_DB.sqlite";
            _fileNamesArray[(int)App.FileNames.RandomDataParameters] = "RandomDataParameters.json";
            _fileNamesArray[(int)App.FileNames.RandomTextData] = "RandomTextData.txt";
        }
        private void CreatePathList(string folderPath)
        {
            for (int i = 0; i < (int)App.FileNames.Total; i++)
            {
                string filePath = Path.Combine(folderPath, _fileNamesArray[i]);
                this.Add(filePath);
            }
        }
    }
}
