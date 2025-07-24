namespace WarehouseModel;

public record ClientCache(int version, Dictionary<string, int> records);