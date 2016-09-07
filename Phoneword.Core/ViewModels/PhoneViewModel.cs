using CorePCL.Services;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace CorePCL.ViewModels
{
    public class PhoneViewModel : ReactiveObject
    {
        string _phoneNumber;
        public string PhoneNumber {
            get { return _phoneNumber;  }
            set { this.RaiseAndSetIfChanged(ref _phoneNumber, value); }
        }

        public ReactiveCommand<string> TranslateNumber { get; protected set; }

        public ReactiveCommand<Unit> Call { get; protected set; }

        public ReactiveCommand<object> CallHistory { get; protected set; }

        ObservableAsPropertyHelper<string> _translatedNumber;
        public string TranslatedNumber => _translatedNumber.Value;

        public PhoneViewModel()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new CallHistoryManager(), typeof(CallHistoryManager));

            TranslateNumber = ReactiveCommand.CreateAsyncTask(parameter => GetTranslatedNumber(this.PhoneNumber));

            this.WhenAnyValue(x => x.PhoneNumber)
                .Throttle(TimeSpan.FromMilliseconds(800), RxApp.MainThreadScheduler)
                .Select(x => x?.Trim())
                .DistinctUntilChanged()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .InvokeCommand(TranslateNumber);

            TranslateNumber.ThrownExceptions.Subscribe(ex => {/* Handle errors*/ });

            _translatedNumber = TranslateNumber.ToProperty(this, x => x.TranslatedNumber);

            Call = ReactiveCommand.CreateAsyncTask(
                this.WhenAnyValue(x => x.TranslatedNumber).
                Select(x => !String.IsNullOrWhiteSpace(x)),
                async _ =>
                {
                    var c = new Call();
                    c.NumberCalled = TranslatedNumber;
                    await Locator.Current.GetService<CallHistoryManager>().Add(c);
                });

            CallHistory = ReactiveCommand.Create();
        }


        public static async Task<string> GetTranslatedNumber(string raw)
        {
            return await Task.Run(() => (new PhoneTranslator()).ToNumber(raw));
        }

    }
}
