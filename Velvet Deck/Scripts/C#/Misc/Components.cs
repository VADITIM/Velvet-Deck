using Godot;
using System;

public partial class Components : Node
{
    public static Components Instance { get; set; }

    [Export] public DeckManager DeckManager;
    [Export] public TurnManager TurnManager;
    [Export] public CardManager CardManager;
    [Export] public Animations Animations;
    [Export] public CardAnimations CardAnimations;
    [Export] public PlayerSetupManager PlayerSetupManager;
    [Export] public ButtonHandler ButtonHandler;
    [Export] public TimerController TimerController;
    [Export] public ColorPicker ColorPicker;
    [Export] public JokerManager JokerManager;
    [Export] public Options Options;
    [Export] public Node VibrationController;


    public override void _Ready()
    {
        if (Instance == null)
        { Instance = this; }
        else
        { QueueFree(); }

        DeckManager = GetNode<DeckManager>("Deck Manager");
        TurnManager = GetNode<TurnManager>("Turn Manager");
        CardManager = GetNode<CardManager>("Card Manager");
        Animations = GetNode<Animations>("Animations");
        CardAnimations = GetNode<CardAnimations>("Card Animations");
        ButtonHandler = GetNode<ButtonHandler>("Button Handler");
        Options = GetNode<Options>("Options");
        VibrationController = GetNode("VibrationController");
    }
}
