using BlazorBattle.Client.Modules.BattleModule.Models;

namespace BlazorBattle.Client.Modules.BattleModule;

/// <summary>
/// This module handles all the battle related logic.
/// All data in this is transient, and will be lost when the server is restarted.
/// If a battle is not finished, it will be saved in the list of current battles.
/// </summary>
public class BattleService
{
    private readonly List<Battle> CurrentBattles = new();

    public void AddTurnToBattle(Guid battleId, BattleTurn battleTurn)
    {
        var battle = CurrentBattles.FirstOrDefault(x => x.Id == battleId);
        if (battle is null)
            return;

        battle.AddBattleTurn(battleTurn);
        
        if(battle.IsMobBattle)
            AddMobAction(battle);
    }

    public BattleTurnResponse HandleTurn(Guid battleId)
    {
        var battle = CurrentBattles.FirstOrDefault(x => x.Id == battleId);
        if (battle == null)
            return new BattleTurnResponse();

        
        // // Handle player actions and other battle logic here

        //if (battle.IsFinished) handle end of battle
        // battle is ended if all characters of one side are dead
        // the victors will get the experience points from the losers

        //Handle if any victors leveled up

        // the battle will be removed from the list of current battles

        return battle.HandleTurn();
    }

    private void AddMobAction(Battle battle)
    {
        foreach (var character in battle.Characters)
        {
            if (character.Mob)
            {
                var mobAI = new MobAi(character);
                mobAI.Update();
                // Handle the result of the mob's action (attack, defend, heal, flee)
                var action = mobAI.Update();
                while (action is null)
                {
                    action = mobAI.Update();
                }
                
                var targets = battle.Characters.Where(x => !x.Mob).ToList();
                var randomTarget = targets.Count > 0 ? targets[new Random().Next(0, targets.Count)] : null;
                if (randomTarget is null)
                    continue;
                
                var mobBattleTurn = new BattleTurn(character.Id, randomTarget.Id, action);
                
                battle.AddBattleTurn(mobBattleTurn);
            }
        }
    }
}