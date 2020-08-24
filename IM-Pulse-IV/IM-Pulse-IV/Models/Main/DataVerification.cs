using IM_Pulse_IV.Models.RandomDataGenerator;
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
            addDataToList();
            //checkIfAccurate();

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
                    List<RandomDataStats> thisRandomDataStatsList;

                    foreach (RandomDataStats data in readList)
                    {
                        for (int seg = 0; seg < _maxNumberOfSegments; seg++)
                        {
                            for (int com = 0; com < _commandParameterList.Count; com++)
                            {
                                thisDVList = dvOC.Where(d => d.SegmentID == seg)
                                            .Where(d => d.CommandParameter == _commandParameterList[com].CommandName)
                                            .ToList();

                                thisRandomDataStatsList = allData.Where(d => d.SegmentID == seg)
                                            .Where(d => d.CommandParameter == _commandParameterList[com].CommandName)
                                            .ToList();

                                if (thisDVList != null || thisRandomDataStatsList != null)
                                {
                                    for (int index = 0; index < thisDVList.Count; index++)
                                    {
                                        if(index > thisDVList.Count || index > thisRandomDataStatsList.Count)
                                        {
                                            thisDVList[index].ReadIndex = thisRandomDataStatsList[index].Index;
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            void addDataToList()
            {
                foreach (RandomDataStats data in allData)
                {
                    DataVerification dv = dvOC.Where(d => d.CommandParameter == data.CommandParameter)
                                                .Where(d => d.SegmentID == data.SegmentID)
                                                .FirstOrDefault();

                    if (dv != null)
                    {
                        if (data.IsReadVsInsert) { dv.ReadIndex = data.Index; }
                        else { dv.InsertIndex = data.Index; }
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
