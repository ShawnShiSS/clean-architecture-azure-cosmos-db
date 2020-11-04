using CleanArchitectureCosmosDB.Core.Entities;

namespace CleanArchitectureCosmosDB.Core.Interfaces.Persistence
{
    public interface IAuditRepository : IRepository<Audit>
    {
    }
}
