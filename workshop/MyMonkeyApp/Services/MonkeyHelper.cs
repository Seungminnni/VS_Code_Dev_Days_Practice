using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyMonkeyApp.Models;

namespace MyMonkeyApp.Services;

/// <summary>
/// 원숭이 데이터 컬렉션을 관리하는 정적 헬퍼 클래스입니다.
/// MCP 서버에서 데이터를 가져오고, 조회/무작위 선택 시 접근 횟수를 추적합니다.
/// </summary>
public static class MonkeyHelper
{
    private static readonly object _sync = new();
    private static IReadOnlyList<MonkeyDetailed> _cache = Array.Empty<MonkeyDetailed>();
    private static readonly Dictionary<Guid, int> _accessCounts = new();

    /// <summary>
    /// MCP 클라이언트를 사용해 초기 데이터를 비동기로 로드합니다. 실패 시 로컬 리포지토리를 사용합니다.
    /// </summary>
    public static async Task InitializeAsync(IMcpMonkeyClient client, CancellationToken cancellationToken = default)
    {
        if (client is null) throw new ArgumentNullException(nameof(client));

        try
        {
            var list = await client.GetMonkeysAsync(cancellationToken).ConfigureAwait(false);
            lock (_sync)
            {
                _cache = list ?? Array.Empty<MonkeyDetailed>();
                _accessCounts.Clear();
                foreach (var m in _cache)
                    _accessCounts[m.Id] = 0;
            }
        }
        catch
        {
            // MCP 호출이 실패하면 기존 로컬 리포지토리를 사용합니다.
            var repo = new MonkeyRepository();
            var local = repo.GetMonkeys().Select(m => new MonkeyDetailed
            {
                Id = Guid.NewGuid(),
                Name = m.Name,
                Location = m.Location,
                Population = m.Population,
                Status = null,
                Notes = null
            }).ToList();

            lock (_sync)
            {
                _cache = local;
                _accessCounts.Clear();
                foreach (var m in _cache)
                    _accessCounts[m.Id] = 0;
            }
        }
    }

    /// <summary>
    /// 모든 원숭이를 반환합니다.
    /// </summary>
    public static IReadOnlyList<MonkeyDetailed> GetAll()
    {
        lock (_sync) return _cache;
    }

    /// <summary>
    /// 이름으로 원숭이를 찾습니다. 찾으면 접근 횟수를 증가시킵니다.
    /// </summary>
    public static MonkeyDetailed? GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        lock (_sync)
        {
            var found = _cache.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                _accessCounts[found.Id] = _accessCounts.GetValueOrDefault(found.Id) + 1;
            }

            return found;
        }
    }

    /// <summary>
    /// 무작위 원숭이를 반환하고 해당 원숭이의 접근 횟수를 증가시킵니다.
    /// </summary>
    public static MonkeyDetailed? GetRandom()
    {
        lock (_sync)
        {
            if (_cache == null || _cache.Count == 0) return null;
            var rnd = new Random();
            var idx = rnd.Next(0, _cache.Count);
            var item = _cache[idx];
            _accessCounts[item.Id] = _accessCounts.GetValueOrDefault(item.Id) + 1;
            return item;
        }
    }

    /// <summary>
    /// 특정 원숭이의 접근 횟수를 반환합니다.
    /// </summary>
    public static int GetAccessCount(Guid id)
    {
        lock (_sync) return _accessCounts.GetValueOrDefault(id);
    }

    /// <summary>
    /// 데이터를 새로고침합니다.
    /// </summary>
    public static async Task RefreshAsync(IMcpMonkeyClient client, CancellationToken cancellationToken = default)
    {
        await InitializeAsync(client, cancellationToken).ConfigureAwait(false);
    }
}
