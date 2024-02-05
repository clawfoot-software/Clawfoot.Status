using System;
using System.Threading.Tasks;
using Clawfoot.Status.Interfaces;

namespace Clawfoot.Status
{
    public static class InvokeExtensions
    {
        private static void HandleInvokeException(IStatus status, Exception ex, bool keepException)
        {
            if (!keepException)
            {
                status.AddError(ex.Message);
            }
            else
            {
                status.AddException(ex);
            }
        }


        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static IStatus Invoke(this IStatus status, Action action, bool keepException = false)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static Status Invoke(this Status status, Action action, bool keepException = false) =>
            (Status)Invoke((IStatus)status, action, keepException);

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static IStatus Do(this IStatus status, Action action, bool keepException = false) => Invoke(status, action, keepException);

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static Status Do(this Status status, Action action, bool keepException = false) =>
            (Status)Invoke((IStatus)status, action, keepException);

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<IStatus> InvokeAsync(this IStatus status, Func<Task> action, bool keepException = false)
        {
            try
            {
                await action.Invoke();
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static IStatus Invoke<TParam>(this IStatus status, Action<TParam> action, TParam obj, bool keepException = false)
        {
            try
            {
                action.Invoke(obj);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static Status Invoke<TParam>(this Status status, Action<TParam> action, TParam obj, bool keepException = false) =>
            (Status)Invoke((IStatus)status, action, obj, keepException);

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static IStatus Do<TParam>(this IStatus status, Action<TParam> action, TParam obj, bool keepException = false) =>
            Invoke(status, action, obj, keepException);
        
        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        public static Status Do<TParam>(this Status status, Action<TParam> action, TParam obj, bool keepException = false) =>
            (Status)Invoke((IStatus)status, action, obj, keepException);

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<IStatus> InvokeAsync<TParam>(this IStatus status,
            Func<TParam, Task> action,
            TParam obj,
            bool keepException = false)
        {
            try
            {
                await action.Invoke(obj);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static IStatus Invoke(this IStatus status, Func<IStatus> func, bool keepException = false)
        {
            try
            {
                IStatus resultStatus = func.Invoke();
                resultStatus.MergeIntoStatus(status);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static IStatus Do(this IStatus status, Func<IStatus> func, bool keepException = false) =>
            Invoke(status, func, keepException);
        
        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static Status Do(this Status status, Func<Status> func, bool keepException = false) =>
            (Status)Invoke((IStatus)status, func, keepException);
        
        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static Status Do(this Status status, Func<IStatus> func, bool keepException = false) =>
            (Status)Invoke(status, func, keepException);
        
        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<IStatus> InvokeAsync(this IStatus status, Func<Task<IStatus>> func, bool keepException = false)
        {
            try
            {
                IStatus resultStatus = await func.Invoke();
                resultStatus.MergeIntoStatus(status);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus{T}"/>, merges that result status into this status, and returns the TResult result
        /// If an exception occurs, records that exception in this status and returns default(TResult).
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static TResult InvokeResult<TResult>(this IStatus status, Func<IStatus<TResult>> func, bool keepException = false)
        {
            try
            {
                IStatus<TResult> resultStatus = func.Invoke();
                resultStatus.MergeIntoStatus(status); //Merge errors into this
                return resultStatus.Result;
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return default(TResult);
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="IStatus{T}"/>, merges that result status into this status, and returns the TResult result
        /// If an exception occurs, records that exception in this status and returns default(TResult).
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<TResult> InvokeResultAsync<TResult>(this IStatus status,
            Func<Task<IStatus<TResult>>> func,
            bool keepException = false)
        {
            try
            {
                IStatus<TResult> resultStatus = await func.Invoke();
                resultStatus.MergeIntoStatus(status); //Merge errors into this
                return resultStatus.Result;
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return default(TResult);
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status and returns default(TResult).
        /// If success, return the result of the delegate
        /// </summary>
        /// <typeparam name="TResult">The output type</typeparam>
        /// <param name="func">The delegate</param>
        /// <param name="keepException">To keep the exception in the status, or just record the error message</param>
        /// <returns></returns>
        public static TResult InvokeResult<TResult>(this IStatus status, Func<TResult> func, bool keepException = false)
        {
            try
            {
                TResult result = func.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return default(TResult);
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status and returns default(TResult).
        /// If success, return the result of the delegate
        /// </summary>
        /// <typeparam name="TResult">The output type</typeparam>
        /// <param name="func">The delegate</param>
        /// <param name="keepException">To keep the exception in the status, or just record the error message</param>
        /// <returns></returns>
        public static async Task<TResult> InvokeResultAsync<TResult>(this IStatus status,
            Func<Task<TResult>> func,
            bool keepException = false)
        {
            try
            {
                TResult result = await func.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return default(TResult);
        }
    }
}