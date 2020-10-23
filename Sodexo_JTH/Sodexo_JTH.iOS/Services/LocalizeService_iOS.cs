using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Foundation;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalizeService_iOS))]
namespace Sodexo_JTH.iOS.Services
{
    class LocalizeService_iOS : ILocalize
    {
        public void ChangeLocale(string sLanguageCode)
        {
            var ci = CultureInfo.CreateSpecificCulture(sLanguageCode);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Console.WriteLine("ChangeToLanguage: " + ci.Name);
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            CultureInfo ci = null;
            ci = System.Globalization.CultureInfo.CurrentCulture;
            return ci;
        }

        public CultureInfo GetCurrentCultureInfo(string sLanguageCode)
        {
            return CultureInfo.CreateSpecificCulture(sLanguageCode);
        }

        public string GetDeviceName()
        {
           return UIDevice.CurrentDevice.SystemName;
        }

        public string GetIpAddress()
        {
            var ipAddress = "";

            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = addrInfo.Address.ToString();

                        }
                    }
                }
            }

            return ipAddress;

        }

        public void SetLocale()
        {
            var ci = GetCurrentCultureInfo();
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}