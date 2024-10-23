﻿using MonoTanksClientLogic.Enums;
using MonoTanksClientLogic.Models;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents required methods for agent.
/// </summary>
public interface IAgent
{
    /// <summary>
    /// Represents game start callback.
    /// </summary>
    /// <remarks>
    /// This method will be called in your
    /// agent exactly one at start of each game.
    /// </remarks>
    public void OnGameStarting();

    /// <summary>
    /// Represents next move callback.
    /// </summary>
    /// <remarks>
    /// This method will be called in your
    /// agent every game tick. Agent response will be used
    /// to control your agent in game.
    /// </remarks>
    /// <param name="gameState">
    /// Represents curret game state.
    /// </param>
    /// <returns>
    /// AgentResponse representing next move.
    /// </returns>
    public AgentResponse NextMove(GameState gameState);

    /// <summary>
    /// Represents game end callback.
    /// </summary>
    /// <param name="gameEnd">
    /// Represents final player stats.
    /// </param>
    /// <remarks>
    /// This method will be called in your
    /// agent exactly one at end of each game.
    /// </remarks>
    public void OnGameEnd(GameEnd gameEnd);

    /// <summary>
    /// Represents additional lobby data callback.
    /// </summary>
    /// <param name="lobbyData">
    /// Represents lobby data.
    /// </param>
    /// <remarks>
    /// In case more than one lobby data is sent. This
    /// function will be called.
    /// </remarks>
    public void OnSubsequentLobbyData(LobbyData lobbyData);

    /// <summary>
    /// Represents warning callback.
    /// </summary>
    /// <param name="warning">
    /// Represents warning type.
    /// </param>
    /// <param name="message">
    /// Represents warning message.
    /// </param>
    /// <remarks>
    /// This function will be called every time game server
    /// sends warning to your agent.
    /// </remarks>
    public void OnWarningReceived(Warning warning, string? message);
}
