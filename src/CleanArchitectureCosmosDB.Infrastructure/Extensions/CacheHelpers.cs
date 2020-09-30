namespace CleanArchitectureCosmosDB.Infrastructure.Extensions
{
    public static class CacheHelpers
    {
        public static string GenerateToDoItemsCacheKey()
        {
            return "todoitems:";
        }
    }
}
