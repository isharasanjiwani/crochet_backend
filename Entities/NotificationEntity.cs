using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Entities;

[Table("notifications")]
public class NotificationEntity : BaseEntity
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity User { get; set;} = null!;
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required bool IsRead { get; set; }
    public required string Type { get; set; }
}