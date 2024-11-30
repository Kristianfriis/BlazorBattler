using BlazorBattle.Client.Models;

namespace BlazorBattle.Client.Modules.BattleModule.Models;

public class Battle
{
    public Guid Id { get; set; }
    public List<Character> Characters { get; set; }
    public Character Winner { get; set; }
    public List<string> BattleLog { get; set; }
    public bool IsFinished { get; set; }
    public List<BattleTurn> CurrentBattleTurns { get; set; } = new();
    public int TurnNumber { get; set; }
    
    public bool IsMobBattle => Characters.FirstOrDefault(x => x.Mob) is not null;
    
    public Battle(List<Character> characters)
    {
        Id = Guid.NewGuid();
        Characters = characters;
        BattleLog = new List<string>();
    }

    public BattleTurnResponse HandleTurn()
    {
        TurnNumber++;
        
        var result = new BattleTurnResponse();
        result.BattleLog = new List<string>();
        
        foreach (var currentBattleTurn in CurrentBattleTurns)
        {
            var owner = Characters.FirstOrDefault(x => x.Id == currentBattleTurn.Owner);
            if (owner == null)
                continue;
            
            currentBattleTurn.Speed += owner.BaseSpeed;
        }
        
        var sortedTurns = CurrentBattleTurns.OrderByDescending(x => x.Speed).ToList();

        foreach (var currentBattleTurn in sortedTurns)
        {
            var target = Characters.FirstOrDefault(x => x.Id == currentBattleTurn.Target);
            var owner = Characters.FirstOrDefault(x => x.Id == currentBattleTurn.Owner);
            
            switch (currentBattleTurn.Action.ActionType)
            {
                case ActionType.Attack:
                    HandleAttack(currentBattleTurn, target, owner, result);
                    break;
                case ActionType.Defence:
                    HandleDefence(currentBattleTurn, target, owner, result);
                    break;
                case ActionType.Heal:
                    HandleHeal(currentBattleTurn, target, owner, result);
                    break;
                case ActionType.Energy:
                    HandleEnergy(currentBattleTurn, target, owner, result);
                    break;
                case ActionType.Buff:
                    break;
            }
        }   

        CurrentBattleTurns = new List<BattleTurn>();
        
        BattleLog.AddRange(result.BattleLog);
        
        result.Characters = Characters;
        
        return result;
    }

    private void HandleEnergy(BattleTurn currentBattleTurn, Character? target, Character? owner,
        BattleTurnResponse result)
    {
        if (!CanPerformAction(target, owner, currentBattleTurn.Action, result))
            return;
        
        owner.Energy -= currentBattleTurn.Action.EnergyCost;
        target.Energy += currentBattleTurn.Action.Modifier;
        
        BattleLog.Add($"{owner.Name} gave {target.Name} {currentBattleTurn.Action.Modifier} energy");
    }

    private void HandleHeal(BattleTurn currentBattleTurn, Character? target, Character? owner,
        BattleTurnResponse result)
    {
        if (!CanPerformAction(target, owner, currentBattleTurn.Action, result))
            return;
        
        owner.Energy -= currentBattleTurn.Action.EnergyCost;
        target.HitPoints += currentBattleTurn.Action.Modifier;
        
        result.BattleLog.Add($"{owner.Name} healed {target.Name} for {currentBattleTurn.Action.Modifier} health");
    }

    private void HandleDefence(BattleTurn currentBattleTurn, Character? target, Character? owner,
        BattleTurnResponse result)
    {
        if (!CanPerformAction(target, owner, currentBattleTurn.Action, result))
            return;
        
        throw new NotImplementedException();
    }

    private void HandleAttack(BattleTurn currentBattleTurn, Character? target, Character? owner,
        BattleTurnResponse result)
    {
        if (!CanPerformAction(target, owner, currentBattleTurn.Action, result))
            return;
            
        owner.Energy -= currentBattleTurn.Action.EnergyCost;
        target.HitPoints -= currentBattleTurn.Action.Modifier; 
        ;
        result.BattleLog.Add($"{owner.Name} attacked {target.Name} for {currentBattleTurn.Action.Modifier} damage");
        
        if (target.HitPoints <= 0)
        {
            result.BattleLog.Add($"{target.Name} died");
        }
        
        if (owner.Energy <= 0)
        {
            result.BattleLog.Add($"{owner.Name} is out of energy");
        }
        
    }
    
    private bool CanPerformAction(Character target, Character owner, IAction action, BattleTurnResponse result)
    {
        if (target.HitPoints <= 0)
        {
            result.BattleLog.Add($"{target.Name} is already dead");
            return false;
        }
        
        if (owner.HitPoints <= 0)
        {
            return false;
        }

        if (owner.Energy < action.EnergyCost)
        {
            result.BattleLog.Add($"{owner.Name} does not have enough energy to perform this action");
            return false;
        }

        return true;
    }

    public void AddBattleTurn(BattleTurn battleTurn)
    {
        CurrentBattleTurns.Add(battleTurn);
    }
}