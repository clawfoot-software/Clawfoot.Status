using System;
using System.Collections.Generic;
using System.Text;

namespace Clawfoot.Builders.Interfaces
{
    /// <summary>
    /// A generic builder used to build any model type
    /// </summary>
    /// <typeparam name="TModel">The model type</typeparam>
    public interface IGenericBuilder<TModel> : IBuilder<TModel> where TModel : new()
    {
        /// <summary>
        /// Add an action that sets the appropriate model property. Is chainable.
        /// </summary>
        /// <example>
        ///     <code>With(x => x.Name = "Name")</code>
        /// </example>         
        /// <param name="with">The action to set the property</param>
        IGenericBuilder<TModel> With(Action<TModel> with);

        /// <summary>
        /// Applies the preconfigured defaults, or the provided defaults to the model
        /// </summary>
        /// <param name="defaults">Optional default values for the model</param>
        IGenericBuilder<TModel> UseDefaults(IModelDefaultValues<TModel> defaults = null);
    }
}
