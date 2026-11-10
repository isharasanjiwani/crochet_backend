using System;

namespace CrochetWorldApi.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public DateTime SessionExpiresAt { get; set; }
    public DateTime? Revoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsActive => Revoked is null && DateTime.UtcNow < Expires;

    public DateTime ExpiryDate { get; internal set; }
}
