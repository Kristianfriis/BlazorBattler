namespace BlazorBattle.Client.Modules.DungeonModule.Models;

public enum TileTypes
{
    Grass,
    Stone,
    Wall,
    Treasure,
    Void
}

public class GameTile
{
    public TileTypes Type { get; set; }
    public bool IsPassable => Type != TileTypes.Wall;

    public GameTile(TileTypes type)
    {
        Type = type;
    }

    public override string ToString()
    {
        return Type.ToString().ToLower();
    }
}
