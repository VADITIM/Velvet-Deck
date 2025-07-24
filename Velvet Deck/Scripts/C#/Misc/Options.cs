using Godot;
using System;

public partial class Options : Node
{
    [Export] public Button RoleplayButton;
    [Export] public Button VibrationButton;

    private bool roleplayCardsEnabled = true;
    public bool vibrationsEnabled = true;

    private Node vibrationController;

    public override void _Ready()
    {
        vibrationController = GetNode("/root/VibrationController");

        RoleplayButton.Pressed += OnRoleplayButtonPressed;
        UpdateRoleplayButtonVisual();

        VibrationButton.Pressed += OnVibrationButtonPressed;
        UpdateVibrationButtonVisual();
    }

    private void OnRoleplayButtonPressed()
    {
        roleplayCardsEnabled = !roleplayCardsEnabled;
        UpdateRoleplayButtonVisual();

        if (!Components.Instance.TurnManager.IsGameStarted())
        {
            Components.Instance.CardManager.ReinitializeDecksWithOptions();
        }
    }

    private void OnVibrationButtonPressed()
    {
        vibrationsEnabled = !vibrationsEnabled;
        UpdateVibrationButtonVisual();

        vibrationController.Call("set_vibration_enabled", vibrationsEnabled);
    }

    public void DisableVibrationForTimer()
    {
        if (!vibrationsEnabled)
        {
            return;
        }

        vibrationsEnabled = !vibrationsEnabled;
        UpdateVibrationButtonVisual();

        vibrationController.Call("set_vibration_enabled", vibrationsEnabled);
    }

    public void EnableVibrationForTimer()
    {
        if (vibrationsEnabled)
        {
            return;
        }

        vibrationsEnabled = !vibrationsEnabled;
        UpdateVibrationButtonVisual();

        vibrationController.Call("set_vibration_enabled", vibrationsEnabled);
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
