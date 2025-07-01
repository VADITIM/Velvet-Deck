using Godot;

public class Card
{
    public CardType Type { get; set; }
    public string Description { get; set; }
    public string Header { get; set; }
    public int ShotCount { get; set; }
    public float Timer { get; set; }

    public Card(CardType type, string description = "", string header = "", int shotCount = 1, float timer = 0f)
    {
        Type = type;
        Description = description;
        Header = header;
        ShotCount = shotCount;
        Timer = timer;
    }
}
