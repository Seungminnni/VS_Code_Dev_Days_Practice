using System;

namespace MyMonkeyApp.Models;

/// <summary>
/// 원숭이(종) 상세 정보를 나타냅니다.
/// </summary>
public sealed class MonkeyDetailed
{
    /// <summary>
    /// 고유 식별자 (자동 생성)
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 종 이름
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 서식지/분포
    /// </summary>
    public required string Location { get; set; }

    /// <summary>
    /// 개체수(추정). 음수 불가.
    /// </summary>
    public int Population { get; set; }

    /// <summary>
    /// 보전 상태(선택)
    /// </summary>
    public ConservationStatus? Status { get; set; }

    /// <summary>
    /// 비고/추가 설명(선택)
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// 간단한 보전 상태 열거형
/// </summary>
public enum ConservationStatus
{
    LeastConcern,
    NearThreatened,
    Vulnerable,
    Endangered,
    CriticallyEndangered,
    Extinct
}
