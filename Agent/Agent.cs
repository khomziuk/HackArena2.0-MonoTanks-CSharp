using MonoTanksClientLogic;
using MonoTanksClientLogic.Enums;
using MonoTanksClientLogic.Models;

namespace Agent;

public class Agent : IAgent
{
    private string myId;

    public Agent(LobbyData lobbyData)
    {
        this.myId = lobbyData.PlayerId;
    }

    public void OnSubsequentLobbyData(LobbyData lobbyData) { }

    public AgentResponse NextMove(GameState gameState)
    {
        Console.WriteLine("Map:");
        for (int y = 0; y < gameState.Map.GetLength(0); y++)
        {
            for (int x = 0; x < gameState.Map.GetLength(1); x++)
            {
                Tile tile = gameState.Map[y, x];
                char symbol = ' ';
                if (tile.IsVisible)
                {
                    symbol = '.';
                }

                if (tile.ZoneIndex != null)
                {
                    int zoneIndex = (int)gameState.Map[y, x].ZoneIndex!;
                    symbol = tile.IsVisible ? (char)zoneIndex : (char)(zoneIndex + 32);
                }

                foreach (var entity in gameState.Map[y, x].Entities)
                {
                    if (entity is Tile.Wall)
                    {
                        symbol = '#';
                    }
                    else if (entity is Tile.Item item)
                    {
                        symbol = item.ItemType switch
                        {
                            SecondaryItemType.Laser => 'L',
                            SecondaryItemType.Mine => 'M',
                            SecondaryItemType.Radar => 'R',
                            SecondaryItemType.DoubleBullet => 'D',
                            SecondaryItemType.Unknown => '?',
                            _ => throw new NotSupportedException()
                        };
                    }
                    else if (entity is Tile.OwnTank ownTank)
                    {
                        symbol = ownTank.Direction switch
                        {
                            Direction.Down => 'V',
                            Direction.Left => '<',
                            Direction.Up => '^',
                            Direction.Right => '>',
                            _ => throw new NotSupportedException()
                        };

                        // There is also turret direction
                        // ownTank.Turret.Direction
                    }
                    else if (entity is Tile.EnemyTank)
                    {
                        symbol = '#';
                    }
                    else if (entity is Tile.Bullet bullet)
                    {
                        if (bullet.Type == BulletType.Basic)
                        {
                            symbol = bullet.Direction switch
                            {
                                Direction.Down => '↓',
                                Direction.Left => '←',
                                Direction.Up => '↑',
                                Direction.Right => '→',
                                _ => throw new NotSupportedException()
                            };
                        }
                        else
                        {
                            symbol = bullet.Direction switch
                            {
                                Direction.Down => '⇊',
                                Direction.Left => '⇇',
                                Direction.Up => '⇈',
                                Direction.Right => '⇉',
                                _ => throw new NotSupportedException()
                            };
                        }
                    }
                    else if (entity is Tile.Laser laser)
                    {
                        symbol = laser.Orientation switch
                        {
                            LaserDirection.Horizontal => '-',
                            LaserDirection.Vertical => '|',
                            _ => throw new NotSupportedException()
                        };
                    }
                    else if (entity is Tile.Mine mine)
                    {
                        symbol = 'X';
                    }
                }
                Console.Write(symbol);
                Console.Write(' ');
            }
            Console.Write("\n");
        }

        //Bot that randomly choses one of all possible agent responses.
        var rand = new Random();
        return rand.Next(0, 18) switch
        {
            0 => AgentResponse.Pass(),
            1 => AgentResponse.Move(MovementDirection.Backward),
            2 => AgentResponse.Move(MovementDirection.Forward),
            3 => AgentResponse.Rotate(null, null),
            4 => AgentResponse.Rotate(null, Rotation.Left),
            5 => AgentResponse.Rotate(null, Rotation.Right),
            6 => AgentResponse.Rotate(Rotation.Left, null),
            7 => AgentResponse.Rotate(Rotation.Left, Rotation.Left),
            8 => AgentResponse.Rotate(Rotation.Left, Rotation.Right),
            9 => AgentResponse.Rotate(Rotation.Right, null),
            10 => AgentResponse.Rotate(Rotation.Right, null),
            11 => AgentResponse.Rotate(Rotation.Right, Rotation.Left),
            12 => AgentResponse.Rotate(Rotation.Right, Rotation.Right),
            13 => AgentResponse.UseAbility(AbilityType.DropMine),
            14 => AgentResponse.UseAbility(AbilityType.FireBullet),
            15 => AgentResponse.UseAbility(AbilityType.FireDoubleBullet),
            16 => AgentResponse.UseAbility(AbilityType.UseLaser),
            17 => AgentResponse.UseAbility(AbilityType.UseRadar),
            _ => throw new NotSupportedException(),
        };
    }

    public void OnGameEnd(GameEnd gameEnd)
    {
        // Define what your program should do when game is finished.
        GameEndPlayer winner = gameEnd.Players[0];
        if (winner.Id == this.myId)
        {
            Console.WriteLine("I won!");
        }

        foreach (var player in gameEnd.Players)
        {
            Console.WriteLine($"{player.Nickname} - {player.Score}");
        }
    }

    public void OnGameStarting()
    {
        // Define what your program should do when game is starting.
    }

    public void OnWarningReceived(Warning warning, string? message)
    {
        // Define what your program should do when game is warning is recieved.

        switch (warning)
        {
            case Warning.PlayerAlreadyMadeActionWarning:
                {
                    Console.WriteLine("Player already made action warning");
                    break;
                }
            case Warning.SlowResponseWarning:
                {
                    Console.WriteLine("Slow response warning");
                    break;
                }
            case Warning.ActionIgnoredDueToDeadWarning:
                {
                    Console.WriteLine("Action ignored due to dead warning");
                    break;
                }
            case Warning.CustomWarning:
                {
                    Console.WriteLine($"Custom warning: {message ?? "no message"}");
                    break;
                }
        }
    }
}