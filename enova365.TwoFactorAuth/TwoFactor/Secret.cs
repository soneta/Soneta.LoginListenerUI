using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using enova365.TwoFactorAuth;
using Soneta.Business;
using Soneta.Business.App;

//[assembly: NewRow(typeof(Secret))]

namespace enova365.TwoFactorAuth
{

    public class Secret : TwoFactorModule.SecretRow
    {
        public Secret(RowCreator creator) : base(creator)
        {
        }

        public Secret(Operator _operator) : base(_operator)
        {
        }
    }
}
