using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Sodexo_JTH.Droid.Services;
using Sodexo_JTH.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(Notification_droid))]
namespace Sodexo_JTH.Droid.Services
{
    public class Notification_droid : INotify
    {

        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        public Task<string> ShowAlert(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            var dlgAlert = new AlertDialog.Builder(MainApplication.ActivityContext).Create();
            dlgAlert.SetMessage(body);
            dlgAlert.SetTitle(title);
            dlgAlert.SetButton(acceptbtn, handllerNotingButton);
            dlgAlert.SetButton2(rejectbtn, handllerNotingButton);
            dlgAlert.SetButton3(cancelbtn, handllerNotingButton);
            dlgAlert.Show();

            return null;   
        }
        void handllerNotingButton(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Android.Widget.Button btnClicked = objAlertDialog.GetButton(e.Which);

            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "btnClicked", btnClicked.Text);

            //Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        public void ShowLocalNotification(string title, string body)
        {
            var intent = new Intent(MainApplication.ActivityContext, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(MainApplication.ActivityContext, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(MainApplication.ActivityContext)
                .SetSmallIcon(Resource.Drawable.AppIcon)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetContentIntent(pendingIntent);


            var notificationManager = NotificationManager.FromContext(MainApplication.ActivityContext);
            notificationManager.Notify(0, notificationBuilder.Build());

            //NotificationCompat.Builder builder = new NotificationCompat.Builder(MainApplication.ActivityContext, CHANNEL_ID)
            //                                                           .SetContentTitle(title)
            //                                                           .SetContentText(body)
            //                                                           .SetSmallIcon(Resource.Drawable.AppIcon)
                                                                       

            //// Build the notification:
            //Notification notification = builder.Build();

            //// Get the notification manager:
            //NotificationManager notificationManager =
            //    MainApplication.ActivityContext.GetSystemService(Context.NotificationService) as NotificationManager;

            //// Publish the notification:
            //const int notificationId = 0;
            //notificationManager.Notify(notificationId, notification);
        }

        public void ShowToast(string body)
        {
            var activity = MainApplication.ActivityContext as Activity;
            activity.RunOnUiThread(() =>
            {
                var view = activity.FindViewById(Android.Resource.Id.Content);
                Toast.MakeText(activity, body, ToastLength.Long).Show();
            });
        }

        public void ShowAlertDroid(string title, string body, string acceptbtn = null, string rejectbtn = null, string cancelbtn = null)
        {
            var dlgAlert = new AlertDialog.Builder(MainApplication.ActivityContext).Create();
            dlgAlert.SetMessage(body);
            dlgAlert.SetTitle(title);
            dlgAlert.SetButton(acceptbtn, handllerNotingButton);
            dlgAlert.SetButton2(rejectbtn, handllerNotingButton);
            dlgAlert.SetButton3(cancelbtn, handllerNotingButton);
            dlgAlert.Show();
        }
    }
}