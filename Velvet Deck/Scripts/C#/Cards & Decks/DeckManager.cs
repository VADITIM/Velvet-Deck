using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DeckManager : Node
{
    CardManager CardManager => Components.Instance?.CardManager;
    CardAnimations CardAnimations => Components.Instance?.CardAnimations;
    TurnManager TurnManager => Components.Instance?.TurnManager;
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    JokerManager JokerManager => Components.Instance?.JokerManager;

    public Card currentCard = null;

    [Export] public Label TypeLabel { get; set; }
    [Export] public Label HeaderLabel { get; set; }
    [Export] public Label DescriptionLabel { get; set; }
    [Export] public Label LuckyHeaderLabel { get; set; }
    [Export] public TextureRect LuckyCardImage { get; set; }
    [Export] public Label LuckyCardType { get; set; }
    [Export] public Label LuckyDescriptionLabel { get; set; }
    [Export] public TextureRect CardTypeImage { get; set; }
    [Export] public TextureRect ShotCountImage { get; set; }

    [Export] public Control CardsContainer { get; set; }
    [Export] public Panel FrontCardPanel { get; set; }
    [Export] public Panel BackCardPanel { get; set; }
    [Export] public Panel LuckyCardPanel { get; set; }

    [Export] public TimerController TimerController { get; set; }

    private bool isTimerActive = false;
    private bool isCountdownActive = false;
    public bool gameStarted = false;

    public override void _Ready()
    {
        ConnectButtons();
        HideAllCards();
    }

    private void HideAllCards()
    {
        if (FrontCardPanel != null) FrontCardPanel.Visible = false;
        if (BackCardPanel != null) BackCardPanel.Visible = false;
        if (LuckyCardPanel != null) LuckyCardPanel.Visible = false;
    }

    private void ConnectButtons()
    {
    }

    public void DisplayFirstCard(Card card)
    {
        if (card == null) return;

        if (Components.Instance?.CardManager == null) { return; }

        var cardManager = CardManager;
        var cardTypeTextures = cardManager.GetCardTypeTextures();

        if (TypeLabel != null)
        {
            TypeLabel.Text = card.Type.ToString();
            TypeLabel.Modulate = cardManager.GetCardTypeLabelColor(card.Type);
        }
        if (CardTypeImage != null && cardTypeTextures.ContainsKey(card.Type))
        {
            CardTypeImage.Texture = cardTypeTextures[card.Type];
        }

        if (BackCardPanel != null && !BackCardPanel.Visible)
        {
            if (HeaderLabel != null) HeaderLabel.Text = "";
            if (DescriptionLabel != null) DescriptionLabel.Text = "";
            if (ShotCountImage != null) ShotCountImage.Texture = null;
        }

        SetCardColor(FrontCardPanel, card.Type);

    }

    public void DisplaySecondCard(Card card)
    {
        if (card == null) return;

        if (Components.Instance?.CardManager == null)
        {
            return;
        }

        var cardManager = CardManager;
        var shotCountTextures = cardManager.GetShotCountTextures();

        if (HeaderLabel != null) HeaderLabel.Text = card.Header;
        if (DescriptionLabel != null) DescriptionLabel.Text = card.Description;
        if (ShotCountImage != null && shotCountTextures.ContainsKey(card.ShotCount))
        {
            ShotCountImage.Texture = shotCountTextures[card.ShotCount];
        }

        SetCardColor(BackCardPanel, card.Type);

        if (card.Timer > 0f && TimerController != null)
        {
            TimerController.StartCountdown(card.Timer);
            isTimerActive = true;
        }
    }

    public void DisplayLuckyCard(Card card)
    {
        if (card == null) return;

        CardAnimations.ShowLuckyCard();

        LuckyHeaderLabel.Text = card.Header;
        LuckyDescriptionLabel.Text = card.Description;
        LuckyCardType.Visible = false;

        SetCardColor(LuckyCardPanel, CardType.Lucky);
    }

    public void DisplayCard(Card card)
    {
        if (card == null) return;

        var cardManager = CardManager;
        var cardTypeTextures = cardManager.GetCardTypeTextures();
        var shotCountTextures = cardManager.GetShotCountTextures();

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
    }

    public Card DrawAndDisplayFirstCard()
    {
        Card drawnCard = CardManager.DrawCard();
        DisplayFirstCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplaySecondCard()
    {
        Card drawnCard = CardManager.DrawCard();
        DisplaySecondCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplayCard()
    {
        Card drawnCard = CardManager.DrawCard();
        DisplayCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplayLuckyCard()
    {
        Card luckyCard = CardManager.DrawLuckyCard();
        if (luckyCard != null)
        {
            DisplayLuckyCard(luckyCard);
        }
        return luckyCard;
    }

    public void ProgressToNextCard()
    {
        if (TimerController != null)
        {
            TimerController.HideAllTimers();
            isTimerActive = false;
        }

        if (CardManager.GetRemainingCards() == 0)
        {
            HandleGameComplete();
            return;
        }

        if (Components.Instance?.CardManager?.ShouldShowLuckyCard() == true)
            ShowLuckyCard();
        ShowNextFrontCard();
    }

    private void HandleGameComplete()
    {
        CardAnimations.AnimateDeckEmpty();
        gameStarted = false;
    }


    public void ShowNextFrontCard()
    {
        if (!gameStarted) return;

        if (Components.Instance == null || CardManager == null)
        {
            CallDeferred(nameof(ShowNextFrontCard));
            return;
        }

        currentCard = CardManager.DrawCard();

        if (currentCard == null)
        {
            CardAnimations.AnimateDeckEmpty();
            return;
        }

        DisplayFirstCard(currentCard);

        if (FrontCardPanel != null)
        {
            FrontCardPanel.Visible = true;
            FrontCardPanel.Scale = new Vector2(1.0f, FrontCardPanel.Scale.Y);
        }
        if (BackCardPanel != null)
        {
            BackCardPanel.Visible = false;
            BackCardPanel.Scale = new Vector2(1.0f, BackCardPanel.Scale.Y);
        }

        ButtonHandler.FrontCardButton.Disabled = false;
        ButtonHandler.BackCardButton.Disabled = true;
    }

    private void ShowBackCard()
    {
        if (currentCard != null)
        {
            DisplaySecondCard(currentCard);

            if (FrontCardPanel != null) FrontCardPanel.Visible = false;
            if (BackCardPanel != null) BackCardPanel.Visible = true;
        }
    }

    private void HideBackCard()
    {
        if (BackCardPanel != null) BackCardPanel.Visible = false;
    }

    private void ShowLuckyCard()
    {
        if (Components.Instance?.CardManager == null)
        {
            return;
        }

        Card luckyCard = CardManager.DrawLuckyCard();
        if (luckyCard != null)
        {
            LuckyCardPanel.Visible = true;
            DisplayLuckyCard(luckyCard);
            HideBackCard();
        }
    }

    public void ClearFrontCardElements()
    {
        if (TypeLabel != null) TypeLabel.Text = "";
        if (CardTypeImage != null) CardTypeImage.Texture = null;
    }

    private void SetCardColor(Panel panel, CardType cardType)
    {
        var cardManager = CardManager;
        var cardTypeColors = cardManager.GetCardTypeColors();

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

        var targetColor = cardTypeColors[cardType];
        styleBox.BgColor = targetColor;
        panel.AddThemeStyleboxOverride("panel", styleBox);
    }
}
