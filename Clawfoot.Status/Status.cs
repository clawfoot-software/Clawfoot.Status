using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clawfoot.Status;

namespace Clawfoot.Status
{
    /// <summary>
    /// Exists to help with namespace concerns or locals conflicts
    /// </summary>
    public class GenericStatus : Status { }

    public partial class Status : AbstractStatus<Status>
    {
        /// <summary>
        /// Create a generic status
        /// </summary>
        public Status() { }

        /// <summary>
        /// Create a generic status
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        public Status(string successMessage)
        {
            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                _successMessage = successMessage;
            }
        }

        public Status MergeStatuses<T>(Status<T> status)
        {
            return base.MergeStatuses<Status, Status<T>>(status);
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with no errors
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static Status Ok(string successMessage = null)
        {
            return new Status(successMessage);
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with a success message and a result
        /// </summary>
        /// <param name="result">The result of this generic</param>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static Status<TResult> Ok<TResult>(TResult result, string successMessage = null)
        {
            return new Status<TResult>(result, successMessage);
        }

        /// <summary>
        /// Sugar to create a status from an error enum.
        /// </summary>
        /// <typeparam name="TErrorEnum">The error enum type</typeparam>
        /// <param name="errorEnum">The actual error enum value</param>
        /// <param name="errorParams">The string formatting params for the error message, if any</param>
        /// <returns></returns>
        public static Status FromError<TErrorEnum>(TErrorEnum errorEnum, params string[] errorParams)
             where TErrorEnum : Enum
        {
            IError error = Clawfoot.Status.Error.From(errorEnum, errorParams);
            return Status.Error(error);
        }

        /// <summary>
        /// Sugar to create a status from an error enum.
        /// </summary>
        /// <typeparam name="TErrorEnum">The error enum type</typeparam>
        /// <param name="errorEnum">The actual error enum value</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public static Status FromError<TErrorEnum>(TErrorEnum errorEnum, string message, string userMessage = "")
             where TErrorEnum : Enum
        {
            IError error = Clawfoot.Status.Error.From(errorEnum, message, userMessage);
            return Status.Error(error);
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with an error message
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public static Status Error(string message, string userMessage = "")
        {
            Status status = new Status();
            status.AddError(message, userMessage);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with an error message
        /// </summary>
        /// <param name="error">The error model</param>
        /// <returns></returns>
        public static Status Error(IError error)
        {
            Status status = new Status();
            status.AddError(error);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with multiple error messages
        /// </summary>
        /// <param name="errors">The errors</param>
        /// <returns>A New Status</returns>
        public static Status Error(IEnumerable<IError> errors)
        {
            Status status = new Status();
            status.AddErrors(errors);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with an error message
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public static Status<TResult> Error<TResult>(string message, string userMessage = "")
        {
            Status<TResult> status = new Status<TResult>();
            status.AddError(message, userMessage);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with an error message
        /// </summary>
        /// <param name="error">The error model</param>
        /// <returns></returns>
        public static Status<TResult> Error<TResult>(IError error)
        {
            Status<TResult> status = new Status<TResult>();
            status.AddError(error);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with multiple error messages
        /// </summary>
        /// <param name="errors">The errors</param>
        /// <returns>A New Status</returns>
        public static Status<TResult> Error<TResult>(IEnumerable<IError> errors)
        {
            Status<TResult> status = new Status<TResult>();
            status.AddErrors(errors);
            return status;
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with the provided exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns></returns>
        public static Status Error(Exception ex)
        {
            Status status = new Status();
            status.AddException(ex);
            return status;
        }

        /// <summary>
        /// Helper method that creates a generic <see cref="Status{T}"/> with the provided exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns></returns>
        public static Status<TResult> Error<TResult>(Exception ex)
        {
            Status<TResult> status = new Status<TResult>();
            status.AddException(ex);
            return status;
        }
        
        public void Deconstruct(out bool success, out Status status)
        {
            success = Success;
            status = this;
        }
        
        public static implicit operator bool(Status status)
        {
            return status.Success;
        }
    }
}