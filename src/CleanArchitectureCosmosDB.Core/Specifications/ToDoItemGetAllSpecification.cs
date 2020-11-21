using Ardalis.Specification;

namespace CleanArchitectureCosmosDB.Core.Specifications
{
    public class ToDoItemGetAllSpecification : Specification<Core.Entities.ToDoItem>
    {
        public ToDoItemGetAllSpecification(bool isCompleted)
        {
            // Use Specification Builder 
            Query.Where(item =>
                item.IsCompleted == isCompleted
            );
        }
    }
}
