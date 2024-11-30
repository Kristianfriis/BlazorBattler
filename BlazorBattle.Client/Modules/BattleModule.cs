using BlazorBattle.Client.Models;

namespace BlazorBattle.Client.Modules;

/// <summary>
/// This module handles all the battle related logic.
/// All data in this is transient, and will be lost when the server is restarted.
/// If a battle is not finished, it will be saved in the list of current battles.
/// </summary>
public class BattleModule
{
    private readonly List<Battle> CurrentBattles = new();
    
    public void AddTurnToBattle(Guid battleId, BattleTurn battleTurn)
    {
        var battle = CurrentBattles.FirstOrDefault(x => x.Id == battleId);
        if (battle == null)
            return;
        
        battle.AddBattleTurn(battleTurn);
    }
    
    public BattleTurnResponse HandleTurn(Guid battleId)
    {
        var battle = CurrentBattles.FirstOrDefault(x => x.Id == battleId);
        if (battle == null)
            return new BattleTurnResponse();
     
        //if (battle.IsFinished) handle end of battle
        // battle is ended if all characters of one side are dead
        // the victors will get the experience points from the losers
        
        //Handle if any victors leveled up
        
        // the battle will be removed from the list of current battles
        
        return battle.HandleTurn();
    }
}

public class Battle
{
    public Guid Id { get; set; }
    public List<Character> Characters { get; set; }
    public Character Winner { get; set; }
    public List<string> BattleLog { get; set; }
    public bool IsFinished { get; set; }
    public List<BattleTurn> CurrentBattleTurns { get; set; } = new();
    public int TurnNumber { get; set; }
    
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

public class BattleTurnResponse
{
    public List<Character> Characters { get; set; }
    public List<string> BattleLog { get; set; }
}

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