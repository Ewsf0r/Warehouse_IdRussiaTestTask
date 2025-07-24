using WarehouseModel;

namespace WarehouseClient.Interfaces;

public interface IWarehouseHttpClient
{
  Task<bool> UpdateServer(List<WarehouseRecord> warehouseRecords, int version, CancellationToken cancellationToken);
  Task<int> CheckServerVersion(string selfId, CancellationToken cancellationToken);
}