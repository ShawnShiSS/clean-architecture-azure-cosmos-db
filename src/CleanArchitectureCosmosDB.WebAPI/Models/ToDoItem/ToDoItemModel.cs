using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     ToDoItem Api Model
    /// </summary>
    public class ToDoItemModel
    {
        /// <summary>
        ///     ToDoItem Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///     Category which the To-Do-Item belongs to
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        ///     Title of the To-Do-Item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Whether the To-Do-Item is done
        /// </summary>
        public bool IsCompleted { get; private set; }
    }
}
