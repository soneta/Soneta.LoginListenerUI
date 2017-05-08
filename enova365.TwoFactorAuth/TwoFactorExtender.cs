using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using enova365.TwoFactorAuth;
using enova365.TwoFactorAuth.Utils;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Types;

[assembly: Worker(typeof(TwoFactorExtender))]

namespace enova365.TwoFactorAuth
{
    public class TwoFactorExtender
    {
        [Context]
        public Session Session{get; set; }

        public FormActionResult CreateTwoFactor(Context cx)
        {
            TwoFactorInfo twoFactorInfo = new TwoFactorInfo(cx, Session);

            return new FormActionResult
            {
                Context = cx,
                EditValue = twoFactorInfo,
                UseDialog = true,
                CommittingHandler = cx2 =>
                {
                    twoFactorInfo.Create();
                    return new MessageBoxInformation()
                    {
                        Caption = "Potwierdzenie",
                        Text = "Działa"
                    };
                }
            };
        }
    }
}
