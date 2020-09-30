using CleanArchitectureCosmosDB.Core.Entities;
using System.Collections.Generic;

namespace CleanArchitectureCosmosDB.Core.Interfaces.Cache
{
    public interface ICachedToDoItemsService
    {
        IEnumerable<ToDoItem> GetCachedToDoItems();
        void DeleteCachedToDoItems();
        void SetCachedToDoItems(IEnumerable<ToDoItem> entry);
    }
}
