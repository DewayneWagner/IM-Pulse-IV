using System.Threading.Tasks;
using Xamarin.Forms;

namespace TheManXS.ViewModel.Services
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task DisplayAlert(string title, string message);
        Task DisplayAlert(string message);
        Task PopAsync();
    }
}
