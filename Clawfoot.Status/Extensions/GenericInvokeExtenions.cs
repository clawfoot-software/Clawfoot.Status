using System;
using System.Threading.Tasks;
using Clawfoot.Status.Interfaces;

namespace Clawfoot.Status
{
    public static class GenericInvokeExtenions
    {
        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status and returns null.
        /// If success, sets the status result, and returns the result of the delegate
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static T InvokeResult<T>(this IStatus<T> status, Func<T> func, bool keepException = false)
        {
            try
            {
                T result = func.Invoke();
                status.SetResult(result);
                return result;
            }
            catch (Exception ex)
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

            return default(T);
        }

        /// <summary>
        /// Invokes the delegate, and if it throws an exception, records it in the current status and returns null.
        /// If success, sets the status result, and returns the result of the delegate
        /// </summary>
        /// <param name="func"></param>
        /// <param name="keepException"></param>
        /// <returns></returns>
        public static async Task<T> InvokeResultAsync<T>(this IStatus<T> status, Func<Task<T>> func,
            bool keepException = false)
        {
            try
            {
                T result = await func.Invoke();
                status.SetResult(result);
                return result;
            }
            catch (Exception ex)
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

            return default(T);
        }
    }
}