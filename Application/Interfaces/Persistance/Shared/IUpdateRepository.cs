using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Persistance
{
   public interface IUpdateRepository<T,R>
    {
        void UpdateResource(T resource, R dto);
      
    }
}
