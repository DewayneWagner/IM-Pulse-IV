using IM_Pulse_IV.Models.Main;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace IM_Pulse_IV.IO
{
    public class SavedRandomDataParameters_JSON
    {
        public static RandomDataParameters GetSavedData()
        {            
            string filePath = App.ListOfAllPaths[(int)App.FileNames.RandomDataParameters];
            string readData = null;

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    readData = sr.ReadToEnd();
                    sr.Close();
                }
                if (String.IsNullOrEmpty(readData)) { return getDefaultRandomDataParameter(); }
                else { return JsonConvert.DeserializeObject<RandomDataParameters>(readData); }                
            }
            catch
            {
                return getDefaultRandomDataParameter();
            }

            RandomDataParameters getDefaultRandomDataParameter()
            {
                return new RandomDataParameters()
                {
                    DeLimiter = "/",
                    MaxNumberOfCommandsPerSegment = 10,
                    MinNumberOfCommandsPerSegment = 10,
                    SegmentLength = 2500,
                    NumberOfSegments = 4,
                    IsPulseVsContinuous = true,
                    RandomTextGenerationFrequency = 5000,
                };
            }
        }
        public static void WriteRandomDataParametersToFile()
        {
            RandomDataParameters currentRandomDataParameters = (RandomDataParameters)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)];
            string output = JsonConvert.SerializeObject(currentRandomDataParameters);
            string filePath = App.ListOfAllPaths[(int)App.FileNames.RandomDataParameters];
            
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(output);
                sw.Close();
            }
        }
    }
}
