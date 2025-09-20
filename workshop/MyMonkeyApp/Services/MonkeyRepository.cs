using MyMonkeyApp.Models;

namespace MyMonkeyApp.Services;

/// <summary>
/// 원숭이 데이터를 메모리에서 제공하는 간단한 리포지토리입니다.
/// </summary>
public sealed class MonkeyRepository
{
    private readonly List<Monkey> _monkeys;
    private readonly Random _random = new();

    /// <summary>
    /// 생성자에서 샘플 데이터를 초기화합니다.
    /// </summary>
    public MonkeyRepository()
    {
        _monkeys = new List<Monkey>
        {
            new Monkey { Name = "Capuchin", Location = "Central & South America", Population = 120000 },
            new Monkey { Name = "Howler", Location = "Central & South America", Population = 87000 },
            new Monkey { Name = "Macaque", Location = "Asia", Population = 450000 },
            new Monkey { Name = "Squirrel Monkey", Location = "Central & South America", Population = 230000 },
            new Monkey { Name = "Gibbon", Location = "Southeast Asia", Population = 30000 },
            new Monkey { Name = "Mandrill", Location = "Central Africa", Population = 15000 }
        };
    }

    /// <summary>
    /// 모든 원숭이 목록을 반환합니다.
    /// </summary>
    public IReadOnlyList<Monkey> GetMonkeys() => _monkeys;

    /// <summary>
    /// 이름으로 원숭이를 조회합니다. 없으면 null을 반환합니다.
    /// </summary>
    public Monkey? GetMonkeyByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return _monkeys.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 목록에서 무작위 원숭이를 반환합니다.
    /// </summary>
    public Monkey GetRandomMonkey()
    {
        var idx = _random.Next(0, _monkeys.Count);
        return _monkeys[idx];
    }
}
