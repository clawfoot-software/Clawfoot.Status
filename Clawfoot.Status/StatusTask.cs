using Clawfoot.Status.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clawfoot.Status
{
    [AsyncMethodBuilder(typeof(StatusTaskTaskMethodBuilder<>))]
    public class StatusTask<T> : INotifyCompletion
    {
        private Action _continuation;

        public StatusTask()
        { }

        public StatusTask(T value)
        {
            this.Value = new Status<T>(value);
            this.IsCompleted = true;
        }

        public StatusTask(IError error)
        {
            this.Value = Status.Error<T>(error);
            this.IsCompleted = true;
        }

        public StatusTask<T> GetAwaiter() => this;

        public bool IsCompleted { get; private set; }

        public IStatus<T> Value { get; private set; }

        public Exception Exception { get; private set; }

        public static StatusTask<T> AsError(IError error)
        {
            return new StatusTask<T>(error);
        }

        public IStatus<T> GetResult()
        {
            if (!this.IsCompleted) throw new Exception("Not completed");
            if (this.Exception != null)
            {
                ExceptionDispatchInfo.Capture(this.Exception).Throw();
            }
            return this.Value;
        }


        internal void SetResult(IError error)
        {
            if (this.IsCompleted) throw new Exception("Already completed");
            this.Value = Status.Error<T>(error);
            this.IsCompleted = true;
            this._continuation?.Invoke();
        }

        internal void SetResult(T value)
        {
            if (this.IsCompleted) throw new Exception("Already completed");
            this.Value = new Status<T>(value);
            this.IsCompleted = true;
            this._continuation?.Invoke();
        }

        internal void SetException(Exception exception)
        {
            this.IsCompleted = true;
            this.Exception = exception;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            this._continuation = continuation;
            if (this.IsCompleted)
            {
                continuation();
            }
        }
    }
    public class StatusTask
    {

    }

    public class StatusTaskTaskMethodBuilder<T>
    {
        public StatusTaskTaskMethodBuilder()
            => this.Task = new StatusTask<T>();

        public static StatusTaskTaskMethodBuilder<T> Create()
        => new StatusTaskTaskMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
            => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void SetException(Exception exception)
            => this.Task.SetException(exception);

        public void SetResult(IError result)
            => this.Task.SetResult(result);

        public void SetResult(T result)
            => this.Task.SetResult(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => this.GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => this.GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        public void GenericAwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => awaiter.OnCompleted(stateMachine.MoveNext);

        public StatusTask<T> Task { get; }
    }

    public sealed class StatusClassMethodBuilder
    {

    }

}
