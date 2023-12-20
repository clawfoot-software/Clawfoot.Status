using Clawfoot.Status.Interfaces;
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

    public partial class Status : IStatus
    {
        internal const string DefaultSuccessMessage = "Success";
        private protected readonly List<IError> _errors = new List<IError>();
        private protected readonly List<Exception> _exceptions = new List<Exception>();
        private protected string _successMessage = DefaultSuccessMessage;

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

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with no errors
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static IStatus AsSuccess(string successMessage = null)
        {
            return new Status(successMessage);
        }

        /// <summary>
        /// Helper method that creates a <see cref="Status"/> with no errors
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static IStatus Ok(string successMessage = null) => AsSuccess(successMessage);

        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with a success message and a result
        /// </summary>
        /// <param name="result">The result of this generic</param>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static IStatus<TResult> AsSuccess<TResult>(TResult result, string successMessage = null)
        {
            return new Status<TResult>(result, successMessage);
        }
        
        /// <summary>
        /// Helper method that creates a <see cref="Status{T}"/> with a success message and a result
        /// </summary>
        /// <param name="result">The result of this generic</param>
        /// <param name="successMessage">The default success message</param>
        /// <returns></returns>
        public static IStatus<TResult> Ok<TResult>(TResult result, string successMessage = null) => AsSuccess(result, successMessage);

        /// <summary>
        /// Sugar to create a status from an error enum.
        /// </summary>
        /// <typeparam name="TErrorEnum">The error enum type</typeparam>
        /// <param name="errorEnum">The actual error enum value</param>
        /// <param name="errorParams">The string formatting params for the error message, if any</param>
        /// <returns></returns>
        public static IStatus FromError<TErrorEnum>(TErrorEnum errorEnum, params string[] errorParams)
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
        public static IStatus FromError<TErrorEnum>(TErrorEnum errorEnum, string message, string userMessage = "")
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
        public static IStatus AsError(string message, string userMessage = "")
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
        public static IStatus Error(IError error)
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
        public static IStatus Error(IEnumerable<IError> errors)
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
        public static IStatus<TResult> Error<TResult>(string message, string userMessage = "")
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
        public static IStatus<TResult> Error<TResult>(IError error)
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
        public static IStatus<TResult> Error<TResult>(IEnumerable<IError> errors)
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
        public static IStatus Error(Exception ex)
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
        public static IStatus<TResult> Error<TResult>(Exception ex)
        {
            Status<TResult> status = new Status<TResult>();
            status.AddException(ex);
            return status;
        }



        /// <inheritdoc/>
        public IEnumerable<IError> Errors => _errors.AsEnumerable();

        /// <inheritdoc/>
        public IEnumerable<Exception> Exceptions => _exceptions.AsEnumerable();

        /// <inheritdoc/>
        public bool Success => _errors.Count == 0;

        /// <inheritdoc/>
        public bool HasErrors => _errors.Count > 0;

        /// <inheritdoc/>
        public bool HasExceptions => _exceptions.Count > 0;

        /// <inheritdoc/>
        public string Message
        {
            get => Success
                ? _successMessage
                : $"Failed with {_errors.Count} error(s)";
            set => _successMessage = value;
        }

        /// <inheritdoc/>
        public string ToString(string seperator = "\n")
        {
            if (_errors.Count > 0)
            {
                return string.Join(seperator, _errors);
            }
            return string.Empty;
        }

        /// <inheritdoc/>
        public string ToUserFriendlyString(string seperator = "\n")
        {
            if (_errors.Count > 0)
            {
                return string.Join(seperator, _errors.Select(x => x.ToUserString()).ToArray());
            }
            return string.Empty;
        }

        /// <inheritdoc/>
        public IStatus<T> As<T>()
        {
            IStatus<T> status = new Status<T>();
            status.MergeStatuses(this);
            return status;
        }

        /// <inheritdoc/>
        public IStatus<T> SetResult<T>(T result)
        {
            IStatus<T> status = new Status<T>();
            status.MergeStatuses(this);
            status.SetResult(result);
            return status;
        }

        /// <inheritdoc/>
        public IStatus MergeStatuses(IStatus status)
        {
            _errors.AddRange(status.Errors);
            _exceptions.AddRange(status.Exceptions);

            if (!HasErrors)
            {
                _successMessage = status.Message;
            }

            return this;
        }

        /// <inheritdoc/>
        public IStatus MergeIntoStatus(IStatus status)
        {
            return status.MergeStatuses(this);
        }

        /// <inheritdoc/>
        public IStatus AddException(Exception ex)
        {
            _exceptions.Add(ex);
            AddError(ex.Message);
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddError(string message, string userMessage = "")
        {
            _errors.Add(new Error(message, userMessage));
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddError(IError error)
        {
            _errors.Add(error);
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddErrors(IEnumerable<IError> errors)
        {
            foreach(IError error in errors)
            {
                _errors.Add(error);
            }
            
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddErrorIfNull<T>(T value, string message, string userMessage = "") where T : class
        {
            if (value is null)
            {
                return AddError(message, userMessage);
            }
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddErrorIfNull<T>(T? value, string message, string userMessage = "") where T : struct
        {
            if (value is null)
            {
                return AddError(message, userMessage);
            }
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddErrorIfNullOrDefault<T>(T value, string message, string userMessage = "") where T : class
        {
            if (value is null || value == default(T))
            {
                return AddError(message, userMessage);
            }
            return this;
        }

        /// <inheritdoc/>
        public IStatus AddErrorIfNullOrDefault<T>(T? value, string message, string userMessage = "") where T : struct
        {
            if (value is null || value.Value.Equals(default(T)))
            {
                return AddError(message, userMessage);
            }
            return this;
        }
        
        public static implicit operator bool(Status status)
        {
            return status.Success;
        }
    }
}