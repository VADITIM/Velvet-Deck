using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

public partial class DeckManager : Node
{
    private List<Card> mainDeck = new List<Card>();
    private Dictionary<CardType, List<Card>> individualDecks = new Dictionary<CardType, List<Card>>();
    private List<Card> luckyCards = new List<Card>();
    private List<Card> sexCards = new List<Card>();

    private Random random = new Random();
    private float luckyCardChance = 0.15f;

    private Card currentCard = null;
    private int cardsSinceLastSex = 0;
    private int nextSexCardAt = 0;
    private bool deckEmpty = false;

    [Export] public Label TypeLabel { get; set; }
    [Export] public Label HeaderLabel { get; set; }
    [Export] public Label DescriptionLabel { get; set; }
    [Export] public Label LuckyHeaderLabel { get; set; }
    [Export] public Label LuckyDescriptionLabel { get; set; }
    [Export] public TextureRect CardTypeImage { get; set; }
    [Export] public TextureRect ShotCountImage { get; set; }

    [Export] public Panel FrontCard { get; set; }
    [Export] public Panel BackCard { get; set; }
    [Export] public Panel LuckyCard { get; set; }

    [Export] public Button FrontCardButton { get; set; }
    [Export] public Button BackCardButton { get; set; }
    [Export] public Button LuckyCardButton { get; set; }

    [Export] public TimerController TimerController { get; set; }

    private Dictionary<CardType, Texture2D> cardTypeTextures = new Dictionary<CardType, Texture2D>();
    private Dictionary<int, Texture2D> shotCountTextures = new Dictionary<int, Texture2D>();
    private Dictionary<CardType, Color> cardTypeColors = new Dictionary<CardType, Color>();

    private bool isTimerActive = false;
    private bool isCountdownActive = false;

    public override void _Ready()
    {
        LoadAssetsAndColors();
        InitializeDecks();
        ShuffleDecksTogether();
        ConnectButtons();

        ShowNextFrontCard();
    }

    private void ConnectButtons()
    {
        if (FrontCardButton != null)
            FrontCardButton.Pressed += OnFrontCardPressed;
        if (BackCardButton != null)
            BackCardButton.Pressed += OnBackCardPressed;
        if (LuckyCardButton != null)
            LuckyCardButton.Pressed += OnLuckyCardPressed;
    }

    private void LoadAssetsAndColors()
    {
        cardTypeTextures[CardType.Drink] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/drink.png");
        cardTypeTextures[CardType.Foreplay] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/foreplay.png");
        cardTypeTextures[CardType.Fun] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/fun.png");
        cardTypeTextures[CardType.Sex] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/sex.png");
        cardTypeTextures[CardType.Roleplay] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/roleplay.png");
        cardTypeTextures[CardType.Love] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/love.png");
        cardTypeTextures[CardType.Lucky] = GD.Load<Texture2D>("res://Assets/Textures/Card Types/lucky.png");

        shotCountTextures[1] = GD.Load<Texture2D>("res://Assets/Textures/Shots/one-shot.png");
        shotCountTextures[2] = GD.Load<Texture2D>("res://Assets/Textures/Shots/two-shot.png");
        shotCountTextures[3] = GD.Load<Texture2D>("res://Assets/Textures/Shots/three-shot.png");

        cardTypeColors[CardType.Drink] = new Color(0.2f, 0.7f, 0.9f);
        cardTypeColors[CardType.Foreplay] = new Color(0.9f, 0.3f, 0.6f);
        cardTypeColors[CardType.Fun] = new Color(0.9f, 0.8f, 0.2f);
        cardTypeColors[CardType.Sex] = new Color(0.8f, 0.2f, 0.2f);
        cardTypeColors[CardType.Roleplay] = new Color(0.6f, 0.4f, 0.9f);
        cardTypeColors[CardType.Love] = new Color(0.9f, 0.2f, 0.7f);
        cardTypeColors[CardType.Lucky] = new Color(0.2f, 0.9f, 0.3f);
    }

    private void InitializeDecks()
    {
        individualDecks = DeckInitializer.InitializeAllDecks();

        if (individualDecks.ContainsKey(CardType.Lucky))
        {
            luckyCards = new List<Card>(individualDecks[CardType.Lucky]);
            individualDecks.Remove(CardType.Lucky);
        }

        if (individualDecks.ContainsKey(CardType.Sex))
        {
            sexCards = new List<Card>(individualDecks[CardType.Sex]);
            individualDecks.Remove(CardType.Sex);
        }

        nextSexCardAt = random.Next(10, 14);
    }

    private void ShuffleDecksTogether()
    {
        mainDeck.Clear();

        foreach (var deckPair in individualDecks)
        {
            mainDeck.AddRange(deckPair.Value);
        }

        ShuffleDeckWithSexConstraints();

        GD.Print($"Main deck initialized with {mainDeck.Count} cards (Lucky and Sex cards excluded from shuffle)");
    }

    private void ShuffleDeckWithSexConstraints()
    {
        ShuffleDeck(mainDeck);

        List<Card> finalDeck = new List<Card>();
        int cardCount = 0;
        int sexCardIndex = 0;
        int nextSexAt = nextSexCardAt;

        foreach (Card card in mainDeck)
        {
            finalDeck.Add(card);
            cardCount++;

            if (cardCount == nextSexAt && sexCardIndex < sexCards.Count)
            {
                finalDeck.Add(sexCards[sexCardIndex]);
                sexCardIndex++;
                nextSexAt += random.Next(10, 14);
            }
        }

        while (sexCardIndex < sexCards.Count)
        {
            finalDeck.Add(sexCards[sexCardIndex]);
            sexCardIndex++;
        }

        mainDeck = finalDeck;
    }

    private void ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = random.Next(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Card DrawCard()
    {
        if (mainDeck.Count == 0)
        {
            GD.Print("Main deck is empty! No more cards available.");
            deckEmpty = true;
            return null;
        }

        Card drawnCard = mainDeck[0];
        mainDeck.RemoveAt(0);

        if (drawnCard.Type == CardType.Sex)
        {
            cardsSinceLastSex = 0;
        }
        else
        {
            cardsSinceLastSex++;
        }

        GD.Print($"Drew card: {drawnCard.Description} (Type: {drawnCard.Type})");
        return drawnCard;
    }

    public int GetRemainingCards()
    {
        return mainDeck.Count;
    }

    public void ReshuffleDeck()
    {
        mainDeck.Clear();
        cardsSinceLastSex = 0;
        nextSexCardAt = random.Next(10, 14);
        deckEmpty = false;
        ShuffleDecksTogether();
    }

    public List<Card> GetCardsByType(CardType cardType)
    {
        return mainDeck.Where(card => card.Type == cardType).ToList();
    }

    public List<Card> GetOriginalDeckByType(CardType cardType)
    {
        if (cardType == CardType.Lucky)
        {
            return new List<Card>(luckyCards);
        }

        if (cardType == CardType.Sex)
        {
            return new List<Card>(sexCards);
        }

        return individualDecks.ContainsKey(cardType) ? new List<Card>(individualDecks[cardType]) : new List<Card>();
    }

    public bool ShouldShowLuckyCard()
    {
        return random.NextDouble() < luckyCardChance;
    }

    public Card DrawLuckyCard()
    {
        if (luckyCards.Count == 0)
        {
            GD.Print("No lucky cards available!");
            return null;
        }

        int randomIndex = random.Next(luckyCards.Count);
        Card luckyCard = luckyCards[randomIndex];

        GD.Print($"Lucky card appeared: {luckyCard.Description}");
        return luckyCard;
    }

    public int GetLuckyCardCount()
    {
        return luckyCards.Count;
    }

    public void SetLuckyCardChance(float chance)
    {
        luckyCardChance = Mathf.Clamp(chance, 0f, 1f);
    }

    public float GetLuckyCardChance()
    {
        return luckyCardChance;
    }

    public void DisplayFirstCard(Card card)
    {
        if (card == null) return;

        if (TypeLabel != null) TypeLabel.Text = card.Type.ToString();
        if (CardTypeImage != null && cardTypeTextures.ContainsKey(card.Type))
        {
            CardTypeImage.Texture = cardTypeTextures[card.Type];
        }

        if (HeaderLabel != null) HeaderLabel.Text = "";
        if (DescriptionLabel != null) DescriptionLabel.Text = "";
        if (ShotCountImage != null) ShotCountImage.Texture = null;

        SetCardColor(FrontCard, card.Type);

        GD.Print($"Displayed first card: {card.Type}");
    }

    public void DisplaySecondCard(Card card)
    {
        if (card == null) return;

        if (HeaderLabel != null) HeaderLabel.Text = card.Header;
        if (DescriptionLabel != null) DescriptionLabel.Text = card.Description;
        if (ShotCountImage != null && shotCountTextures.ContainsKey(card.ShotCount))
        {
            ShotCountImage.Texture = shotCountTextures[card.ShotCount];
        }

        if (TypeLabel != null) TypeLabel.Text = "";
        if (CardTypeImage != null) CardTypeImage.Texture = null;

        SetCardColor(BackCard, card.Type);

        GD.Print($"Card timer value: {card.Timer}");
        GD.Print($"TimerController is null: {TimerController == null}");

        if (card.Timer > 0f && TimerController != null)
        {
            GD.Print("Starting timer countdown...");
            TimerController.StartCountdown(card.Timer);
            isTimerActive = true;
        }
        else if (card.Timer <= 0f)
        {
            GD.Print("Card has no timer");
        }
        else if (TimerController == null)
        {
            GD.Print("TimerController is null!");
        }

        GD.Print($"Displayed second card: {card.Header} - {card.Description} (Shots: {card.ShotCount}, Timer: {card.Timer})");
    }

    public void DisplayLuckyCard(Card card)
    {
        if (card == null) return;

        if (LuckyHeaderLabel != null) LuckyHeaderLabel.Text = card.Header;
        if (LuckyDescriptionLabel != null) LuckyDescriptionLabel.Text = card.Description;

        SetCardColor(LuckyCard, CardType.Lucky);

        GD.Print($"Displayed lucky card: {card.Header} - {card.Description}");
    }

    public void DisplayCard(Card card)
    {
        if (card == null) return;

        if (HeaderLabel != null) HeaderLabel.Text = card.Header;
        if (TypeLabel != null) TypeLabel.Text = card.Type.ToString();
        if (DescriptionLabel != null) DescriptionLabel.Text = card.Description;

        if (CardTypeImage != null && cardTypeTextures.ContainsKey(card.Type))
        {
            CardTypeImage.Texture = cardTypeTextures[card.Type];
        }

        if (ShotCountImage != null && shotCountTextures.ContainsKey(card.ShotCount))
        {
            ShotCountImage.Texture = shotCountTextures[card.ShotCount];
        }

        GD.Print($"Displayed card: {card.Header} - {card.Description} (Type: {card.Type}, Shots: {card.ShotCount})");
    }

    public Card DrawAndDisplayFirstCard()
    {
        Card drawnCard = DrawCard();
        DisplayFirstCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplaySecondCard()
    {
        Card drawnCard = DrawCard();
        DisplaySecondCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplayCard()
    {
        Card drawnCard = DrawCard();
        DisplayCard(drawnCard);
        return drawnCard;
    }
    public Card DrawAndDisplayLuckyCard()
    {
        Card luckyCard = DrawLuckyCard();
        if (luckyCard != null)
        {
            DisplayLuckyCard(luckyCard);
        }
        return luckyCard;
    }

    private void OnFrontCardPressed()
    {
        if (currentCard != null)
        {
            ShowBackCard();
        }
    }

    private void OnBackCardPressed()
    {
        if (TimerController != null && TimerController.IsAnyTimerRunning())
        {
            GD.Print("Cannot skip card while timer is running");
            return;
        }

        if (TimerController != null)
        {
            TimerController.HideAllTimers();
            isTimerActive = false;
        }

        HideBackCard();

        if (ShouldShowLuckyCard())
        {
            ShowLuckyCard();
        }
        else
        {
            ShowNextFrontCard();
        }
    }

    private void OnLuckyCardPressed()
    {
        HideLuckyCard();
        ShowNextFrontCard();
    }

    private void ShowNextFrontCard()
    {
        currentCard = DrawCard();

        if (currentCard == null)
        {
            GD.Print("No more cards available!");
            return;
        }

        DisplayFirstCard(currentCard);

        if (FrontCard != null) FrontCard.Visible = true;
        if (BackCard != null) BackCard.Visible = false;
        if (LuckyCard != null) LuckyCard.Visible = false;
    }

    private void ShowBackCard()
    {
        if (currentCard != null)
        {
            DisplaySecondCard(currentCard);

            if (FrontCard != null) FrontCard.Visible = false;
            if (BackCard != null) BackCard.Visible = true;
            if (LuckyCard != null) LuckyCard.Visible = false;
        }
    }

    private void HideBackCard()
    {
        if (BackCard != null) BackCard.Visible = false;
    }

    private void ShowLuckyCard()
    {
        Card luckyCard = DrawLuckyCard();
        if (luckyCard != null)
        {
            DisplayLuckyCard(luckyCard);

            if (FrontCard != null) FrontCard.Visible = false;
            if (BackCard != null) BackCard.Visible = false;
            if (LuckyCard != null) LuckyCard.Visible = true;
        }
    }

    private void HideLuckyCard()
    {
        if (LuckyCard != null) LuckyCard.Visible = false;
    }

    public bool IsDeckEmpty()
    {
        return deckEmpty;
    }

    public int GetCardsSinceLastSex()
    {
        return cardsSinceLastSex;
    }

    public int GetNextSexCardAt()
    {
        return nextSexCardAt;
    }

    public int GetSexCardCount()
    {
        return sexCards.Count;
    }

    private void SetCardColor(Panel panel, CardType cardType)
    {
        if (panel == null || !cardTypeColors.ContainsKey(cardType)) return;

        var styleBox = panel.GetThemeStylebox("panel") as StyleBoxFlat;
        if (styleBox == null)
        {
            styleBox = new StyleBoxFlat();
            panel.AddThemeStyleboxOverride("panel", styleBox);
        }
        else
        {
            styleBox = styleBox.Duplicate() as StyleBoxFlat;
            panel.AddThemeStyleboxOverride("panel", styleBox);
        }

        styleBox.BgColor = cardTypeColors[cardType];
    }
}
