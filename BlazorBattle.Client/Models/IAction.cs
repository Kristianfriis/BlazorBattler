namespace BlazorBattle.Client.Models;

public interface IAction
{
    public int EnergyCost { get; }
    public string Name { get; }
    public ActionType ActionType { get; }
    public int Modifier { get; }
    public int Speed { get; }
}