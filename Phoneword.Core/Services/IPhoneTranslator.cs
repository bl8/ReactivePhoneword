using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePCL.Services
{
    public interface IPhoneTranslator
    {
        string ToNumber(string raw);
    }
}
