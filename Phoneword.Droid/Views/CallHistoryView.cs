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
    [Activity(Label = "@string/callHistory")]
    public class CallHistoryView : ReactiveActivity<CallHistoryViewModel>
    {
        private ListView callHistoryListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CallHistoryView);

            this.ViewModel = new CallHistoryViewModel();

            this.callHistoryListView = FindViewById<ListView>(Resource.Id.CallHistoryListView);

            var adapter = new ReactiveListAdapter<CallHistoryItemViewModel>(this.ViewModel.CallHistoryList,
                (viewModel, parent) => new CallHistoryItemView(viewModel, this, parent));

            this.callHistoryListView.Adapter = adapter;
        }
    }
}