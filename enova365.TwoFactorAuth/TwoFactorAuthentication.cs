using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Business;
using Soneta.Business.App;


[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.MyListener), Priority = 200)]


namespace enova365.TwoFactorAuth
{
    public class TwoFactorAuthentication
    {

    }


    public class MyListener : ILoginListenerUI
    {
        public void AfterLoginResult(AfterLoginResultArgs args)
        {
            args.Values.Add(new TwoFactorAuthentication());
        }
    }
}
