using System;
using System.Collections.Generic;
using System.Text;

namespace Clawfoot.Builders.Interfaces
{
    public interface IBuilder<T> where T : new()
    {
        T Build(bool fillInForeignKeysIds = true);
        T Build(T built, bool fillInForeignKeysIds = true);
    }
}
