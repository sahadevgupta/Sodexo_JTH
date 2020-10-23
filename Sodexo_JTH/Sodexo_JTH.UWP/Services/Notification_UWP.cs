using Sodexo_JTH.Interfaces;
using System;
using Windows.UI.Notifications;
using Xamarin.Forms;
using Windows.Data.Xml.Dom;
using Sodexo_JTH.UWP.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Popups;
using System.Threading.Tasks;
using SolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;

[assembly:Dependency(typeof(Notification_UWP))]
namespace Sodexo_JTH.UWP.Services
{
    public class Notification_UWP : INotify
    {

        private const string _TOAST_TEXT02_TEMPLATE = "<toast>"
                                                    + "<visual>"
                                                    + "<binding template='ToastText02'>"
                                                    + "<text id='1'>{0}</text>"
                                                    + "<text id='2'>{1}</text>"
                                                    + "</binding>"
                                                    + "</visual>"
                                                    + "</toast>";

        public void ShowLocalNotification(string title, string body)
        {
            // XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var xmlData = string.Format(_TOAST_TEXT02_TEMPLATE, title, body);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);
            //DateTime currentDate = DateTime.Now;
            //var nTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 30, 0);
            //var a = DateTime.Now.AddSeconds(5);
            //DateTime scheduledTime = DateTime.Today + TimeSpan.FromHours(11) + TimeSpan.FromDays(1);
            //var scheduledTileNotification = new ScheduledToastNotification(xmlDoc, scheduledTime)
            //{
            //    Id = id.ToString(),

            //};
            var toast = new ToastNotification(xmlDoc);
           
            //TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(scheduledTileNotification);

            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast);
            // var toast = new ToastNotification(xmlDoc);
        }

        public void ShowToast(string descripition)
        {
            var label = new TextBlock
            {
                Text = descripition,
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            var style = new Windows.UI.Xaml.Style { TargetType = typeof(FlyoutPresenter) };
            style.Setters.Add(new Windows.UI.Xaml.Setter(Control.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.Black)));
            style.Setters.Add(new Windows.UI.Xaml.Setter(FrameworkElement.MaxHeightProperty, 1));
            var flyout = new Flyout
            {
                Content = label,
                Placement = FlyoutPlacementMode.Full,
                FlyoutPresenterStyle = style,
            };

            flyout.ShowAt(Window.Current.Content as FrameworkElement);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (sender, e) => {
                timer.Stop();
                flyout.Hide();
            };
            timer.Start();
        }

        public async Task<string> ShowAlert(string title,string body,string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            var dialog = new MessageDialog(body, title);
            
            // If you want to add custom buttons
            if (!string.IsNullOrEmpty(acceptbtn))
            {
                dialog.Commands.Add(new UICommand(acceptbtn, delegate (IUICommand command)
                {
                    // Your command action here
                }));
            }

            if (!string.IsNullOrEmpty(rejectbtn))
            {
                dialog.Commands.Add(new UICommand(rejectbtn, delegate (IUICommand command)
                {
                    // Your command action here
                }));
            }

            if (!string.IsNullOrEmpty(cancelbtn))
            {
                dialog.Commands.Add(new UICommand(cancelbtn, delegate (IUICommand command)
                {
                    // Your command action here
                }));
            }
           

            // Show dialog and save result
            var result = await dialog.ShowAsync();
            return result.Label;
        }

        public void ShowAlertDroid(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            throw new NotImplementedException();
        }
    }
    
}
