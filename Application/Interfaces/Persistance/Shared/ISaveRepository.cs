using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistance.Shared
{
   public interface ISaveRepository 
    {
        Task<bool> SaveAsync();
    }
}
