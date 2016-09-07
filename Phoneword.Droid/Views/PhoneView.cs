using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using ReactiveUI;
using CorePCL.ViewModels;

namespace Phoneword.Views
{
    [Activity(Label = "Phone Word", MainLauncher = true)]
    public class PhoneView : ReactiveActivity<PhoneViewModel>
    {
        public EditText PhoneNumberText { get; private set; }
        public TextView TranslatedText { get; private set; }
        public Button CallButton { get; private set; }
        public Button CallHistoryButton { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainView);

            ViewModel = new PhoneViewModel();

            PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            TranslatedText = FindViewById<TextView>(Resource.Id.TranslatedNumber);
            CallButton = FindViewById<Button>(Resource.Id.CallButton);
            CallHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

            this.Bind(ViewModel, vm => vm.PhoneNumber, v => v.PhoneNumberText.Text);
            this.OneWayBind(ViewModel, vm => vm.TranslatedNumber, v => v.TranslatedText.Text);
            this.BindCommand(ViewModel, vm => vm.Call, v => v.CallButton);
            this.BindCommand(ViewModel, vm => vm.CallHistory, v => v.CallHistoryButton);

            ViewModel.Call.Subscribe(_ =>
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + ViewModel.TranslatedNumber + " ?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + ViewModel.TranslatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                callDialog.Show();
            });

            ViewModel.CallHistory.Subscribe(_ =>
            {
                var intent = new Intent(this, typeof(CallHistoryView));
                StartActivity(intent);
            });
        }
    }
}
