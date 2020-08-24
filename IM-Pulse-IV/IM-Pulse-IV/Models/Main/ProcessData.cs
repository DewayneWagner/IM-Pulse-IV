using IM_Pulse_IV.IO;
using IM_Pulse_IV.Models.RandomDataGenerator;
using IM_Pulse_IV.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IM_Pulse_IV.Models.Main
{
    class ProcessData
    {
        string _rawData;
        string[] _dataSegments;
        RandomDataParameters _randomDataParameters;
        List<char> _listOfCommandParameters;
        List<ReadData> _readDataList;

        public ProcessData()
        {
            _randomDataParameters = (RandomDataParameters)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)];            
            _rawData = RandomTextIO.ReadTextData();
            SeparateSegments();
            SetListOfCommandStrings();            
            ProcessDataString();
            AddReadDataToRandomDataStatsList();
            AddReadDataToDB();
        }
        void SeparateSegments()
        {
            char[] delimiter = _randomDataParameters.DeLimiter.ToCharArray();
            char d = delimiter[0];
            _dataSegments = _rawData.Split(d);
            List<int> segmentLength = new List<int>();

            for (int i = 0; i < _dataSegments.Length; i++)
            {
                segmentLength.Add(_dataSegments[i].Length);
            }
        }
        void SetListOfCommandStrings()
        {
            _listOfCommandParameters = new List<char>();
            using (DBContext db = new DBContext())
            {
                List<string> cpList = db.CommandParameters.Select(c => c.CommandName).ToList();
                foreach (string cp in cpList)
                {
                    char[] cA = cp.ToCharArray();
                    _listOfCommandParameters.Add(cA[0]);
                }
            }
        }
        void ProcessDataString()
        {
            _readDataList = new List<ReadData>();

            for (int seg = 0; seg < _dataSegments.Length - 1; seg++)
            {
                char[] c = _dataSegments[seg].ToCharArray();
                addteststrings();

                for (int ii = 0; ii < c.Length; ii++)
                {
                    ReadData rd = new ReadData()
                    {
                        Data = c[ii],
                        SegmentID = seg,
                        Index = ii,
                    };

                    if (_listOfCommandParameters.Contains(rd.Data))
                    {
                        rd.IsCommand = true;
                    }

                    _readDataList.Add(new ReadData
                    {
                        Data = c[ii],
                        SegmentID = seg,
                        Index = ii,
                        IsCommand = _listOfCommandParameters.Contains(c[ii]) ? true : false,
                    });
                }
                void addteststrings()
                {
                    for (int i = 0; i < _listOfCommandParameters.Count; i++)
                    {
                        c[i] = _listOfCommandParameters[i];
                    }
                }
            }            
        }
        void AddReadDataToRandomDataStatsList()
        {
            List<RandomDataStats> randomDataStatsList = (List<RandomDataStats>)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataStats)];
            List<ReadData> list = _readDataList.Where(c => c.IsCommand).ToList();

            foreach(ReadData item in list)
            {
                randomDataStatsList.Add(new RandomDataStats
                {
                    CommandParameter = item.Data.ToString(),
                    Index = item.Index,
                    SegmentID = item.SegmentID,
                    IsReadVsInsert = true,
                });
            }
        }
        void AddReadDataToDB()
        {
            using (DBContext db = new DBContext())
            {
                db.AddRange(_readDataList);
                db.SaveChanges();
            }
        }
        
    }
}
