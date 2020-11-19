using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public ToDoItemModel ToDoItemModel { get; set; }
        /// <summary>
        ///     Date audit record created
        /// </summary>
        [Required]
        public DateTime DateCreatedUTC { get; set; }
    }
}
