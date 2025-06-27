using Godot;
using System;

public partial class Components : Node
{
    public static Components Instance { get; set; }

    [Export] public DeckManager DeckManager;
    [Export] public TurnManager TurnManager;
    [Export] public CardManager CardManager;
    [Export] public Animations Animations;

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
    }
}
