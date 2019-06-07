using System;
using System.Collections.Generic;
using System.Text;

namespace Clawfoot.Builders.Interfaces
{
    public interface IModelsDefaults
    {
        /// <summary>
        /// Adds the default values for a model to the cache
        /// </summary>
        /// <typeparam name="T">The type of the model</typeparam>
        /// <param name="defaultValues"></param>
        void Add<T>(IModelDefaultValues<T> defaultValues) where T : new();

        /// <summary>
        /// Gets the defaults for the model
        /// </summary>
        /// <typeparam name="T">The type of the model</typeparam>
        /// <returns></returns>
        IModelDefaultValues<T> Get<T>() where T : new();
    }
}
