using CleanArchitectureCosmosDB.Core.Entities;
using Xunit;

namespace CleanArchitectureCosmosDB.UnitTests.Core.Entities
{
    public class ToDoItemTest
    {
        [Fact]
        public void SetIsCompletedToTrue()
        {
            ToDoItem item = new ToDoItem()
            {
                Category = "UnitTest",
                Title = "Mark me as completed",
                //IsCompleted = false // private property that can only be set by MarkComplete method            
            };

            item.MarkComplete();

            Assert.True(item.IsCompleted);

        }
    }
}
