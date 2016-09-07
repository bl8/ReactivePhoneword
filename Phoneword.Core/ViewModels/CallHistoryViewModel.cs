using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using CorePCL.Services;

namespace CorePCL.ViewModels
{
    public class CallHistoryViewModel : ReactiveObject
    {
        ReactiveList<CallHistoryItemViewModel> callHistoryList;
        public ReactiveList<CallHistoryItemViewModel> CallHistoryList
        {
            get { return callHistoryList; }
            set { this.RaiseAndSetIfChanged(ref this.callHistoryList, value); }
        }

        public CallHistoryViewModel()
        {
            this.CallHistoryList = new ReactiveList<CallHistoryItemViewModel>();

            var calls = Locator.Current.GetService<CallHistoryManager>().GetAll();

            calls.Subscribe<Call>(call => 
            {
                this.CallHistoryList.Add(new CallHistoryItemViewModel(call.NumberCalled));
            });
        }
    }
}
