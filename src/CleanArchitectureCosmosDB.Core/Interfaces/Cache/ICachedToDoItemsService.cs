using CleanArchitectureCosmosDB.Core.Entities;
using System.Collections.Generic;

namespace CleanArchitectureCosmosDB.Core.Interfaces
{
    public interface ICachedToDoItemsService
    {
        IEnumerable<ToDoItem> GetCachedToDoItems();
        void DeleteCachedToDoItems();
        void SetCachedToDoItems(IEnumerable<ToDoItem> entry);
    }
}
