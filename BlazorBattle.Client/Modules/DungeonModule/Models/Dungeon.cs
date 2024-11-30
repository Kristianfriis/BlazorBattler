namespace BlazorBattle.Client.Modules.DungeonModule.Models;

public class Tile
{
    public TileType Type { get; set; }
    public bool Discovered { get; set; }
    public bool Visited { get; set; }
}

public enum TileType
{
    Empty,
    Wall,
    Treasure,
    Trap
}

public class Dungeon
{
    public const int DungeonWidth = 5;
    public int DungeonLength {get; private set;} = 10;

    public List<List<Tile>> DungeonTiles { get; private set; } = new();
    public (int X, int Y) PlayerPosition { get; private set; } = (1, 0);

    public Dungeon(int dungeonLength)
    {
        DungeonLength = dungeonLength;
        GenerateDungeon();
    }

    public event Action TreasureFound;
    public event Action TrapHit;
    public event Action MonsterHit;

    private void GenerateDungeon()
    {
        var random = new Random(); 
        var tileTypes = Enum.GetValues(typeof(TileType)).Cast<TileType>().ToArray(); 
        
        DungeonTiles.Clear(); 
        
        var firstRow = new List<Tile>(); 

        for (int x = 1; x < DungeonWidth - 1; x++)
        {
            firstRow.Add(new Tile
                {
                    Type = TileType.Empty,
                    Discovered = true,
                    Visited = false
                });
        }

        DungeonTiles.Add(firstRow); 

        for (int y = 1; y < DungeonLength; y++)
        {
            var row = new List<Tile>(); for (int x = 1; x < DungeonWidth - 1; x++)
            {
                row.Add(new Tile
                    {
                        Type = tileTypes[random.Next(tileTypes.Length)],
                        Discovered = y == 0,
                        Visited = false
                    });
            }
            var lastRow = DungeonTiles[y - 1];

            if (lastRow is null)
            { 
                // Replace one wall with an empty tile
                var wallIndex = random.Next(1, DungeonWidth - 2); 
                row[wallIndex].Type = TileType.Empty;
            } 
            else 
            {
            // Ensure each row has at least one empty tile
                if(lastRow.All(tile => tile.Type == TileType.Wall))
                {
                    if (lastRow is null)
                    { 
                        // Replace one wall with an empty tile
                        var wallIndex = random.Next(1, DungeonWidth - 2); 
                        row[wallIndex].Type = TileType.Empty;
                    }
                    else
                    {
                        var wallIndex = random.Next(1, DungeonWidth - 2); 
                        while (lastRow[wallIndex].Type == TileType.Wall)
                        {
                            wallIndex = random.Next(1, DungeonWidth - 2);
                        }
                        row[wallIndex].Type = TileType.Empty;
                    }
                }
                else
                {
                    if(lastRow.All(tile => tile.Type == TileType.Empty))
                    {
                        var wallIndex = random.Next(1, DungeonWidth - 2); 
                        row[wallIndex].Type = TileType.Empty;
                    } 
                    else
                    {
                        var wallIndex = random.Next(DungeonWidth - 2); 

                        while (lastRow[wallIndex].Type == TileType.Wall)
                        {
                            wallIndex = random.Next(1, DungeonWidth - 2);
                        }

                        row[wallIndex].Type = TileType.Empty;
                    }
                }
            }

            DungeonTiles.Add(row);
        }

        // Add the outer walls to each row
        int num = 0;
        foreach (var row in DungeonTiles)
        {
            // Discover the first row
            var discovered = num == 0;
            row.Insert(0, new Tile { Type = TileType.Wall, Discovered = discovered, Visited = false });
            row.Add(new Tile { Type = TileType.Wall, Discovered = discovered, Visited = false });
            num++;
        }

        // Add a final row of walls at the end
        var finalRow = new List<Tile>();
        for (int x = 0; x < DungeonWidth; x++)
        {
            finalRow.Add(new Tile
                {
                    Type = TileType.Wall,
                    Discovered = false,
                    Visited = false
                });
        }
        DungeonTiles.Add(finalRow);
    }

    public void MoveDown()
    {
        if (PlayerPosition.Y < DungeonLength - 1)
        {
            // Mark current tile as visited
            DungeonTiles[PlayerPosition.Y][PlayerPosition.X].Visited = true;

            // Move to next row
            var nextRow = PlayerPosition.Y + 1;

            // Discover next row
            for (int x = 0; x < DungeonWidth; x++)
            {
                DungeonTiles[nextRow][x].Discovered = true;
                if (PlayerPosition.Y == DungeonLength)
                {
                    DungeonTiles[nextRow + 1][x].Discovered = true;
                }
            }

            var nextTile = DungeonTiles[nextRow][PlayerPosition.X];
            var collision = CollisionCheck(nextTile);

            if(!collision)
                PlayerPosition = (PlayerPosition.X, nextRow);

            // Check tile interactions
            var currentTile = DungeonTiles[nextRow][PlayerPosition.X];
            HandleTileInteraction(currentTile);
        }
        else if (PlayerPosition.Y == DungeonLength - 1)
        {
            Console.WriteLine("You reached the end of the dungeon!");
            for (int x = 0; x < DungeonWidth; x++)
            {
                DungeonTiles[DungeonLength][x].Discovered = true;
            }
        }
    }

    public void MoveHorizontal(int direction)
    {
        var newX = PlayerPosition.X + direction;
        if (newX >= 0 && newX < DungeonWidth)
        {
            var row = DungeonTiles[PlayerPosition.Y];
            var nextTile = row[newX];

            var collision = CollisionCheck(nextTile);
            if(!collision)
                PlayerPosition = (newX, PlayerPosition.Y);

            HandleTileInteraction(nextTile);
        }
    }

    public bool CollisionCheck(Tile tile){
        if (tile.Type == TileType.Wall)
        {
            return true;
        }
        return false;
    }

    private void HandleTileInteraction(Tile tile)
    {
        switch (tile.Type)
        {
            case TileType.Treasure:
                TreasureFound?.Invoke();
                break;
            case TileType.Trap:
                TrapHit?.Invoke();
                break;
            case TileType.Wall:
                Console.WriteLine("You hit a wall!");
                break;
        }
    }

}
