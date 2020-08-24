using IM_Pulse_IV.IO;
using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace IM_Pulse_IV.Models.RandomDataGenerator
{
    class RandomTextGenerator
    {
        private RandomDataParameters _currentRandomDataParameters;
        private List<string> _allUniqueTextParameters;
        private List<string> _allCommandParameters;
        private List<RandomDataStats> _randomDataStatsList;
        System.Random rnd = new System.Random();
        private List<string> _listOfSegments;
        private string _dataToWrite = null;

        public RandomTextGenerator(bool isFirstWrite = true)
        {
            _currentRandomDataParameters = (RandomDataParameters)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)];
            _randomDataStatsList = (List<RandomDataStats>)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataStats)];
            SetListOfAllUniqueTextParametersAndCommandStrings();
            ScrubDelimitersAndCommandParameters();
            SetListOfRandomDataStats();
            GenerateRandomTextString();
            CombineSegments();
            WriteRandomStatsToDB();
            RandomTextIO.WriteTextData(_dataToWrite, isFirstWrite);
        }

        private void GenerateRandomTextString()
        {
            if(_listOfSegments == null) { initList(); }

            for (int seg = 0; seg < _currentRandomDataParameters.NumberOfSegments; seg++)
            {
                List<RandomDataStats> sortedList = _randomDataStatsList.Where(s => s.SegmentID == seg)
                                    .OrderBy(s => s.Index)
                                    .ToList();

                RandomDataStats rds;
                int index = 0;

                for (int i = 0; i < _currentRandomDataParameters.SegmentLength; i++)
                {
                    if (sortedList.Any(s => s.Index == i) && i != 0)
                    {
                        rds = sortedList.Where(s => s.Index == i).FirstOrDefault();
                        _listOfSegments[seg] += rds.CommandParameter;
                    }
                    else
                    {
                        index = rnd.Next(0, _allUniqueTextParameters.Count);
                        _listOfSegments[seg] += _allUniqueTextParameters[index];
                    }
                }
            }
            void initList()
            {                
                _listOfSegments = new List<string>();
                for (int i = 0; i < _currentRandomDataParameters.NumberOfSegments; i++)
                {
                    _listOfSegments.Add(null);
                }
            }
        }

        private void SetListOfRandomDataStats()
        {
            for (int i = 0; i < _currentRandomDataParameters.NumberOfSegments; i++)
            {
                for (int ii = 0; ii < _allCommandParameters.Count; ii++)
                {
                    int numberOfThisCommandInSegment = rnd.Next(_currentRandomDataParameters.MinNumberOfCommandsPerSegment,
                        _currentRandomDataParameters.MaxNumberOfCommandsPerSegment);

                    for (int iii = 0; iii < numberOfThisCommandInSegment; iii++)
                    {
                        int insertIndex = getInsertIndex();

                        _randomDataStatsList.Add(new RandomDataStats
                        {
                            SegmentID = i,
                            CommandParameter = _allCommandParameters[ii],
                            Index = insertIndex,
                            IsReadVsInsert = false,
                        });
                    }
                }
            }
            _randomDataStatsList.OrderBy(r => r.Index)
                .ThenBy(r => r.SegmentID);

            int getInsertIndex()
            {
                int index = 0;
                do
                {
                    index = rnd.Next(1, _currentRandomDataParameters.SegmentLength);
                } while (_randomDataStatsList.Any(r => r.Index == index));
                return index;
            }
        }
        private void SetListOfAllUniqueTextParametersAndCommandStrings()
        {
            using (DBContext db = new DBContext())
            {
                _allUniqueTextParameters = db.UniqueTextParameters.Select(u => u.Text).ToList();
                _allCommandParameters = db.CommandParameters.Select(c => c.CommandName).ToList();
            }
        }
        private void ScrubDelimitersAndCommandParameters()
        {
            if (_allUniqueTextParameters.Contains(_currentRandomDataParameters.DeLimiter))
            {
                _allUniqueTextParameters.Remove(_currentRandomDataParameters.DeLimiter);
            }

            for (int i = 0; i < _allCommandParameters.Count; i++)
            {
                if (_allUniqueTextParameters.Contains(_allCommandParameters[i]))
                {
                    _allUniqueTextParameters.Remove(_allCommandParameters[i]);
                }
            }
        }
        private void CombineSegments()
        {
            foreach (string segment in _listOfSegments)
            {
                _dataToWrite += segment;
                _dataToWrite += _currentRandomDataParameters.DeLimiter;
            }
        }
        private void WriteRandomStatsToDB()
        {
            using (DBContext db = new DBContext())
            {
                List<RandomDataStats> oldData = db.RandomDataStats.ToList();
                if (oldData.Count > 0) { db.RandomDataStats.RemoveRange(oldData); }
                db.RandomDataStats.AddRange(_randomDataStatsList);
                db.SaveChanges();
            }
        }
    }
}
