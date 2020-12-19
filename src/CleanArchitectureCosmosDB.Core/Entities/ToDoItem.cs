using CleanArchitectureCosmosDB.Core.Entities.Base;

namespace CleanArchitectureCosmosDB.Core.Entities
{
    public class ToDoItem : BaseEntity
    {
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

        public void MarkComplete()
        {
            IsCompleted = true;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "Done!" : "Not done.";
            return $"{Id}: Status: {status} - {Title}";
        }
    }
}
