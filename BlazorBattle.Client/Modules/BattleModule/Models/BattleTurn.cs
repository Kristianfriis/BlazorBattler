using BlazorBattle.Client.Models;

namespace BlazorBattle.Client.Modules.BattleModule.Models;

public class BattleTurn
{
    public Guid Owner { get; set; }
    public Guid Target { get; set; }
    public IAction Action { get; set; }
    public int Speed { get; set; }
    
    public BattleTurn(Guid attacker, Guid opponent, IAction action)
    {
        Owner = attacker;
        Target = opponent;
        Action = action;
        Speed = action.Speed;
    }
}