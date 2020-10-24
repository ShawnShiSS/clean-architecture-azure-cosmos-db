using Ardalis.Specification;

namespace CleanArchitectureCosmosDB.Core.Specifications
{
    public class ToDoItemSearchSpecification : Specification<Core.Entities.ToDoItem>
    {
        public ToDoItemSearchSpecification(bool isCompleted)
        {
            // Use Specification Builder 
            Query.Where(item =>
                item.IsCompleted == isCompleted
            );
        }
    }
}
