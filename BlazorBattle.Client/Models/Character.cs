namespace BlazorBattle.Client.Models;

public class Character
{
    public Character()
    {
        ExperienceGranted = 10 * Level;
        TotalHitPoints = TotalHitPoints;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int HitPoints { get; set; }
    public int TotalHitPoints { get; set; }
    public int Defence { get; set; }
    public int Energy { get; set; }
    public int Level { get; set; } = 1;
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; } = 100;
    public int ExperienceGranted { get; set; }
    public List<IAction> Actions { get; set; } = new();
    public int BaseSpeed { get; set; } = 1;
    public bool Mob { get; set; }
}