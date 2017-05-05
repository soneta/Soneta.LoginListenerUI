using Soneta.Business;
using Soneta.Business.App;

[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.MyListener), Priority = 200)]

namespace enova365.TwoFactorAuth
{
    public class TwoFactor
    {
        [Context]
        public Context Context { get; set; }

        public string Komunikat
        {
            get { return "przykład lalaal"; }
        }
    }

    public class MyListener : ILoginListenerUI
    {
        Soneta.Types.Currency c1 = Soneta.Types.Currency.Zero;

        public void AfterLoginResult(AfterLoginResultArgs args)
        {
            args.Values.Add( new TwoFactor());
        }
    }
}
