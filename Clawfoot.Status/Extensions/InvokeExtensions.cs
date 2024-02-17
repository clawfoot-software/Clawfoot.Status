using System;
using System.Threading.Tasks;

namespace Clawfoot.Status
{
    public static class InvokeExtensions
    {
        private static void HandleInvokeException<TStatus>(TStatus status, Exception ex, bool keepException)
            where TStatus: AbstractStatus<TStatus>
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
        public static TStatus Invoke<TStatus>(this TStatus status, Action action, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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
        public static TStatus Do<TStatus>(this TStatus status, Action action, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            return Invoke(status, action, keepException);
        }
            

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<TStatus> InvokeAsync<TStatus>(this TStatus status, Func<Task> action, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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
        public static TStatus Invoke<TParam, TStatus>(this TStatus status, Action<TParam> action, TParam obj, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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
        public static TStatus Do<TParam, TStatus>(this TStatus status, Action<TParam> action, TParam obj, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            return Invoke(status, action, obj, keepException);
        }
        
        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status.
        /// Returns this status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<TStatus> InvokeAsync<TParam, TStatus>(this TStatus status,
            Func<TParam, Task> action,
            TParam obj,
            bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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
        /// Invokes the delegate that returns an <see cref="Status"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static TStatus Invoke<TStatus>(this TStatus status, Func<Status> func, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            try
            {
                Status resultStatus = func.Invoke();
                resultStatus.MergeIntoStatus(status);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="Status"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        public static TStatus Do<TStatus>(this TStatus status, Func<Status> func, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            return Invoke(status, func, keepException);
        }
        
        /// <summary>
        /// Invokes the delegate that returns an <see cref="Status"/>, and merges that result status into this status
        /// If an exception occurs, records that exception in this status.
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<TStatus> InvokeAsync<TStatus>(this TStatus status, Func<Task<Status>> func, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            try
            {
                Status resultStatus = await func.Invoke();
                resultStatus.MergeIntoStatus(status);
            }
            catch (Exception ex)
            {
                HandleInvokeException(status, ex, keepException);
            }

            return status;
        }

        /// <summary>
        /// Invokes the delegate that returns an <see cref="Status{T}"/>, merges that result status into this status, and returns the TResult result
        /// If an exception occurs, records that exception in this status and returns default(TResult).
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static TResult InvokeResult<TResult, TStatus>(this TStatus status, Func<Status<TResult>> func, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            try
            {
                Status<TResult> resultStatus = func.Invoke();
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
        /// Invokes the delegate that returns an <see cref="Status{T}"/>, merges that result status into this status, and returns the TResult result
        /// If an exception occurs, records that exception in this status and returns default(TResult).
        /// Returns this status
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<TResult> InvokeResultAsync<TResult, TStatus>(this TStatus status,
            Func<Task<Status<TResult>>> func,
            bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
        {
            try
            {
                Status<TResult> resultStatus = await func.Invoke();
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
        public static TResult InvokeResult<TResult, TStatus>(this TStatus status, Func<TResult> func, bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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
        public static async Task<TResult> InvokeResultAsync<TResult, TStatus>(this TStatus status,
            Func<Task<TResult>> func,
            bool keepException = false)
            where TStatus: AbstractStatus<TStatus>
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