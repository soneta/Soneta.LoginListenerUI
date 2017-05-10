using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Business;
using Soneta.Business.App;

//[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.InformacjaListener), Priority = 100)]

namespace enova365.TwoFactorAuth
{
    [DataFormStyle(UseDialog = true)]
    public class Informacja
    {
        [Context]
        public Context Context { get; set; }

        public string Komunikat => "Przykładowy komunikat po zalogowaniu";
    }

    public class InformacjaListener : ILoginListenerUI
    {
        public void AfterLoginResult(AfterLoginResultArgs args)
        {
            args.Values.Add(new Informacja());
            //args.Values.RemoveRange(0,args.Values.Count);
        }
    }
}
