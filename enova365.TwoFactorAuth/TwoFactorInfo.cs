﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using enova365.TwoFactorAuth.Utils;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Types;

namespace enova365.TwoFactorAuth
{
    [Caption("Podwójna weryfikacja")]
    public class TwoFactorInfo : ContextBase
    {
        public TwoFactorInfo(Context context, Session session) : base(context)
        {
            _session = session;
            TwoFactor = new TwoFactor(Tools.Company(session), Tools.EnovaOperator(session));
        }

        private TwoFactor TwoFactor;

        private Session _session { get; set; }

        private string _sharedSecret;

        private string _code;

        private string _qrCode;

        public string SharedSecret
        {
            get { return TwoFactor.GetSharedSecret(); }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string QrCode
        {
            get { return TwoFactor.GetQrImage(); }
        }

        public void Create()
        {

            if(!TwoFactor.Verify(SharedSecret, Code))
                throw new Exception("Zły kod");
        }


    }
}
