using System.Globalization;

namespace Sodexo_JTH.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        CultureInfo GetCurrentCultureInfo(string sLanguageCode);
        void SetLocale();
        void ChangeLocale(string sLanguageCode);

        string GetIpAddress();

        string GetDeviceName();
    }
}
