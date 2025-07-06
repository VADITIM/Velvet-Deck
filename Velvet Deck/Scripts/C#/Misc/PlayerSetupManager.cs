using Godot;
using System;

public partial class PlayerSetupManager : Node
{
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    Animations Animations => Components.Instance?.Animations;

    [Export] public Control PlayerSetupScene;

    private bool leftPlayerConfirmed = false;
    private bool rightPlayerConfirmed = false;

    public override void _Ready()
    {
        CallDeferred(nameof(InitializeComponents));
    }

    private void InitializeComponents()
    {
        Animations.RightPlayerNameEdit.Scale = new Vector2(.92f, .92f);
        Animations.RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;

        Animations.LeftPlayerNameEdit.Text = "";
        Animations.RightPlayerNameEdit.Text = "";
        Animations.LeftPlayerNameEdit.PlaceholderText = "Player 1";
        Animations.RightPlayerNameEdit.PlaceholderText = "Player 2";
        Animations.LeftPlayerNameEdit.EditingToggled += OnHomeInputToggle;
        Animations.RightPlayerNameEdit.EditingToggled += OnAwayInputToggle;

        CheckBothPlayersConfirmed();
    }

    public void OnHomeInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            Animations.LeftPlayerNameEdit.SelectAll();

            leftPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Animations.StartLeftPlayerTurn();
            Animations.LeftPlayerNameEdit.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(Animations.LeftPlayerNameEdit.Text.Trim()))
        {
            leftPlayerConfirmed = true;
            Animations.FlipLeftInput();
            Animations.ResetPlayersPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            leftPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }

    public void OnAwayInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            rightPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Animations.StartRightPlayerTurn();
            Animations.RightPlayerNameEdit.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(Animations.RightPlayerNameEdit.Text.Trim()))
        {
            rightPlayerConfirmed = true;
            Animations.FlipRightInput();
            Animations.ResetPlayersPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            rightPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }

    private void CheckBothPlayersConfirmed()
    {
        bool bothConfirmed = leftPlayerConfirmed && rightPlayerConfirmed;
        Animations.AnimateStartButton(bothConfirmed);
    }

    public void ResetConfirmationStates()
    {
        leftPlayerConfirmed = false;
        rightPlayerConfirmed = false;
        CheckBothPlayersConfirmed();
    }

    public string GetHomePlayerName() { return string.IsNullOrEmpty(Animations.LeftPlayerNameEdit.Text.Trim()) ? "Home Player" : Animations.LeftPlayerNameEdit.Text.Trim(); }
    public string GetAwayPlayerName() { return string.IsNullOrEmpty(Animations.RightPlayerNameEdit.Text.Trim()) ? "Away Player" : Animations.RightPlayerNameEdit.Text.Trim(); }
}
