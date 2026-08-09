using Store.KH.Core.Entities;
using Store.KH.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
         
        //Create Repository<T> And Return
        IGenaricRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
