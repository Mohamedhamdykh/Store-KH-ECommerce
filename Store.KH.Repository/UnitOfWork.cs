using Store.KH.Core;
using Store.KH.Core.Entities;
using Store.KH.Core.Repositories.Contract;
using Store.KH.Repository.Data.Contexts;
using Store.KH.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable(); 
        }
        public async Task<int> CompleteAsync() =>  await _context.SaveChangesAsync();
         

        public IGenaricRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
              var repository = new GenaricRepository<TEntity, Tkey>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenaricRepository<TEntity, Tkey>;

        }
    }
}
