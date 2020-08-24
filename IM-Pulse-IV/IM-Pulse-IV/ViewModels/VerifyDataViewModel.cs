using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using TheManXS.ViewModel.Services;

namespace IM_Pulse_IV.ViewModels
{
    class VerifyDataViewModel : BaseViewModel
    {
        private Color _good = Color.LightGreen;
        private Color _bad = Color.Red;

        PageService _pageServices;
        public VerifyDataViewModel()
        {
            _pageServices = new PageService();

            DataVerificationOC = new DataVerification().GetOCOfAllData();
            ReadDataAccuracy = GetDataAccuracy();
            Instructions = GetInstructions();
        }

        private string _readDataAccuracy;
        public string ReadDataAccuracy
        {
            get => _readDataAccuracy;
            set
            {
                if (value == "100%") { DataAccuracyLabelBackgroundColor = _good; }
                else { DataAccuracyLabelBackgroundColor = _bad; }

                _readDataAccuracy = value;
                SetValue(ref _readDataAccuracy, value);
            }
        }

        private Color _dataAccuracyLabelBackgroundColor;
        public Color DataAccuracyLabelBackgroundColor
        {
            get => _dataAccuracyLabelBackgroundColor;
            set
            {
                _dataAccuracyLabelBackgroundColor = value;
                SetValue(ref _dataAccuracyLabelBackgroundColor, value);
            }
        }
        private ObservableCollection<DataVerification> _dataVerificationOC;
        public ObservableCollection<DataVerification> DataVerificationOC
        {
            get => _dataVerificationOC;
            set
            {
                _dataVerificationOC = value;
                SetValue(ref _dataVerificationOC, value);
            }
        }
        private string _instructions;
        public string Instructions
        {
            get => _instructions;
            set
            {
                _instructions = value;
                SetValue(ref _instructions, value);
            }
        }

        private string GetDataAccuracy()
        {
            double success = DataVerificationOC.Where(d => d.IsAccurate).Count();
            return (success / DataVerificationOC.Count).ToString("p1");
        }
        private string GetInstructions()
        {
            string message = "This sheet verifies the accuracy of the string reader, by comparing results of where the command paramneters were inserted vs. the index where they were " +
                "found in the string by the Data Reader / processor.";

            return message;
        }
    }
}
