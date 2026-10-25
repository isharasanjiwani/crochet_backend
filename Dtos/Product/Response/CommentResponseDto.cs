namespace CrochetWorldApi.Dtos.Product.Response;

public class CommentResponseDto
{
    public required string Name { get; set; }

    public string? ProfileImage { get; set; }

    public required string Comment { get; set; }
}