using Godot;
using System;

public enum Player
{
    Player1,
    Player2
}

public partial class TurnManager : Node
{
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    Animations Animations => Components.Instance?.Animations;
    PlayerSetupManager PlayerSetupManager => Components.Instance?.PlayerSetupManager;
    JokerManager JokerManager => Components.Instance?.JokerManager;

    private Player currentPlayer;
    public bool gameStarted = false;
    public bool turnsAssigned = false;

    public void AssignFirstPlayer()
    {
        Random random = new Random();
        currentPlayer = random.Next(2) == 0 ? Player.Player1 : Player.Player2;
    }

    public void NextTurn()
    {
        if (!gameStarted) return;

        currentPlayer = currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;

        UpdatePlayerTurn();
    }

    public void UpdatePlayerTurn()
    {
        Animations.AnimateForPlayer(currentPlayer);
    }

    public Player GetCurrentPlayer() { return currentPlayer; }

    public bool IsGameStarted() { return gameStarted; }

    public void SetGameStarted(bool started)
    {
        gameStarted = started;
    }

    public bool AreTurnsAssigned() { return turnsAssigned; }

    public void SetStartButtonVisible(bool visible) { ButtonHandler.StartGameButton.Visible = visible; }

    public PlayerSetupManager GetCustomizePlayers() { return PlayerSetupManager as PlayerSetupManager; }
}
