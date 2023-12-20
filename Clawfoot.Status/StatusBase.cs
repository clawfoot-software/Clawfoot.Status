using System;
using System.Collections.Generic;
using System.Linq;
using Clawfoot.Status.Interfaces;

namespace Clawfoot.Status
{
    public abstract class StatusBase : IStatus
    {
        internal const string DefaultSuccessMessage = "Success";
        private protected readonly List<IError> _errors = new List<IError>();
        private protected readonly List<Exception> _exceptions = new List<Exception>();
        private protected string _successMessage = DefaultSuccessMessage;
        
        /// <summary>
        /// Create a generic status
        /// </summary>
        public StatusBase() { }

        /// <summary>
        /// Create a generic status
        /// </summary>
        /// <param name="successMessage">The default success message</param>
        public StatusBase(string successMessage)
        {
            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                _successMessage = successMessage;
            }
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
        public Status<T> As<T>()
        {
            Status<T> status = new Status<T>();
            status.MergeStatuses(this);
            return status;
        }

        /// <inheritdoc/>
        public Status<T> SetResult<T>(T result)
        {
            Status<T> status = new Status<T>();
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
    }
}