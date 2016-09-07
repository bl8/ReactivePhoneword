using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePCL.Services
{
    class CallHistoryManager
    {
        private static IList<Call> calls;

        public CallHistoryManager()
        {
            calls = new List<Call>();
        }

        async public Task Add(Call call)
        {
            await Task.Run(() => calls.Add(call));
        }

        public IObservable<Call> GetAll()
        {
            return Observable.Create<Call>(async observer =>
            {
                foreach(var c in calls)
                {
                    Call call;
                    await Task.Run(() => call = c);
                    observer.OnNext(c);
                }
                observer.OnCompleted();
            });
        }
    }
}
