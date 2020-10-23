using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.iOS.Services;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(Notification_iOS))]
namespace Sodexo_JTH.iOS.Services
{
    class Notification_iOS : INotify
    {
        NSTimer alertDelay;
        UIAlertController alert;
        public Task<string> ShowAlert(string title, string body)
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowAlert(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            return null;
        }

        public void ShowAlertDroid(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            throw new NotImplementedException();
        }

        public void ShowLocalNotification(string title, string body)
        {
            var content = new UNMutableNotificationContent();
            content.Title = title;
            content.Body = body;
            content.Sound = UNNotificationSound.Default;
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);
            var request = UNNotificationRequest.FromIdentifier("FiveSecond", content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (NSError error) =>
            {
                if (error != null) Console.WriteLine(error);
            });

            UNUserNotificationCenter.Current.Delegate = new MyUNUserNotificationCenterDelegate();
        }

        public void ShowToast(string descripition)
        {
            alertDelay = NSTimer.CreateScheduledTimer(1, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, descripition, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        private void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
    public class MyUNUserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {

            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

    }
}