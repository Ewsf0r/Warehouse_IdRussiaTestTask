using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using WarehouseClient.Interfaces;
using WarehouseModel;

namespace WarehouseClient.Implementation;

public class RecordsCache: IRecordsCache
{
  private static readonly FileInfo _cacheInfo = new FileInfo(AppContext.BaseDirectory + ".cache");
  private int _cacheVersion;
  private Dictionary<string, int> _cache = new ();
  private const string _versionKey = "version";


  public RecordsCache()
  {
    if (!_cacheInfo.Exists)
      _cacheInfo.Create().Dispose();
    using var disposable = Observable
      .FromAsync(async _cancel => await GetCacheAsync(_cancel))
      .Subscribe();
  }

  private async Task GetCacheAsync(CancellationToken cancel)
  {
    await using var readStream = _cacheInfo.OpenRead();
    using var streamReader = new StreamReader(readStream);
    var cacheText = await streamReader.ReadToEndAsync(cancel);
    var jsonCache =  JsonNode.Parse(cacheText)?.GetValue<ClientCache>();

    _cacheVersion = jsonCache.version;
    _cache = jsonCache.records;
  }

  private async Task SaveCacheAsync(CancellationToken cancel)
  {
    await using var writeStream = _cacheInfo.OpenWrite();
    await using var writer = new StreamWriter(writeStream);
    var json = JsonSerializer.Serialize(new ClientCache(_cacheVersion, _cache));
    await writer.WriteAsync(json);
    await writer.FlushAsync(cancel);
  }
  
  public void Add(WarehouseRecord record)
  {
    _cache[record.Id] += record.Count;
    _cacheVersion++;
    using var disposable = Observable
      .FromAsync(async _cancel => await SaveCacheAsync(_cancel))
      .Subscribe();
  }

  public void Clear()
  {
    _cache = new();
    using var disposable = Observable
      .FromAsync(async _cancel => await SaveCacheAsync(_cancel))
      .Subscribe();
  }

  public List<WarehouseRecord> GetAll(out int cacheVersion)
  {
    var result = _cache
      .Select(_kv=>new WarehouseRecord(_kv.Key, _kv.Value))
      .ToList();
    cacheVersion = _cacheVersion;
    return result;
  }
}