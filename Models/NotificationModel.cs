using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetWorldApi.Models;

[Table("notifications")]
public class NotificationModel : BaseModel
{
    public Guid Id { get; set; }
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required bool IsRead { get; set; }
    public required string Type { get; set; }
}