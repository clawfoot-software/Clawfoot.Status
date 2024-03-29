using System;
using System.Threading.Tasks;

namespace Clawfoot.Status
{
    public partial class Status
    {
        /// <summary>
        /// Helper method that invokes the delegate, and if it throws an exception, records it in a returned status
        /// Returns a new status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static Status Invoke(Action action, bool keepException = false)
        {
            Status status = new Status();

            return status.Invoke(action, keepException);
        }

        /// <summary>
        /// Helper method that invokes the delegate, and if it throws an exception, records it in a returned status
        /// Returns a new status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public async static Task<Status> InvokeAsync(Func<Task> action, bool keepException = false)
        {
            Status status = new Status();

            return await status.InvokeAsync(action, keepException);
        }

        /// <summary>
        /// Helper method that invokes the delegate, and if it throws an exception, records it in a returned status
        /// Returns a new status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Status Invoke<TParam>(Action<TParam> action, TParam obj,
            bool keepException = false)
        {
            Status status = new Status();

            return status.Invoke(action, obj, keepException);
        }

        /// <summary>
        /// Helper method that invokes the delegate, and if it throws an exception, records it in a returned status
        /// Returns a new status
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keepException"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async static Task<Status> InvokeAsync<TParam>(Func<TParam, Task> action, TParam obj,
            bool keepException = false)
        {
            Status status = new Status();

            return await status.InvokeAsync(action, obj, keepException);
        }
        
        /// <summary>
        /// Invokes the delegate, and returns a merged status based on the return or the failure of the delegate
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static Status Invoke(Func<Status> func, bool keepException = false)
        {
            Status status = new Status();
            status.Invoke(func, keepException);

            return status;
        }

        /// <summary>
        /// Invokes the delegate, and returns a merges status based on the return or the failure of the delegate
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<Status> Invoke(Func<Task<Status>> func,
            bool keepException = false)
        {
            Status status = new Status();
            await status.InvokeAsync(func, keepException);

            return status;
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in a new Status.
        /// If success, return the result of the delegate as a new Status
        /// </summary>
        /// <param name="func">The delegate</param>
        /// <param name="keepException">To keep the exception in the stus, or just record the error message</param>
        /// <returns></returns>
        public static Status<TResult> InvokeResult<TResult>(Func<TResult> func,
            bool keepException = false)
        {
            try
            {
                TResult result = func.Invoke();
                return Ok<TResult>(result);
            }
            catch (Exception ex)
            {
                if (!keepException)
                {
                    return Status.Error<TResult>(ex.Message);
                }

                Status<TResult> status = new Status<TResult>();
                status.AddException(ex);
                return status;
            }
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in a new Status.
        /// If success, return the result of the delegate as a new Status
        /// </summary>
        /// <param name="func">The delegate</param>
        /// <param name="keepException">To keep the exception in the stus, or just record the error message</param>
        /// <returns></returns>
        public static async Task<Status<TResult>> InvokeResultAsync<TResult>(Func<Task<TResult>> func,
            bool keepException = false)
        {
            try
            {
                TResult result = await func.Invoke();
                return Ok<TResult>(result);
            }
            catch (Exception ex)
            {
                if (!keepException)
                {
                    return Status.Error<TResult>(ex.Message);
                }

                Status<TResult> status = new Status<TResult>();
                status.AddException(ex);
                return status;
            }
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in a new Status.
        /// If success, return the status result of the delegate
        /// </summary>
        /// <param name="func">The delegate</param>
        /// <param name="keepException">To keep the exception in the status, or just record the error message</param>
        /// <returns></returns>
        public static async Task<Status<TResult>> InvokeResultAsync<TResult>(Func<Task<Status<TResult>>> func,
            bool keepException = false)
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception ex)
            {
                if (!keepException)
                {
                    return Status.Error<TResult>(ex.Message);
                }

                Status<TResult> status = new Status<TResult>();
                status.AddException(ex);
                return status;
            }
        }
    }
}