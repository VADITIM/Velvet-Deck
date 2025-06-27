using Godot;
using System;

public enum Player
{
    Player1,
    Player2
}

public partial class TurnManager : Node
{
    [Export] Button StartGameButton;

    private Player currentPlayer;
    private bool gameStarted = false;
    private bool turnsAssigned = false;

    public override void _Ready()
    {
        StartGameButton.Pressed += OnStartGamePressed;
    }

    public void OnStartGamePressed()
    {
        GD.Print("Start Game Button Pressed!");

        if (!turnsAssigned)
        {
            AssignFirstPlayer();
            turnsAssigned = true;
            gameStarted = true;

            if (StartGameButton != null)
            {
                StartGameButton.Visible = false;
            }
            UpdatePlayerTurn();
            Components.Instance.DeckManager.OnGameStarted();
        }
        else
        {
            GD.Print("Game already started!");
        }
    }

    private void AssignFirstPlayer()
    {
        Random random = new Random();
        currentPlayer = random.Next(2) == 0 ? Player.Player1 : Player.Player2;
        GD.Print($"Game started! {currentPlayer} goes first.");
    }

    public void NextTurn()
    {
        if (!gameStarted) return;

        currentPlayer = currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        GD.Print($"Turn changed to {currentPlayer}");

        UpdatePlayerTurn();
    }

    private void UpdatePlayerTurn()
    {
        Components.Instance.Animations.AnimateForPlayer(currentPlayer);
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool AreTurnsAssigned()
    {
        return turnsAssigned;
    }
}
