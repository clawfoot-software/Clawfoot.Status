using Clawfoot.Status.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clawfoot.Status
{
    /// <summary>
    /// A generic version of a generic status
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Status<T> :  StatusBase, IStatus<T>
    {
        private T _result;


        public Status() { }

        /// <summary>
        /// Creates a <see cref="GenericStatus{T}"/> with the result and success message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="successMessage"></param>
        public Status(T result, string successMessage = null)
            : base(successMessage)
        {
            _result = result;
        }

        /// <summary>
        /// The returned result
        /// </summary>
        public T Result
        {
            get => _result;
            set => _result = value;
        }

        /// <inheritdoc/>
        public bool HasResult
        {
            get
            {
                if (EqualityComparer<T>.Default.Equals(_result, default(T))) return false;

                return true;
            }
        }

        /// <inheritdoc/>
        public IStatus<T> SetResult(T result)
        {
            Result = result;
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddError(string message, string userMessage = "")
        {
            base.AddError(message, userMessage);
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddError(IError error)
        {
            _errors.Add(error);
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddErrorIfNull<TValue>(TValue value, string message, string userMessage = "") where TValue : class
        {
            base.AddErrorIfNull<TValue>(value, message, userMessage);
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddErrorIfNull<TValue>(TValue? value, string message, string userMessage = "") where TValue : struct
        {
            base.AddErrorIfNull<TValue>(value, message, userMessage);
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddErrorIfNullOrDefault<TValue>(TValue value, string message, string userMessage = "") where TValue : class
        {
            base.AddErrorIfNullOrDefault<TValue>(value, message, userMessage);
            return this;
        }

        /// <inheritdoc/>
        public new IStatus<T> AddErrorIfNullOrDefault<TValue>(TValue? value, string message, string userMessage = "") where TValue : struct
        {
            base.AddErrorIfNullOrDefault<TValue>(value, message, userMessage);
            return this;
        }

        /// <inheritdoc/>
        public IStatus<T> MergeStatuses(IStatus<T> status)
        {
            _errors.AddRange(status.Errors);
            _exceptions.AddRange(status.Exceptions);

            if (!this.HasErrors)
            {
                _successMessage = status.Message;
            }

            // If this doesn't have a result, and status does, keep the provided result
            // THis also implicitly means that an existing result on this status is maintained either way
            if (!this.HasResult && status.HasResult)
            {
                this.SetResult(status.Result);
            }

            return this;
        }

        /// <inheritdoc/>
        public IStatus<T> MergeIntoStatus(IStatus<T> status)
        {
            return status.MergeStatuses(this);
        }

        /// <inheritdoc/>
        public T MergeIntoStatusAndReturnResult(IStatus status)
        {
            status.MergeStatuses(this);
            return this.Result;
        }

        /// <inheritdoc/>
        public T MergeIntoStatusAndReturnResult(IStatus<T> status)
        {
            status.MergeStatuses(this);
            return this.Result;
        }

        /// <inheritdoc/>
        public IStatus<TResult> To<TResult>(TResult result)
        {
            IStatus<TResult> status = new Status<TResult>();
            status.SetResult(result);

            status.MergeStatuses(this);
            return status;

        }
        
        public static implicit operator Status<T>(T value)
        {
            return new Status<T>(value);
        }
        
        public static implicit operator Status<T>(Status value)
        {
            return value.As<T>();
        }
        
        public void Deconstruct(out IStatus status, out T result)
        {
            status = this;
            result = Result;
        }

        public void Deconstruct(out IStatus status, out T result, out bool success)
        {
            status = this;
            result = Result;
            success = Success;
        }
    }
}


