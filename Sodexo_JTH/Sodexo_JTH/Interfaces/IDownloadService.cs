using System;
using System.Collections.Generic;
using System.Text;

namespace Sodexo_JTH.Interfaces
{
    public interface IDownloadService
    {
        void DownloadImage(string filename, string URL);
        string GetImage(string filename);
    }
}
