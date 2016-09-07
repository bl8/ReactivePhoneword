using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using CorePCL.ViewModels;

namespace Phoneword.Views
{
    public class CallHistoryItemView : ReactiveViewHost<CallHistoryItemViewModel>
    {
        public CallHistoryItemView(CallHistoryItemViewModel viewModel, Context context, ViewGroup parent)
            : base(context, Resource.Layout.CallHistoryItemView, parent)
        {
            this.ViewModel = viewModel;

            this.OneWayBind(this.ViewModel, vm => vm.PhoneNumber, v => v.CallHistoryItem.Text);
        }

        public TextView CallHistoryItem { get; private set; }
    }
}