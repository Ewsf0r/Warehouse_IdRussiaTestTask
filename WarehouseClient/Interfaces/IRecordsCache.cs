using WarehouseModel;

namespace WarehouseClient.Interfaces;

public interface IRecordsCache
{
  void Add(WarehouseRecord record);
  void Clear();
  List<WarehouseRecord> GetAll(out int cacheVersion);
}