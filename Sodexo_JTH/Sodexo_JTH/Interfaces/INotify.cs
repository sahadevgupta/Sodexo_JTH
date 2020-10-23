using System.Threading.Tasks;

namespace Sodexo_JTH.Interfaces
{
    public interface INotify
    {
        void ShowLocalNotification(string title, string body);
        void ShowToast(string body);
        Task<string> ShowAlert(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null);

        void ShowAlertDroid(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null);
    }
}
