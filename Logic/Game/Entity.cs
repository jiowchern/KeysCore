using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal
{
    class Entity : IEntity
    {
        Guid _Id;

        T IEntity.QueryAttrib<T>()
        {
            return default(T);
        }
        
        public Guid Id
        {
            get { return _Id; }
        }
    }
}
