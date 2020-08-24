using IM_Pulse_IV.IO;
using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.Models.RandomDataGenerator;
using IM_Pulse_IV.Services;
using IM_Pulse_IV.Views;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace IM_Pulse_IV.ViewModels
{
    class MainMenuViewModel : BaseViewModel
    {
        RandomDataParameters _randomDataParameters;

        public MainMenuViewModel(bool isForPropertyDictionary) {  }

        public MainMenuViewModel()
        {
            _randomDataParameters = (RandomDataParameters)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)];
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.MainMenuViewModel)] = this;
            InitFields();
            StartSimulation = new Command(OnStartSimulation);
            AddNewCommandString = new Command(OnAddNewCommandString);
            CommandParameterList = new CommandParameterList();
        }

        private void InitFields()
        {            
            SegmentLength = _randomDataParameters.SegmentLength;
            NumberOfSegments = _randomDataParameters.NumberOfSegments;
            Delimiter = _randomDataParameters.DeLimiter;
            MinNumberOfCommandsPerSegment = _randomDataParameters.MinNumberOfCommandsPerSegment;
            MaxNumberOfCommandsPerString = _randomDataParameters.MaxNumberOfCommandsPerSegment;
            IsPulseVsContinuous = _randomDataParameters.IsPulseVsContinuous;
            RandomTextGenerationFrequency = _randomDataParameters.RandomTextGenerationFrequency;
            ValidateMessage = "Data Validation...";
        }

        public int MinSegmentLength { get => RandomDataParameters.MinSegmentLength; }
        public int MaxSegmentLength { get => RandomDataParameters.MaxSegmentLength; }
        public int MinNumberOfSegments { get => RandomDataParameters.MinNumberOfSegments; }
        public int MaxNumberOfSegments { get => RandomDataParameters.MaxNumberOfSegments; }
        public int MinCommandsPerSegment { get => RandomDataParameters.MinCommandsPerSegment; }
        public int MaxCommandsPerSegment { get => RandomDataParameters.MaxCommandsPerSegment; }
        public int MinTextGenerationFrequency { get => RandomDataParameters.MinTextGenerationFrequency; }
        public int MaxTextGenerationFrequency { get => RandomDataParameters.MaxTextGenerationFrequency; }

        private int _segmentLength;
        public int SegmentLength
        {
            get => _segmentLength;
            set
            {
                _segmentLength = value;
                SetValue(ref _segmentLength, value);
            }
        }
        private int _numberOfSegments;
        public int NumberOfSegments
        {
            get => _numberOfSegments;
            set
            {
                _numberOfSegments = value;
                SetValue(ref _numberOfSegments, value);
            }
        }
        private string _delimiter;
        public string Delimiter
        {
            get => _delimiter;
            set
            {
                _delimiter = value;
                SetValue(ref _delimiter, value);
            }
        }
        private int _minNumberOfCommandsPerSegment;
        public int MinNumberOfCommandsPerSegment
        {
            get => _minNumberOfCommandsPerSegment;
            set
            {
                _minNumberOfCommandsPerSegment = value;
                SetValue(ref _minNumberOfCommandsPerSegment, value);
            }
        }
        private int _maxNumberOfCommandsPerString;
        public int MaxNumberOfCommandsPerString
        {
            get => _maxNumberOfCommandsPerString;
            set
            {
                _maxNumberOfCommandsPerString = value;
                SetValue(ref _maxNumberOfCommandsPerString, value);
            }
        }
        private int _randomTextGenerationFrequency;
        public int RandomTextGenerationFrequency
        {
            get => _randomTextGenerationFrequency;
            set
            {
                _randomTextGenerationFrequency = value;
                SetValue(ref _randomTextGenerationFrequency, value);
            }
        }
        private bool _isPulseVsContinuous;
        public bool IsPulseVsContinuous
        {
            get => _isPulseVsContinuous;
            set
            {
                _isPulseVsContinuous = value;
                SetValue(ref _isPulseVsContinuous, value);
            }
        }
        private CommandParameterList _commandParametersList;
        public CommandParameterList CommandParameterList
        {
            get => _commandParametersList;
            set
            {
                _commandParametersList = value;
                SetValue(ref _commandParametersList, value);
            }
        }
        private string _newCommandString;
        public string NewCommandString
        {
            get => _newCommandString;
            set
            {
                _newCommandString = value;
                SetValue(ref _newCommandString, value);
            }
        }
        private string _validateMessage;
        public string ValidateMessage
        {
            get => _validateMessage;
            set
            {
                _validateMessage = value;
                SetValue(ref _validateMessage, value);
            }
        }

        public ICommand StartSimulation { get; set; }
        private async void OnStartSimulation(object obj)
        {
            RandomDataParameters rdp = new RandomDataParameters(_segmentLength, _numberOfSegments, _delimiter, _minNumberOfCommandsPerSegment, _maxNumberOfCommandsPerString,
                _isPulseVsContinuous, _randomTextGenerationFrequency);

            if (rdp.EntriesAreValid())
            {                
                App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.RandomDataParameters)] = rdp;
                SavedRandomDataParameters_JSON.WriteRandomDataParametersToFile();
                UniqueTextParameter.InitDB();
                new RandomTextGenerator();
                await new PageService().PushAsync(new DisplayDataView());
            }
            else
            {
                ValidateMessage = rdp.ValidationMessage;
            }
        }

        public ICommand AddNewCommandString { get; set; }
        private void OnAddNewCommandString(object obj)
        {
            NewCommandString.Trim();
            NewCommandString.Replace(" ", string.Empty);

            if (!string.IsNullOrEmpty(NewCommandString) && !CommandParameterList.Any(c => c.CommandName == NewCommandString))
            {
                CommandParameter newCommandParameter = new CommandParameter() { CommandName = NewCommandString };
                CommandParameterList.Add(newCommandParameter);
                using (DBContext db = new DBContext())
                {
                    db.CommandParameters.Add(newCommandParameter);
                    db.SaveChanges();
                }
            }
        }
        public async void DeleteCommand(CommandParameter commandParameter)
        {
            PageService ps = new PageService();
            bool confirmDelete = await ps.DisplayAlert("Confirm Delete", "Are you sure you want to delete this Command string?", "Absolutely!", "Hell No!");
            if (commandParameter != null)
            {
                if (confirmDelete) ps.DisplayAlert("Confirm Delete", "Are you sure you want to delete this Command string?", "Absolutely!", "Hell No!");
                {
                    CommandParameterList.Remove(commandParameter);
                    using (DBContext db = new DBContext())
                    {
                        if (db.CommandParameters.Any(c => c.CommandName == commandParameter.CommandName))
                        {
                            db.Remove(commandParameter);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
