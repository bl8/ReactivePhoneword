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

        public ReactiveCommand<string, string> TranslateNumber { get; protected set; }

        public ReactiveCommand<Unit, Unit> Call { get; protected set; }

        public ReactiveCommand<Unit, Unit> CallHistory { get; protected set; }

        ObservableAsPropertyHelper<string> _translatedNumber;
        public string TranslatedNumber => _translatedNumber.Value;

        public PhoneViewModel()
        {
            RegisterServices();

            TranslateNumber = ReactiveCommand.CreateFromTask<string, string>(
                _ => GetTranslatedNumber(this.PhoneNumber));

            this.WhenAnyValue(x => x.PhoneNumber)
                .Throttle(TimeSpan.FromMilliseconds(800), RxApp.MainThreadScheduler)
                .Select(x => x?.Trim())
                .DistinctUntilChanged()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .InvokeCommand(TranslateNumber);

            TranslateNumber.ThrownExceptions.Subscribe(ex => {/* Handle errors*/ });

            _translatedNumber = TranslateNumber.ToProperty(this, x => x.TranslatedNumber);

            Call = ReactiveCommand.CreateFromTask(
                async _ =>
                {
                    var c = new Call() { NumberCalled = TranslatedNumber };
                    await Locator.Current.GetService<CallHistoryManager>().Add(c);
                },
                this.WhenAnyValue(x => x.TranslatedNumber).
                Select(x => !String.IsNullOrWhiteSpace(x))
            );

            CallHistory = ReactiveCommand.Create(() => { });
        }

        public static async Task<string> GetTranslatedNumber(string raw)
        {
            return await Task.Run(() => Locator.Current.GetService<IPhoneTranslator>().ToNumber(raw));
        }

        private void RegisterServices()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new PhoneTranslator(), typeof(IPhoneTranslator));
            Locator.CurrentMutable.RegisterLazySingleton(() => new CallHistoryManager(), typeof(CallHistoryManager));
        }
    }
}
