using BlazorBattle.Client.Models;
using BlazorBattle.Client.Modules.BattleModule.Models;

namespace BlazorBattle.Client.Modules.BattleModule;

public class MobAi
{
    public MobState CurrentState;
    private readonly Character _mob;

    public MobAi(Character mob)
    {
        _mob = mob;
        CurrentState = MobState.Idle;
    }

    public IAction? Update()
    {
        switch (CurrentState)
        {
            case MobState.Idle:
                return DecideNextAction();
                break;
            case MobState.Attack:
                return PerformAttack();
                break;
            case MobState.Defend:
                return PerformDefend();
                break;
            case MobState.Heal:
                return PerformHeal();
                break;
            case MobState.Flee:
                return PerformFlee();
                break;
            default:
                return null;
        }
    }

    private IAction? DecideNextAction()
    {
        if (_mob.HitPoints < _mob.TotalHitPoints * 0.3)
        {
            CurrentState = MobState.Heal;
        }
        else if (_mob.HitPoints < _mob.TotalHitPoints * 0.5)
        {
            CurrentState = MobState.Defend;
        }
        else
        {
            CurrentState = MobState.Attack;
        }

        return null;
    }

    private IAction? PerformAttack()
    {
        var actionToReturn = _mob.Actions.FirstOrDefault(x => x.ActionType == ActionType.Attack);
        CurrentState = MobState.Idle;
        
        return actionToReturn;
    }

    private IAction? PerformDefend()
    {
        // Defend logic
        var actionToReturn = _mob.Actions.FirstOrDefault(x => x.ActionType == ActionType.Defence);
        if (actionToReturn != null)
        {
            actionToReturn = _mob.Actions.FirstOrDefault(x => x.ActionType == ActionType.Attack);
        }
        CurrentState = MobState.Idle;
        
        return actionToReturn;
    }

    private IAction? PerformHeal()
    {
        // Heal logic
        var actionToReturn = _mob.Actions.FirstOrDefault(x => x.ActionType == ActionType.Heal);
        if (actionToReturn != null)
        {
            actionToReturn = _mob.Actions.FirstOrDefault(x => x.ActionType == ActionType.Attack);
        }
        CurrentState = MobState.Idle;
        
        return actionToReturn;
    }

    private IAction? PerformFlee()
    {
        // Flee logic
        CurrentState = MobState.Idle;
        return null;
    }
}
