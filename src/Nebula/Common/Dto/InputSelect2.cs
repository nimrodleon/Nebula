namespace Nebula.Common.Dto;

public interface ISelect2
{
    string Id { get; set; }
    string Text { get; set; }
}

public class InputSelect2
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
