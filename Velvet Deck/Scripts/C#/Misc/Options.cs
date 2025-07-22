using Godot;
using System;

public partial class Options : Node
{
    [Export] public Button RoleplayButton;
    [Export] public Button VibrationButton;

    private bool roleplayCardsEnabled = true;
    private bool vibrationsEnabled = true;

    private Node vibrationController;

    public override void _Ready()
    {
        vibrationController = GetNode("/root/VibrationController");

        if (RoleplayButton != null)
        {
            RoleplayButton.Pressed += OnRoleplayButtonPressed;
            UpdateRoleplayButtonVisual();
        }

        if (VibrationButton != null)
        {
            VibrationButton.Pressed += OnVibrationButtonPressed;
            UpdateVibrationButtonVisual();
        }
    }

    private void OnRoleplayButtonPressed()
    {
        roleplayCardsEnabled = !roleplayCardsEnabled;
        UpdateRoleplayButtonVisual();

        if (Components.Instance?.TurnManager != null && !Components.Instance.TurnManager.IsGameStarted())
        {
            if (Components.Instance?.CardManager != null)
            {
                Components.Instance.CardManager.ReinitializeDecksWithOptions();
            }
        }

        GD.Print($"Roleplay cards {(roleplayCardsEnabled ? "enabled" : "disabled")}");
    }

    private void OnVibrationButtonPressed()
    {
        vibrationsEnabled = !vibrationsEnabled;
        UpdateVibrationButtonVisual();

        if (vibrationController != null)
        {
            vibrationController.Call("set_vibration_enabled", vibrationsEnabled);
        }

        GD.Print($"Vibrations {(vibrationsEnabled ? "enabled" : "disabled")}");
    }

    private void UpdateRoleplayButtonVisual()
    {
        if (RoleplayButton == null) return;

        RoleplayButton.Text = roleplayCardsEnabled ? "Roleplay: ON" : "Roleplay: OFF";
        RoleplayButton.Modulate = roleplayCardsEnabled ? Colors.White : Colors.Gray;
    }

    private void UpdateVibrationButtonVisual()
    {
        if (VibrationButton == null) return;

        VibrationButton.Text = vibrationsEnabled ? "Vibration: ON" : "Vibration: OFF";
        VibrationButton.Modulate = vibrationsEnabled ? Colors.White : Colors.Gray;
    }

    public bool AreRoleplayCardsEnabled() => roleplayCardsEnabled;
    public bool AreVibrationsEnabled() => vibrationsEnabled;

    public void ResetToDefaults()
    {
        roleplayCardsEnabled = true;
        vibrationsEnabled = true;

        UpdateRoleplayButtonVisual();
        UpdateVibrationButtonVisual();

        vibrationController.Call("set_vibration_enabled", vibrationsEnabled);
    }
}
