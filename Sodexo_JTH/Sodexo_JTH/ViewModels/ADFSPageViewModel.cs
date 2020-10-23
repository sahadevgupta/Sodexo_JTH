using Prism.Commands;
using Prism.Mvvm;
using Sodexo_JTH.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sodexo_JTH.ViewModels
{
	public class ADFSPageViewModel : BindableBase
	{
        private string _source;

        public string Source
        {
            get { return this._source; }
            set { SetProperty(ref _source, value); }
        }
        public ADFSPageViewModel()
        {
            Source = $"https://gatesstaging.sodexonet.com/adfs/oauth2/authorize?response_type=code&client_id={Library.Client_ID}&resource={Library.Resource_identifier}&redirect_uri={Library.Login_redirect_URI}";
        }
    }
}
