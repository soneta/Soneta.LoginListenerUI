using System;
using System.Collections;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Tools;
using Soneta.Types;

[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.AuthenticationListener), Priority = 200)]


namespace enova365.TwoFactorAuth
{
    [DataFormStyle(UseDialog = true, DefaultHeight = 100, DefaultWidth = 350)]
    [Caption("Weryfikacja danych logowania")]
    public class TwoFactorAuthentication : IVerifiable
    {
        Context _context;

        public TwoFactorAuthentication(Context ctx)
        {
            _context = ctx;
            ctx.Session.Verifiers.Add(new CodeVerifier(this));
        }

        public string CodeFromDevice { get; set; }

        public IEnumerable GetVerifiers()
        {
            return ((IVerifiable)_context.Session).GetVerifiers();
        }

        public class CodeVerifier : Verifier
        {
            private readonly TwoFactorAuthentication _twoFactorAuthentication;

            public CodeVerifier(TwoFactorAuthentication tfan)
            {
                _twoFactorAuthentication = tfan;
            }
            public override string Description => "Wygenerowany kod jest niepoprawny, sprawdź czy dobrze go przepisałeś z aplikacji w telefonie.";

            public override object Source => _twoFactorAuthentication;

            protected override bool IsValid()
            {
                if (_twoFactorAuthentication.CodeFromDevice.IsNullOrEmpty())
                    return false;
                TwoFactor tfa = new TwoFactor();
                Session ses = _twoFactorAuthentication._context.Session;
                var sec = TwoFactorModule.GetInstance(ses).Secrets.WgOperator[ses.Login.Operator];
                
                return tfa.Verify(sec.SharedSecret, _twoFactorAuthentication.CodeFromDevice);
            }
        }
    }

    public class AuthenticationListener : ILoginListenerUI
    {
        public void AfterLoginResult(AfterLoginResultArgs args)
        {
            if (TwoFactorModule.GetInstance(args.Context.Session).Secrets.WgOperator[args.Context.Session.Login.Operator] != null)
                args.Values.Add(new TwoFactorAuthentication(args.Context));
        }
    }
}
