using Ardalis.Specification;

namespace CleanArchitectureCosmosDB.Core.Specifications.Base
{
    /// <summary>
    ///     Specification Evaluator for Cosmos DB.
    ///     The evaluator implements methods to translate specifications into Cosmos DB IQueryables, which then allows us to build queryables with filters, predicates etc. to query data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CosmosDbSpecificationEvaluator<T> : SpecificationEvaluatorBase<T> where T : class
    {
    }
}
