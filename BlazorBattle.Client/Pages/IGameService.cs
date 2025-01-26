namespace BlazorBattle.Client.Pages;

public interface IGameService
{
    public int CanvasWidth { get; set; }
    public int CanvasHeight { get; set; }
}

public class GameService : IGameService
{
    public int CanvasWidth { get; set; }
    public int CanvasHeight { get; set; }
}