using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CorePCL.ViewModels
{
    public class CallHistoryItemViewModel : ReactiveObject
    {
        public string PhoneNumber { get; private set; }

        public CallHistoryItemViewModel(string phoneNumber)
        {
            this.PhoneNumber = phoneNumber;
        }
    }
}
