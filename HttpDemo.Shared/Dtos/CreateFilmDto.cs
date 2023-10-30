namespace HttpDemo.Shared.Dtos;

public class CreateFilmDto
{
    public required string Name { get; set; }
    public required string[] Categories { get; set; }
}