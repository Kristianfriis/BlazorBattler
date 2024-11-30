using BlazorBattle.Client.Models;
using BlazorBattle.Client.Modules;

namespace BlazorBattle.Tests;

public class BattleTests
{
    [Fact]
    public void BattleTest()
    {
        var char1 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player1",
            HitPoints = 100,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        var char2 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player2",
            HitPoints = 100,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        
        var ti = new Battle(new List<Character>(){char1, char2});
        
        var battleTurn = new BattleTurn(char1.Id, char2.Id, char1.Actions[0]);
        ti.CurrentBattleTurns.Add(battleTurn);
        ti.CurrentBattleTurns.Add(new BattleTurn(char2.Id, char1.Id, char2.Actions[0]));
        
        var battleResult = ti.HandleTurn();
        
        Assert.NotEmpty(battleResult.BattleLog);
        Assert.Equal(2, battleResult.BattleLog.Count);
        Assert.Equal(5, char1.Energy);
        Assert.Equal(5, char2.Energy);
        Assert.Equal(90, char1.HitPoints);
        Assert.Equal(90, char2.HitPoints);
    }

    [Fact]
    public void BattleTest_With_Death()
    {
        var char1 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player1",
            HitPoints = 100,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        var char2 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player2",
            HitPoints = 10,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        
        var ti = new Battle(new List<Character>(){char1, char2});
        
        var battleTurn = new BattleTurn(char1.Id, char2.Id, char1.Actions[0]);
        ti.CurrentBattleTurns.Add(battleTurn);
        ti.CurrentBattleTurns.Add(new BattleTurn(char2.Id, char1.Id, char2.Actions[0]));
        
        var battleResult = ti.HandleTurn();
        
        Assert.NotEmpty(battleResult.BattleLog);
        Assert.Equal(2, battleResult.BattleLog.Count);
        Assert.Equal(5, char1.Energy);
        Assert.Equal(10, char2.Energy);
        Assert.Equal(100, char1.HitPoints);
        Assert.Equal(0, char2.HitPoints);
    }
    
    [Fact]
    public void BattleTest_With_Death_With_Speed()
    {
        var char1 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player1",
            HitPoints = 10,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        
        var char2 = new Character()
        {
            Id = Guid.NewGuid(),
            Name = "Player2",
            HitPoints = 10,
            Defence = 10,
            Energy = 10,
            Level = 1,
            Experience = 0,
            Actions = new List<IAction>()
            {
                new TestAttack(100),
                new TestDefence(),
                new TestHeal(),
                new TestEnergy()
            }
        };
        
        var ti = new Battle(new List<Character>(){char1, char2});
        
        var battleTurn = new BattleTurn(char1.Id, char2.Id, char1.Actions[0]);
        ti.CurrentBattleTurns.Add(battleTurn);
        ti.CurrentBattleTurns.Add(new BattleTurn(char2.Id, char1.Id, char2.Actions[0]));
        
        var battleResult = ti.HandleTurn();
        
        Assert.NotEmpty(battleResult.BattleLog);
        Assert.Equal(2, battleResult.BattleLog.Count);
        Assert.Equal(10, char1.Energy);
        Assert.Equal(5, char2.Energy);
        Assert.Equal(0, char1.HitPoints);
        Assert.Equal(10, char2.HitPoints);
    }
}

public class TestAttack : IAction
{
    public int EnergyCost { get; }
    public string Name { get; }
    public ActionType ActionType { get; }
    public int Modifier { get; }
    public int Speed { get; }
    
    public TestAttack(int speed = 10)
    {
        EnergyCost = 5;
        Name = "Test Attack";
        ActionType = ActionType.Attack;
        Modifier = 10;
        Speed = speed;
    }
}

public class TestDefence : IAction
{
    public int EnergyCost { get; }
    public string Name { get; }
    public ActionType ActionType { get; }
    public int Modifier { get; }
    public int Speed { get; }
    
    public TestDefence()
    {
        EnergyCost = 5;
        Name = "Test Defence";
        ActionType = ActionType.Defence;
        Modifier = 10;
        Speed = 10;
    }
}

public class TestHeal : IAction
{
    public int EnergyCost { get; }
    public string Name { get; }
    public ActionType ActionType { get; }
    public int Modifier { get; }
    public int Speed { get; }
    
    public TestHeal()
    {
        EnergyCost = 5;
        Name = "Test Heal";
        ActionType = ActionType.Heal;
        Modifier = 10;
        Speed = 10;
    }
}

public class TestEnergy : IAction
{
    public int EnergyCost { get; }
    public string Name { get; }
    public ActionType ActionType { get; }
    public int Modifier { get; }
    public int Speed { get; }
    
    public TestEnergy()
    {
        EnergyCost = 5;
        Name = "Test Energy";
        ActionType = ActionType.Energy;
        Modifier = 10;
        Speed = 10;
    }
}