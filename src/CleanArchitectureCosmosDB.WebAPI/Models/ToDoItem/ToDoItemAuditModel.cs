using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     ToDoItem audit Model
    /// </summary>
    public class ToDoItemAuditModel
    {
        /// <summary>
        ///     Snapshot of the ToDoItem
        /// </summary>
        public ToDoItemModel ToDoItemModel { get; set; }
        /// <summary>
        ///     Date audit record created
        /// </summary>
        public DateTime DateCreatedUTC { get; set; }
    }
}
