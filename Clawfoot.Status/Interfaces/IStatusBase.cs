using System;
using System.Collections.Generic;

namespace Clawfoot.Status.Interfaces
{
    public interface IStatusBase
    {
                /// <summary>
        /// The list of errors of this status
        /// </summary>
        IEnumerable<IError> Errors { get; }

        /// <summary>
        /// The list of exceptions contained in this status
        /// </summary>
        IEnumerable<Exception> Exceptions { get; }

        /// <summary>
        /// If there are no errors this is true
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// If there are errors this is true
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// If the status contains exceptions
        /// </summary>
        bool HasExceptions { get; }

        /// <summary>
        /// The message of this status, does not combine error messages. Use ToString() instead
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Converts this <see cref="IStatus"/> into an <see cref="IStatus{T}"/>
        /// </summary>
        /// <typeparam name="T">The Generic Type for the returned status</typeparam>
        /// <returns></returns>
        Status<T> As<T>();

        /// <summary>
        /// Creates a <see cref="IStatus{T}"/>, merges this status into it, and sets the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// /// <param name="result"></param>
        /// <returns></returns>
        Status<T> SetResult<T>(T result);

        /// <summary>
        /// Will combine the errors and exceptions of the provided status with this status. 
        /// If the provided status has a different success message, and no errors, replaces this statuses success message with the provided status.
        /// Returns this status
        /// </summary>
        /// <param name="status">The status to merge into this status</param>
        /// <returns>This status</returns>
        IStatus MergeStatuses(IStatus status);

        /// <summary>
        /// Will combine the errors and exceptions of this status into the provided status.
        /// Returns the provided status
        /// </summary>
        /// <param name="status">The status to merge into</param>
        /// <returns>The provided status</returns>
        IStatus MergeIntoStatus(IStatus status);

        /// <summary>
        /// Combines all error messages into a single string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        string ToString(string seperator = "\n");

        /// <summary>
        /// Combines all user-friendly error messages into a single string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        string ToUserFriendlyString(string seperator = "\n");

        /// <summary>
        /// Adds the provided exception to the status.
        /// This also adds the exception message as <see langword="abstract"/>new error
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        IStatus AddException(Exception ex);

        /// <summary>
        /// Adds a new error to the status
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        IStatus AddError(string message, string userMessage = "");

        /// <summary>
        /// Adds a new error to the status
        /// </summary>
        /// <param name="error"></param>
        /// <returns>This status</returns>
        IStatus AddError(IError error);

        /// <summary>
        /// Adds multiple errors to the status
        /// </summary>
        /// <param name="error"></param>
        /// <returns>This status</returns>
        IStatus AddErrors(IEnumerable<IError> errors);

        /// <summary>
        /// Adds a new error to the status if the item is null
        /// </summary>
        /// <remarks>This only accepts reference types</remarks>
        /// <param name="value">>The value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        IStatus AddErrorIfNull<T>(T value, string message, string userMessage = "") where T : class;

        /// <summary>
        /// Adds a new error to the status if the item is null
        /// </summary>
        /// <remarks>This only accepts structs that implement <see cref="Nullable{T}"/></remarks>
        /// <param name="value">>The nullable value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        IStatus AddErrorIfNull<T>(T? value, string message, string userMessage = "") where T : struct;

        /// <summary>
        /// Adds a new error to the status if the item is null or is default(T)
        /// </summary>
        /// <remarks>This only accepts reference types</remarks>
        /// <param name="value">>The value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        IStatus AddErrorIfNullOrDefault<T>(T value, string message, string userMessage = "") where T : class;


        /// <summary>
        /// Adds a new error to the status if the item is null or is default(T)
        /// </summary>
        /// <remarks>This only accepts structs that implement <see cref="Nullable{T}"/></remarks>
        /// <param name="value">>The nullable value that is checked</param>
        /// <param name="message">The error message</param>
        /// <param name="userMessage">The user friendly error message</param>
        /// <returns></returns>
        IStatus AddErrorIfNullOrDefault<T>(T? value, string message, string userMessage = "") where T : struct;
    }
}