using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardManager : Node
{
    private List<Card> mainDeck = new List<Card>();
    private Dictionary<CardType, List<Card>> individualDecks = new Dictionary<CardType, List<Card>>();
    private List<Card> luckyCards = new List<Card>();
    private List<Card> sexCards = new List<Card>();

    private Random random = new Random();
    private float luckyCardChance = 0.07f;

    private int cardsSinceLastSex = 0;
    private int nextSexCardAt = 0;
    private bool deckEmpty = false;

    private Dictionary<CardType, Texture2D> cardTypeTextures = new Dictionary<CardType, Texture2D>();
    private Dictionary<int, Texture2D> shotCountTextures = new Dictionary<int, Texture2D>();
    private Dictionary<CardType, Color> cardTypeColors = new Dictionary<CardType, Color>();

    public override void _Ready()
    {
        LoadAssetsAndColors();
        InitializeDecks();
        ShuffleDecksTogether();
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

        cardTypeColors[CardType.Drink] = Color.FromHtml("#00a3ff");
        cardTypeColors[CardType.Fun] = Color.FromHtml("#f8ce57");
        cardTypeColors[CardType.Roleplay] = Color.FromHtml("#a441ff");
        cardTypeColors[CardType.Love] = Color.FromHtml("#ff4448");
        cardTypeColors[CardType.Foreplay] = Color.FromHtml("#e165ca");
        cardTypeColors[CardType.Sex] = Color.FromHtml("#000000");
        cardTypeColors[CardType.Lucky] = Color.FromHtml("#12e885");
    }

    private void InitializeDecks()
    {
        individualDecks = DeckInitializer.InitializeAllDecks();

        // Check if roleplay cards should be excluded
        if (Components.Instance?.Options != null && !Components.Instance.Options.AreRoleplayCardsEnabled())
        {
            if (individualDecks.ContainsKey(CardType.Roleplay))
            {
                individualDecks.Remove(CardType.Roleplay);
                GD.Print("Roleplay cards excluded from deck");
            }
        }

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

        nextSexCardAt = Math.Max(random.Next(10, 14), 2);
    }

    private void ShuffleDecksTogether()
    {
        mainDeck.Clear();

        foreach (var deckPair in individualDecks)
        {
            foreach (var card in deckPair.Value)
            {
                mainDeck.Add(card);
            }
        }

        ShuffleDeckWithSexConstraints();
    }

    private void ShuffleDeckWithSexConstraints()
    {
        ShuffleDeck(mainDeck);

        List<Card> finalDeck = new List<Card>();
        List<Card> foreplayCards = mainDeck.Where(card => card.Type == CardType.Foreplay).ToList();
        List<Card> otherCards = mainDeck.Where(card => card.Type != CardType.Foreplay).ToList();

        int sexCardIndex = 0;
        int cardCount = 0;
        int nextSexAt = Math.Max(nextSexCardAt, 2);
        int foreplayUsed = 0;

        bool[] playerHadForeplay = new bool[2];
        int currentPlayerIndex = 0;

        while (cardCount < nextSexAt || sexCardIndex < sexCards.Count || foreplayCards.Count > 0 || otherCards.Count > 0)
        {
            if (cardCount == nextSexAt && sexCardIndex < sexCards.Count)
            {
                if (playerHadForeplay[0] && playerHadForeplay[1])
                {
                    finalDeck.Add(sexCards[sexCardIndex]);
                    sexCardIndex++;
                    nextSexAt += random.Next(10, 14);
                    foreplayUsed = 0;
                    playerHadForeplay[0] = false;
                    playerHadForeplay[1] = false;
                    cardCount++;
                    currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                    continue;
                }
                else
                {
                    nextSexAt++;
                }
            }

            int cardsUntilNextSex = nextSexAt - cardCount;
            bool bothPlayersHadForeplay = playerHadForeplay[0] && playerHadForeplay[1];

            bool shouldPlaceForeplay = false;

            if (!bothPlayersHadForeplay && cardsUntilNextSex <= 2 && foreplayCards.Count > 0)
            {
                shouldPlaceForeplay = true;
            }
            else if (!bothPlayersHadForeplay && foreplayCards.Count > 0 &&
                    (finalDeck.Count == 0 || finalDeck[finalDeck.Count - 1].Type != CardType.Foreplay) &&
                    random.NextDouble() < 0.3)
            {
                shouldPlaceForeplay = true;
            }

            if (shouldPlaceForeplay && foreplayCards.Count > 0)
            {
                finalDeck.Add(foreplayCards[0]);
                foreplayCards.RemoveAt(0);
                foreplayUsed++;

                playerHadForeplay[currentPlayerIndex] = true;

                cardCount++;
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
            else if (otherCards.Count > 0)
            {
                finalDeck.Add(otherCards[0]);
                otherCards.RemoveAt(0);
                cardCount++;
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
            else if (foreplayCards.Count > 0)
            {
                finalDeck.Add(foreplayCards[0]);
                foreplayCards.RemoveAt(0);
                foreplayUsed++;

                playerHadForeplay[currentPlayerIndex] = true;

                cardCount++;
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
            else
            {
                break;
            }
        }

        while (sexCardIndex < sexCards.Count)
        {
            finalDeck.Add(sexCards[sexCardIndex]);
            sexCardIndex++;
        }

        finalDeck.AddRange(foreplayCards);
        finalDeck.AddRange(otherCards);

        mainDeck = finalDeck;
    }

    private void ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = random.Next(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Card DrawCard()
    {
        if (mainDeck.Count == 0)
        {
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

        return drawnCard;
    }

    public Card DrawLuckyCard()
    {
        if (luckyCards.Count == 0)
        {
            return null;
        }

        int randomIndex = random.Next(luckyCards.Count);
        Card luckyCard = luckyCards[randomIndex];

        return luckyCard;
    }

    public bool ShouldShowLuckyCard()
    {
        return random.NextDouble() < luckyCardChance;
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

    public Dictionary<CardType, Texture2D> GetCardTypeTextures()
    {
        return cardTypeTextures;
    }

    public Dictionary<int, Texture2D> GetShotCountTextures()
    {
        return shotCountTextures;
    }

    public Dictionary<CardType, Color> GetCardTypeColors()
    {
        return cardTypeColors;
    }

    public Color GetCardTypeLabelColor(CardType cardType)
    {
        return cardType == CardType.Sex ? new Color(1, 0, 0) : new Color(1, 1, 1);
    }

    public void ReinitializeDecksWithOptions()
    {
        mainDeck.Clear();
        individualDecks.Clear();
        luckyCards.Clear();
        sexCards.Clear();

        cardsSinceLastSex = 0;
        nextSexCardAt = 0;
        deckEmpty = false;

        InitializeDecks();
        ShuffleDecksTogether();

        GD.Print("Decks reinitialized with current options");
    }
}
