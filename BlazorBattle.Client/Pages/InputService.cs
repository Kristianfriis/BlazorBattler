namespace BlazorBattle.Client.Pages;

public interface IInputService
{
    public Direction Direction { get; set; }
}

public enum Direction
{
    None = 0,
    Up,
    Down,
    Right,
    Left
}

public class InputService : IInputService
{
    public Direction Direction { get; set; }
}