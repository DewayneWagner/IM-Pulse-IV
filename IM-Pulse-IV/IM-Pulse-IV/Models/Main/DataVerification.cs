﻿using IM_Pulse_IV.Models.RandomDataGenerator;
using IM_Pulse_IV.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IM_Pulse_IV.Models.Main
{
    class DataVerification
    {
        private RandomDataParameters _randomDataParameters;
        private int _maxNumberOfSegments;
        private List<CommandParameter> _commandParameterList;

        public DataVerification()
        {
            _randomDataParameters = (RandomDataParameters)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)];
            InitCommandParameterList();
        }
        
        public int SegmentID { get; set; }
        public string CommandParameter { get; set; }
        public int ReadIndex { get; set; }
        public int InsertIndex { get; set; }
        public bool IsAccurate { get; set; }

        private void InitCommandParameterList()
        {
            using (DBContext db = new DBContext())
            {
                _commandParameterList = db.CommandParameters.ToList();
            }
        }

        public ObservableCollection<DataVerification> GetOCOfAllData()
        {
            List<RandomDataStats> allData = (List<RandomDataStats>)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataStats)];
            ObservableCollection<DataVerification> dvOC = new ObservableCollection<DataVerification>();
            _maxNumberOfSegments = allData.Max(d => d.SegmentID) + 1;
            initDefaultList();
            checkIfAccurate();

            return dvOC;
            
            void initDefaultList()
            {
                addInsertedValuesToOC();
                addReadValuesToOC();

                void addInsertedValuesToOC()
                {
                    var insertList = allData.Where(d => !d.IsReadVsInsert)
                        .OrderBy(d => d.SegmentID)
                        .ThenBy(d => d.CommandParameter)
                        .ThenBy(d => d.Index)
                        .ToList();

                    foreach (RandomDataStats data in insertList)
                    {
                        dvOC.Add(new DataVerification
                        {
                            SegmentID = data.SegmentID,
                            CommandParameter = data.CommandParameter,
                            InsertIndex = data.Index,
                        });
                    }
                }
                void addReadValuesToOC()
                {
                    var readList = allData.Where(d => d.IsReadVsInsert)
                                        .OrderBy(d => d.SegmentID)
                                        .ThenBy(d => d.CommandParameter)
                                        .ThenBy(d => d.Index)
                                        .ToList();

                    List<DataVerification> thisDVList;
                    List<RandomDataStats> thisRDSList;
                    int adjustedIndex;

                    foreach (RandomDataStats data in readList)
                    {
                        for (int seg = 0; seg < _maxNumberOfSegments; seg++)
                        {
                            for (int com = 0; com < _commandParameterList.Count; com++)
                            {
                                thisDVList = dvOC.Where(d => d.SegmentID == seg)
                                                .Where(d => d.CommandParameter == _commandParameterList[com].CommandName)
                                                .OrderBy(d => d.InsertIndex)
                                                .ToList();

                                thisRDSList = readList.Where(d => d.SegmentID == seg)
                                                .Where(d => d.CommandParameter == _commandParameterList[com].CommandName)
                                                .OrderBy(d => d.Index)
                                                .ToList();

                                int smallestIndex = thisDVList.Count < thisRDSList.Count ? thisDVList.Count : thisRDSList.Count;
                                adjustedIndex = thisRDSList.Count > thisDVList.Count ? 1 : 0;

                                for (int index = 0; index < smallestIndex; index++)
                                {
                                    thisDVList[index].ReadIndex = thisRDSList[index + adjustedIndex].Index;
                                }
                            }
                        }
                    }
                }
            }
            void checkIfAccurate()
            {
                dvOC.OrderBy(d => d.CommandParameter)
                    .ThenBy(d => d.CommandParameter);

                foreach (DataVerification dv in dvOC)
                {
                    if(dv.ReadIndex == dv.InsertIndex) { dv.IsAccurate = true; }    
                }
            }
        }
    }
}
