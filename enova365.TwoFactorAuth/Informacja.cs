using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.UI;
using Soneta.Types;

//[assembly: Service(typeof(ILoginListenerUI), typeof(enova365.TwoFactorAuth.InformacjaListener), Priority = 100)]

namespace enova365.TwoFactorAuth
{
    [DataFormStyle(UseWizard = true, DefaultHeight = 200, DefaultWidth = 300)]
    public class Informacja : ICommittable
    {
        [Context]
        public Context Context { get; set; }

        public string Komunikat => "Krok 1";
        public object OnCommitting(Context cx)
        {
            return new Informacja2();
        }

        public object OnCommitted(Context cx)
        {
            return null;
        }
    }

    [DataFormStyle(UseFinalWizard = true, DefaultHeight = 200, DefaultWidth = 300)]
    public class Informacja2 : ICommittable
    {
        [Context]
        public Context Context { get; set; }

        public string Komunikat2 => "Krok 2";
        public object OnCommitting(Context cx)
        {
            return "Koniec wizarda";
        }

        public object OnCommitted(Context cx)
        {
            return null;
        }
    }

    public class InformacjaListener : ILoginListenerUI
    {
        public void AfterLoginResult(AfterLoginResultArgs args)
        {

            args.Values.Add(new Informacja());


            //args.Values.Add(new MessageBoxInformation("Test","Test Test"));



            //args.Values.Add(QueryContextInformation.Create((MessageParams p) =>
            //{
            //    return new MessageBoxInformation
            //    {
            //        Caption = "Test Query Context",
            //        Text = p.Message,
            //        Type = p.IsWarning ? MessageBoxInformationType.Warning : MessageBoxInformationType.Information
            //    };
            //}));
        }
    }

    [Caption("Nowa nazwa pliku")]
    public class MessageParams : ContextBase
    {

        public MessageParams(Context context) : base(context)
        {
        }

        private string message;

        [Caption("Komunikat")]
        [Required]
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnChanged();
            }
        }

        private bool isWarning;

        [Caption("Ostrzeżenie")]
        public bool IsWarning
        {
            get { return isWarning; }
            set
            {
                isWarning = value;
                OnChanged();
            }
        }

    }
}
