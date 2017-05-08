using Soneta.Business;
using Soneta.Core;
using Soneta.Types;

namespace enova365.TwoFactorAuth.Utils
{
    public static class Tools
    {
        /// <summary>
        /// Ważne ze względnu na wczytywanie bibliotek, które mają referencje do Soneta.Types
        /// Tylko biblioteki z referencją do Soneta.Types są wczytywane podczas analizowania form.xml
        /// </summary>
#pragma warning disable 414
        private static FromTo _ft = new FromTo();
#pragma warning restore 414

        public const string Version = "1.0";

        public static string EnovaOperator(Session session)
        {
            return session.Login.OperatorName;
        }

        public static string Company(Session session)
        {
            return session.Get<CoreModule>().Config.Firma.Pieczątka.NazwaSkrócona;
        }
    }
}
