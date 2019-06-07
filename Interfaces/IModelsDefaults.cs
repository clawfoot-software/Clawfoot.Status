using System;
using System.Collections.Generic;
using System.Text;

namespace Clawfoot.Builders.Interfaces
{
    public interface IModelsDefaults
    {
        void Add<T>(IModelDefaultValues<T> defaultValues) where T : new();
        IModelDefaultValues<T> Get<T>() where T : new();
    }
}
