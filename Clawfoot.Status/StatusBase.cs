using System;
using System.Collections.Generic;
using System.Linq;

namespace Clawfoot.Status
{
    public abstract class StatusBase
    {
        protected internal const string DEFAULT_SUCCESS_MESSAGE = "Success";
        private protected readonly List<IError> _errors = new List<IError>();
        private protected readonly List<Exception> _exceptions = new List<Exception>();
        private protected string _successMessage = DEFAULT_SUCCESS_MESSAGE;

        /// <summary>
        /// Create a generic status
        /// </summary>
        public StatusBase() { }

        /// <summary>
        /// Create a status
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        public StatusBase(string successMessage)
        {
            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                _successMessage = successMessage;
            }
        }

        /// <summary>
        /// The list of errors of this status
        /// </summary>
        public IEnumerable<IError> Errors => _errors.AsEnumerable();

        /// <summary>
        /// The list of exceptions contained in this status
        /// </summary>
        public IEnumerable<Exception> Exceptions => _exceptions.AsEnumerable();

        /// <summary>
        /// If there are no errors this is true
        /// </summary>
        public bool Success => _errors.Count == 0;

        /// <summary>
        /// If there are errors this is true
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// If the status contains exceptions
        /// </summary>
        public bool HasExceptions => _exceptions.Count > 0;

        /// <summary>
        /// The message of this status, does not combine error messages. Use ToString() instead
        /// </summary>
        public string Message
        {
            get => Success
                ? _successMessage
                : $"Failed with {_errors.Count} error(s)";
            set => _successMessage = value;
        }

        /// <summary>
        /// Combines all error messages into a single string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public string ToString(string seperator = "\n")
        {
            if (_errors.Count > 0)
            {
                return string.Join(seperator, _errors);
            }

            return string.Empty;
        }

        /// <summary>
        /// Combines all user-friendly error messages into a single string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public string ToUserFriendlyString(string seperator = "\n")
        {
            if (_errors.Count > 0)
            {
                return string.Join(seperator, _errors.Select(x => x.ToUserString()).ToArray());
            }

            return string.Empty;
        }
        
        /// <summary>
        /// Will combine the errors and exceptions of the provided status with this status. 
        /// If the provided status has a different success message, and no errors, replaces this statuses success message with the provided status.
        /// Returns this status
        /// </summary>
        /// <param name="status">The status to merge into this status</param>
        /// <returns>This status</returns>
        public virtual StatusBase MergeStatuses<TStatus>(TStatus status)
            where TStatus : StatusBase
        {
            _errors.AddRange(status.Errors);
            _exceptions.AddRange(status.Exceptions);

            if (!HasErrors)
            {
                _successMessage = status.Message;
            }

            return this;
        }
        
        // TODO: THIS MAY BE A MISTAKE
        public virtual TReturn MergeStatuses<TReturn, TStatus>(TStatus status)
            where TReturn : StatusBase
            where TStatus : StatusBase
        {
            return (TReturn)MergeStatuses(status);
        }
    }
    
    public abstract class AbstractStatus<TConcrete> : StatusBase
        where TConcrete : AbstractStatus<TConcrete>
    {
        
        /// <summary>
        /// Create a generic status
        /// </summary>
        public AbstractStatus() { }

        /// <summary>
        /// Create a status
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        public AbstractStatus(string successMessage) : base(successMessage) { }
        
        /// <summary>
        /// Converts this <see cref="Status"/> into an <see cref="Status{T}"/>
        /// </summary>
        /// <typeparam name="T">The Generic Type for the returned status</typeparam>
        /// <returns></returns>
        public virtual Status<T> As<T>()
        {
            Status<T> status = new Status<T>();
            switch (this)
            {
                case Status thisStatus:
                    status.MergeStatuses(thisStatus);
                    break;
                case Status<T> thisStatusT:
                    status.MergeStatuses(thisStatusT);
                    break;
            }

            return status;
        }

        /// <summary>
        /// Creates a <see cref="Status{T}"/>, merges this status into it, and sets the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// /// <param name="result"></param>
        /// <returns>A new status</returns>
        public Status<T> SetResult<T>(T result)
        {
            Status<T> status = new Status<T>();
            switch (this)
            {
                case Status thisStatus:
                    status.MergeStatuses(thisStatus);
                    break;
                case Status<T> thisStatusT:
                    status.MergeStatuses(thisStatusT);
                    break;
            }

            status.SetResult(result);
            return status;
        }
        
        /// <summary>
        /// Will combine the errors and exceptions of this status into the provided status.
        /// Returns the provided status
        /// </summary>
        /// <param name="status">The status to merge into</param>
        /// <returns>The provided status</returns>
        public TConcrete MergeIntoStatus<TStatus>(TStatus status)
            where TStatus : StatusBase
        {
            return (TConcrete)status.MergeStatuses(this);
        }

        /// <summary>
        /// Adds the provided exception to the status.
        /// This also adds the exception message as <see langword="abstract"/>new error
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public TConcrete AddException(Exception ex)
        {
            _exceptions.Add(ex);
            AddError(ex.Message);
            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public TConcrete AddError(string message, string userMessage = "")
        {
            _errors.Add(new Error(message, userMessage));
            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status
        /// </summary>
        /// <param name="error"></param>
        /// <returns>This status</returns>
        public TConcrete AddError(IError error)
        {
            _errors.Add(error);
            return (TConcrete)this;
        }

        /// <summary>
        /// Adds multiple errors to the status
        /// </summary>
        /// <param name="errors"></param>
        /// <returns>This status</returns>
        public TConcrete AddErrors(IEnumerable<IError> errors)
        {
            foreach (IError error in errors)
            {
                _errors.Add(error);
            }

            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status if the item is null
        /// </summary>
        /// <remarks>This only accepts reference types</remarks>
        /// <param name="value">>The value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public TConcrete AddErrorIfNull<T>(T value, string message, string userMessage = "") where T : class
        {
            if (value is null)
            {
                return AddError(message, userMessage);
            }

            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status if the item is null
        /// </summary>
        /// <remarks>This only accepts structs that implement <see cref="Nullable{T}"/></remarks>
        /// <param name="value">>The nullable value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public TConcrete AddErrorIfNull<T>(T? value, string message, string userMessage = "") where T : struct
        {
            if (value is null)
            {
                return AddError(message, userMessage);
            }

            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status if the item is null or is default(T)
        /// </summary>
        /// <remarks>This only accepts reference types</remarks>
        /// <param name="value">>The value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public TConcrete AddErrorIfNullOrDefault<T>(T value, string message, string userMessage = "") where T : class
        {
            if (value is null || value == default(T))
            {
                return AddError(message, userMessage);
            }

            return (TConcrete)this;
        }

        /// <summary>
        /// Adds a new error to the status if the item is null or is default(T)
        /// </summary>
        /// <remarks>This only accepts structs that implement <see cref="Nullable{T}"/></remarks>
        /// <param name="value">>The nullable value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        public TConcrete AddErrorIfNullOrDefault<T>(T? value, string message, string userMessage = "") where T : struct
        {
            if (value is null || value.Value.Equals(default(T)))
            {
                return AddError(message, userMessage);
            }

            return (TConcrete)this;
        }
    }
}