using Sodexo_JTH.UWP.Services;
using Sodexo_JTH.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalizeService_UWP))]
namespace Sodexo_JTH.UWP.Services
{
    public class LocalizeService_UWP : ILocalize
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

        public void SetLocale()
        {
            var ci = GetCurrentCultureInfo();
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public string GetIpAddress()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp != null && icp.NetworkAdapter != null)
            {
                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                            hn.IPInformation != null && hn.IPInformation.NetworkAdapter != null
                            && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

                if (hostname != null)
                {
                    // the ip address
                    return hostname.CanonicalName;
                }
            }

            return string.Empty;
        }

        public string GetDeviceName()
        {
           var deviceInfo = new EasClientDeviceInformation();
           return  deviceInfo.FriendlyName;
        }
    }
}
