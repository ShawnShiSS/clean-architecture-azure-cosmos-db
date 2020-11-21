using Ardalis.Specification;
using CleanArchitectureCosmosDB.Core.Specifications.Interfaces;

namespace CleanArchitectureCosmosDB.Core.Specifications
{
    public class ToDoItemSearchSpecification : Specification<Entities.ToDoItem>
    {
        public ToDoItemSearchSpecification(string title = "",
                                             int pageStart = 0,
                                             int pageSize = 50,
                                             string sortColumn = "title",
                                             SortDirection sortDirection = SortDirection.Ascending
                                             )
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Query.Where(item => item.Title.ToLower().Contains(title.ToLower()));
            }

            // Pagination
            if (pageSize != -1) //Display all entries and disable pagination 
            {
                Query.Skip(pageStart).Take(pageSize);
            }

            // Sort
            switch (sortColumn.ToLower())
            {
                case ("title"):
                    {
                        if (sortDirection == SortDirection.Ascending)
                        {
                            Query.OrderBy(x => x.Title);
                        }
                        else
                        {
                            Query.OrderByDescending(x => x.Title);
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
