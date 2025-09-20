using MyMonkeyApp.Services;
using MyMonkeyApp.Models;

/// <summary>
/// 간단한 콘솔 앱: 리포지토리에서 원숭이 목록을 받아 테이블로 출력합니다.
/// </summary>
var repo = new MonkeyRepository();
var monkeys = repo.GetMonkeys();

// 테이블 헤더
const int nameWidth = 20;
const int locationWidth = 30;
const int populationWidth = 12;

string PadCenter(string s, int width)
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

