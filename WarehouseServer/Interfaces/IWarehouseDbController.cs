using WarehouseModel;

namespace WarehouseServer.Interfaces;

public interface IWarehouseDbController
{
  Task CreateDatabaseIfNotExists(CancellationToken cancellationToken);
  Task<int> AddClientOrGetVersion(string clientId, CancellationToken cancellationToken);
  Task AddWarehouseRecord(string clientId, WarehouseRecord record, CancellationToken cancellationToken);
  Task<List<WarehouseRecord>> GetAll(CancellationToken cancellationToken);
}