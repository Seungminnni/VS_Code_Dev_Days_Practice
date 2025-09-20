using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyMonkeyApp.Models;

namespace MyMonkeyApp.Services;

/// <summary>
/// MCP 서버에서 원숭이 데이터를 비동기로 가져오는 클라이언트 계약입니다.
/// </summary>
public interface IMcpMonkeyClient
{
    /// <summary>
    /// MCP 서버에서 원숭이 목록을 비동기로 가져옵니다.
    /// </summary>
    Task<IReadOnlyList<MonkeyDetailed>> GetMonkeysAsync(CancellationToken cancellationToken = default);
}
