using System;
using System.Collections;
using enova365.TwoFactorAuth;
using enova365.TwoFactorAuth.Utils;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.Db;
using Soneta.Business.UI;
using Soneta.Tools;
using Soneta.Types;

[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.RegistrationListener), Priority = 200)]
[assembly: SpecialAccessSection(typeof(Secret), "Zapisz SharedSecret", typeof(TwoFactorRegistration), "SaveSharedSecret")]

namespace enova365.TwoFactorAuth
{
    [DataFormStyle(UseDialog = true, DefaultHeight = 400, DefaultWidth = 60)]
    [Caption("Konfiguracja podwójnego logowania")]
    public class TwoFactorRegistration : IVerifiable, ICommittable
    {
        Context _context;

        public TwoFactorRegistration(Context ctx)
        {
            _context = ctx;
            _twoFactor = new TwoFactor(Tools.Company(ctx.Session), Tools.EnovaOperator(ctx.Session));
            ctx.Session.Verifiers.Add(new CodeVerifier(this));
        }

        private readonly TwoFactor _twoFactor;

        public string CodeFromDevice { get; set; }

        public string SharedSecret => _twoFactor.GetSharedSecret();

        public string QrCode => _twoFactor.GetQrImage();


        public IEnumerable GetVerifiers()
        {
            return ((IVerifiable)_context.Session).GetVerifiers();
        }

        public void SaveSharedSecret(Secret row, Session session)
        {
            using (ITransaction trans = session.Logout(true))
            {
                TwoFactorModule.GetInstance(session).Secrets.AddRow(row);
                trans.CommitUI();
            }
        }

        public object OnCommitting(Context cx)
        {
            using (Session session = cx.Login.CreateSession(false, true))
            {
                var bm = BusinessModule.GetInstance(session);
                var row = new Secret(bm.Operators.ByName[cx.Login.OperatorName]) { SharedSecret = SharedSecret };
                session.ExecuteSpecialAccessSection((Method<Secret, Session>)SaveSharedSecret, row, session);
                session.Save();
            }
            return null;
        }

        public object OnCommitted(Context cx)
        {
            return null;
        }

        public class CodeVerifier : Verifier
        {
            private readonly TwoFactorRegistration _twoFactorRegistration;

            public CodeVerifier(TwoFactorRegistration tfre)
            {
                _twoFactorRegistration = tfre;
            }
            public override string Description => "Wprowadzony kod jet niepoprawny. Wpisz nowy.";

            public override object Source => _twoFactorRegistration;

            protected override bool IsValid()
            {
                if (_twoFactorRegistration.CodeFromDevice.IsNullOrEmpty())
                    return false;
                if (_twoFactorRegistration._twoFactor.Verify(_twoFactorRegistration.SharedSecret,
                    _twoFactorRegistration.CodeFromDevice))
                {
                    return true;
                }
                return false;
            }
        }
    }

    public class RegistrationListener : ILoginListenerUI
    {
        public void AfterLoginResult(AfterLoginResultArgs args)
        {
            if (TwoFactorModule.GetInstance(args.Context.Session).Secrets.WgOperator[args.Context.Session.Login.Operator] == null)
                args.Values.Add(new TwoFactorRegistration(args.Context));
        }
    }
}
