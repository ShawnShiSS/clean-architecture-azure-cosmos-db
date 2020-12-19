using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Id { get; set; }
        /// <summary>
        ///     Category which the To-Do-Item belongs to
        /// </summary>
        [Required]
        public string Category { get; set; }
        /// <summary>
        ///     Title of the To-Do-Item
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        ///     Whether the To-Do-Item is done
        /// </summary>
        [Required]
        public bool IsCompleted { get; private set; }
    }
}
