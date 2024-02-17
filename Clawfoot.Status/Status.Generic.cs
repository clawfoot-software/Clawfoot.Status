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
    public class Status<T> : AbstractStatus<Status<T>>
    {
        private T _result;


        public Status() { }

        /// <summary>
        /// Creates a <see cref="Status{T}"/> with the result and success message
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

        /// <summary>
        /// If this status has a result.
        /// Returns false if there are errors, even if a result has been set
        /// </summary>
        public bool HasResult
        {
            get
            {
                if (EqualityComparer<T>.Default.Equals(_result, default(T))) return false;

                return true;
            }
        }

        /// <summary>
        /// Sets the result of the status
        /// </summary>
        /// <param name="result"></param>
        public Status<T> SetResult(T result)
        {
            Result = result;
            return this;
        }

        public Status<T> MergeStatuses(Status status)
        {
            return base.MergeStatuses<Status<T>, Status>(status);
        }

        /// <summary>
        /// Will combine the result, errors, and exceptions of the provided status with this status. 
        /// If the provided status has a different success message, and no errors, replaces this statuses success message with the provided status.
        /// Will prioritize keeping the status result that exists. If this status doesn't have a result, and the provided status doe, will keep the provided result.
        /// If both statuses have a result, this will prioritize the current statuses result over the provided status.
        /// Returns this status
        /// </summary>
        /// <param name="status">The status to merge into this status</param>
        /// <returns>This status</returns>
        public Status<T> MergeStatuses(Status<T> status)
        {
            base.MergeStatuses(status);

            // If this doesn't have a result, and status does, keep the provided result
            // THis also implicitly means that an existing result on this status is maintained either way
            if (!HasResult && status.HasResult)
            {
                SetResult(status.Result);
            }

            return this;
        }

        /// <summary>
        /// Will combine the errors, exceptions, and result of this status into the provided status using <see cref="MergeStatuses(Status{T})"/>.
        /// Returns the provided status
        /// </summary>
        /// <param name="status">The status to merge into</param>
        /// <returns>The provided status</returns>
        public Status<T> MergeIntoStatus(Status<T> status)
        {
            return status.MergeStatuses(this);
        }

        /// <summary>
        /// Will combine the errors and exceptions of this status into the provided status using <see cref="Status.MergeStatuses(Status)"/>.
        /// Returns the result of this status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public T MergeIntoStatusAndReturnResult(Status status)
        {
            status.MergeStatuses((StatusBase)this);
            return this.Result;
        }

        /// <summary>
        /// Will combine the errors, exceptions, and result of this status into the provided status using <see cref="MergeStatuses(Status{T})"/>.
        /// Returns the result of this status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public T MergeIntoStatusAndReturnResult(Status<T> status)
        {
            status.MergeStatuses(this);
            return this.Result;
        }

        /// <summary>
        /// Converts this generic status to one with the provided result type
        /// Used by the MapTo extension method
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        public Status<TResult> To<TResult>(TResult result)
        {
            Status<TResult> status = new Status<TResult>();
            status.SetResult(result);

            status.MergeStatuses(this);
            return status;

        }
        
        public static implicit operator Status<T>(T value)
        {
            return new Status<T>(value);
        }
        
        public static implicit operator Status(Status<T> generic)
        {
            return (Status)new Status().MergeStatuses(generic);
        }
        
        public static implicit operator Status<T>(Status value)
        {
            return value.As<T>();
        }
        
        /// <summary>
        /// Deconstructs the status into its parts
        /// </summary>
        /// <param name="status"></param>
        /// <param name="result"></param>
        public void Deconstruct(out Status status, out T result)
        {
            status = this;
            result = Result;
        }
        
        /// <summary>
        ///  Deconstructs the status into its parts
        /// </summary>
        /// <param name="status"></param>
        /// <param name="result"></param>
        /// <param name="success"></param>
        public void Deconstruct(out Status status, out T result, out bool success)
        {
            status = this;
            result = Result;
            success = Success;
        }
    }
}


