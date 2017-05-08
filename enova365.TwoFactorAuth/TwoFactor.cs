using enova365.TwoFactorAuth;
using Soneta.Business;
using Soneta.Business.App;
using System;
using System.Drawing;
using System.IO;
using Soneta.Core;



namespace enova365.TwoFactorAuth
{
    public class TwoFactor
    {
        private readonly TwoFactorAuthNet.TwoFactorAuth _tfa;
        private readonly string _sharedSecret;
        private readonly string _code;

        public TwoFactor(string firma, string oper)
        {
            _tfa = new TwoFactorAuthNet.TwoFactorAuth(firma);
            _sharedSecret = _tfa.CreateSecret();
            _code = _tfa.GetQrCodeImageAsDataUri(oper, _sharedSecret);
        }

        public TwoFactor()
        {
            _tfa = new TwoFactorAuthNet.TwoFactorAuth();
        }

        public string GetSharedSecret()
        {
            return _sharedSecret;
        }

        public string GetQrImage()
        {
            return "<html> <img src=\" " + _code + "\"></html>";
        }

        public bool Verify(string secretCode, string userCode)
        {
            return _tfa.VerifyCode(secretCode, userCode);
        }
    }
}
