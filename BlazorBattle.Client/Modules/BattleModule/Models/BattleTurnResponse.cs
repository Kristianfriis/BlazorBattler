using BlazorBattle.Client.Models;

namespace BlazorBattle.Client.Modules.BattleModule.Models;

public class BattleTurnResponse
{
    public List<Character> Characters { get; set; }
    public List<string> BattleLog { get; set; }
}