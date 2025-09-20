namespace MyMonkeyApp.Models;

/// <summary>
/// 모델: 원숭이에 대한 정보를 담습니다.
/// </summary>
public sealed class Monkey
{
    /// <summary>
    /// 원숭이 이름
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 서식지 또는 위치
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 개체수(대략)
    /// </summary>
    public int Population { get; set; }
}
