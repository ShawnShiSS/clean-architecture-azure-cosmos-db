using CleanArchitectureCosmosDB.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetItemsAsync(string query);
        Task<T> GetItemAsync(string id);
        Task AddItemAsync(T item);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
