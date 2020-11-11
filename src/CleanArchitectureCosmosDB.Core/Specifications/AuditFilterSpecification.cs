using Ardalis.Specification;

namespace CleanArchitectureCosmosDB.Core.Specifications
{
    public class AuditFilterSpecification : Specification<Entities.Audit>
    {
        /// <summary>
        ///     Search by a matching entity Id
        /// </summary>
        /// <param name="entityId"></param>
        public AuditFilterSpecification(string entityId)
        {
            Query.Where(audit =>
                // Must include EntityId, because it is part of the Partition Key
                audit.EntityId == entityId)
                .OrderByDescending(audit => audit.DateCreatedUTC);
        }

    }
}
