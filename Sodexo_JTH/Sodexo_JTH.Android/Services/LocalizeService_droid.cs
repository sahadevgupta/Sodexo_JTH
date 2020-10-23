using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sodexo_JTH.Droid.Services;
using Sodexo_JTH.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalizeService_droid))]

namespace Sodexo_JTH.Droid.Services
{
    public class LocalizeService_droid : ILocalize
    {
        public void ChangeLocale(string sLanguageCode)
        {
            throw new NotImplementedException();
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default; // user's preferred locale
            var netLocale = androidLocale.ToString().Replace("_", "-");

            #region Debugging output
            Console.WriteLine("android:  " + androidLocale.ToString());
            Console.WriteLine("netlang:  " + netLocale);

            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Console.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
            Console.WriteLine("threadui:" + Thread.CurrentThread.CurrentUICulture);
            #endregion

            return ci;
        }

        public CultureInfo GetCurrentCultureInfo(string sLanguageCode)
        {
            return CultureInfo.CreateSpecificCulture(sLanguageCode);
        }

        public string GetDeviceName()
        {
            var ci = GetCurrentCultureInfo();
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return null;
        }

        public string GetIpAddress()
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName());

            return addresses?[0]?.ToString();
        }

        public void SetLocale()
        {
            throw new NotImplementedException();
        }
    }
}