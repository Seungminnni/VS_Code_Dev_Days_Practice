using MyMonkeyApp.Services;
using MyMonkeyApp.Models;
using System.Threading;
using System.Net.Http;
using System.Text.Json;

/// <summary>
/// 대화형 콘솔 앱: MonkeyHelper를 초기화하고 메뉴로 조작합니다.
/// </summary>
static class Program
{
    static async Task<int> Main()
    {
        // 실제 MCP 클라이언트로 초기화합니다.
        var client = new McpMonkeyClient();
        await MonkeyHelper.InitializeAsync(client, CancellationToken.None);

        var arts = new[]
        {
            "  __  __            _       _            \n (  \\/  ) ___  _ __ (_) __ _| | ___  _ __ \n  )    ( / _ \\| '_ \\| |/ _` | |/ _ \\| '__|\n (_/\\/\\_) (_) | | | | | (_| | | (_) | |   ",
            "   .-\"\"\"-.\n  / .===. \\ \n  \\/ 6 6 \\/\n  ( \\___/ )\n___ooo__V__ooo___",
            "   _\n  /_\\  _ __   __ _ _ __   __ _  ___\n / _ \\| '_ \\ / _` | '_ \\ / _` |/ _ \\n/_/ \\_\\ .__/ \\__,_| .__/ \\__,_|\\___/\n      |_|          |_|"
        };

        var rnd = new Random();

        while (true)
        {
            Console.Clear();
            // 랜덤 ASCII 아트 출력
            Console.WriteLine(arts[rnd.Next(0, arts.Length)]);
            Console.WriteLine();

            Console.WriteLine("Monkey Console");
            Console.WriteLine("1) 모든 원숭이 나열");
            Console.WriteLine("2) 이름으로 원숭이 조회");
            Console.WriteLine("3) 무작위 원숭이 가져오기");
            Console.WriteLine("4) 종료");
            Console.Write("선택: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;
            if (!int.TryParse(input.Trim(), out var choice)) continue;

            switch (choice)
            {
                case 1:
                    PrintAll();
                    break;
                case 2:
                    Console.Write("이름을 입력하세요: ");
                    var name = Console.ReadLine() ?? string.Empty;
                    var found = MonkeyHelper.GetByName(name);
                    if (found is null)
                    {
                        Console.WriteLine("찾는 원숭이가 없습니다.");
                    }
                    else
                    {
                        PrintDetails(found);
                    }
                    Pause();
                    break;
                case 3:
                    var random = MonkeyHelper.GetRandom();
                    if (random is null)
                    {
                        Console.WriteLine("원숭이 데이터가 없습니다.");
                    }
                    else
                    {
                        PrintDetails(random);
                        Console.WriteLine($"(이 원숭이는 지금까지 {MonkeyHelper.GetAccessCount(random.Id)}번 선택되었습니다.)");
                    }
                    Pause();
                    break;
                case 4:
                    Console.WriteLine("안녕!");
                    return 0;
                default:
                    break;
            }
        }
    }

    static void PrintAll()
    {
        var monkeys = MonkeyHelper.GetAll();

        const int nameWidth = 20;
        const int locationWidth = 30;
        const int populationWidth = 12;

        static string PadCenter(string s, int width)
        {
            if (s.Length >= width) return s.Substring(0, width);
            int spaces = width - s.Length;
            int padLeft = spaces / 2 + s.Length;
            return s.PadLeft(padLeft).PadRight(width);
        }

        Console.WriteLine();
        Console.WriteLine(PadCenter("Available Monkeys", nameWidth + locationWidth + populationWidth));
        Console.WriteLine(new string('-', nameWidth + locationWidth + populationWidth));
        Console.WriteLine($"{"Name".PadRight(nameWidth)}{"Location".PadRight(locationWidth)}{"Population".PadLeft(populationWidth)}");
        Console.WriteLine(new string('-', nameWidth + locationWidth + populationWidth));

        foreach (var m in monkeys)
        {
            Console.WriteLine($"{m.Name.PadRight(nameWidth)}{m.Location.PadRight(locationWidth)}{m.Population.ToString("N0").PadLeft(populationWidth)}");
        }

        Console.WriteLine(new string('-', nameWidth + locationWidth + populationWidth));
        Console.WriteLine();
        Pause();
    }

    static void PrintDetails(MonkeyDetailed m)
    {
        Console.WriteLine("--- 상세 정보 ---");
        Console.WriteLine($"이름: {m.Name}");
        Console.WriteLine($"서식지: {m.Location}");
        Console.WriteLine($"개체수: {m.Population:N0}");
        Console.WriteLine($"보전 상태: {m.Status}");
        Console.WriteLine($"비고: {m.Notes}");
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("계속하려면 Enter 키를 누르세요...");
        Console.ReadLine();
    }

    /// <summary>
    /// 실제 MCP 서버와 통신하는 클라이언트 구현.
    /// </summary>
    private sealed class McpMonkeyClient : IMcpMonkeyClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:3000"; // MCP 서버 URL (필요 시 변경)

        public McpMonkeyClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        /// <summary>
        /// MCP 서버에서 원숭이 목록을 가져옵니다.
        /// </summary>
        /// <param name="cancellationToken">취소 토큰.</param>
        /// <returns>원숭이 목록.</returns>
        public async Task<IReadOnlyList<MonkeyDetailed>> GetMonkeysAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync("/monkeys", cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var monkeys = JsonSerializer.Deserialize<List<MonkeyDetailed>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MonkeyDetailed>();
                return monkeys;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MCP 서버 오류: {ex.Message}. 로컬 데이터로 폴백합니다.");
                // 폴백: 로컬 데이터 사용
                var repo = new MonkeyRepository();
                var list = repo.GetMonkeys().Select(m => new MonkeyDetailed
                {
                    Id = Guid.NewGuid(),
                    Name = m.Name,
                    Location = m.Location,
                    Population = m.Population,
                    Status = null,
                    Notes = null
                }).ToList();
                return list;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}

