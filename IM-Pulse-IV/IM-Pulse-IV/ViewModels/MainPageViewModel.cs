using IM_Pulse_IV.Services;
using IM_Pulse_IV.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace IM_Pulse_IV.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SetParameters = new Command(OnSetParameters);
            IntroMessage = SetIntroMessage();
            Instructions = SetInstructions();
        }

        private string _introMessage;
        public string IntroMessage
        {
            get => _introMessage;
            set
            {
                _introMessage = value;
                SetValue(ref _introMessage, value);
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

        public ICommand SetParameters { get; set; }
        private async void OnSetParameters(object obj)
        {
            await new PageService().PushAsync(new MainMenuView());
        }

        private string SetIntroMessage()
        {
            string message = null;

            message += "This Xamarin App was set-up to help demonstrate the capabilities of Dewayne Wagner to create a Cross-Platform Application," +
                "utilizing different concepts, such as: " +
                "\n\n" +
                "1. Xamarin MVVM Architecture" +
                "\n" +
                "2. SQLite database" +
                "\n" +
                "3. JSON reading / writing" +
                "\n" +
                "4. Clean & efficient string data processing" +                
                "\n\n" +
                "Although I don't have a lot of software development experience, I am an extremely quick learner, very hard-working, and possess " +
                "a tremendous amount of unique skillsets.  Also - I have a deep understanding and fascination with drilling practices, procedures, and technology." +
                "\n\n" +
                "There may be things that a more experienced software developer have that I don't have - but there are many more benefits with my skillset that " +
                "many other software developers don't have as well." +
                "\n\n" +
                "Basically:  this is my dream job, and I hope you will consider me for this opportunity!";

            return message;
        }

        private string SetInstructions()
        {
            string message = "Test";

            message += "IM-Pulse-IV generates a random string of text, based-upon the parameters entered in the next page.  The program then processes " +
                "the data, searching for the Command Parameters that are set, then returns the index of these strings.";

            return message;
        }
    }
}
