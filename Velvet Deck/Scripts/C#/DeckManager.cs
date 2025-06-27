using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DeckManager : Node
{
    private Card currentCard = null;

    [Export] public Label TypeLabel { get; set; }
    [Export] public Label HeaderLabel { get; set; }
    [Export] public Label DescriptionLabel { get; set; }
    [Export] public Label LuckyHeaderLabel { get; set; }
    [Export] public Label LuckyDescriptionLabel { get; set; }
    [Export] public TextureRect CardTypeImage { get; set; }
    [Export] public TextureRect ShotCountImage { get; set; }

    [Export] public Panel FrontCardPanel { get; set; }
    [Export] public Panel BackCardPanel { get; set; }
    [Export] public Panel LuckyCardPanel { get; set; }

    [Export] public Button FrontCardButton { get; set; }
    [Export] public Button BackCardButton { get; set; }
    [Export] public Button LuckyCardButton { get; set; }

    [Export] public TimerController TimerController { get; set; }

    private bool isTimerActive = false;
    private bool isCountdownActive = false;
    private bool gameStarted = false;

    public override void _Ready()
    {
        ConnectButtons();
        HideAllCards();
    }

    public void OnGameStarted()
    {
        gameStarted = true;
        ShowNextFrontCard();
    }

    private void HideAllCards()
    {
        if (FrontCardPanel != null) FrontCardPanel.Visible = false;
        if (BackCardPanel != null) BackCardPanel.Visible = false;
        if (LuckyCardPanel != null) LuckyCardPanel.Visible = false;
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

    public void DisplayFirstCard(Card card)
    {
        if (card == null) return;

        if (Components.Instance?.CardManager == null) { return; }

        var cardManager = Components.Instance.CardManager;
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

        if (HeaderLabel != null) HeaderLabel.Text = "";
        if (DescriptionLabel != null) DescriptionLabel.Text = "";
        if (ShotCountImage != null) ShotCountImage.Texture = null;

        SetCardColor(FrontCardPanel, card.Type);

    }

    public void DisplaySecondCard(Card card)
    {
        if (card == null) return;

        if (Components.Instance?.CardManager == null)
        {
            return;
        }

        var cardManager = Components.Instance.CardManager;
        var shotCountTextures = cardManager.GetShotCountTextures();

        if (HeaderLabel != null) HeaderLabel.Text = card.Header;
        if (DescriptionLabel != null) DescriptionLabel.Text = card.Description;
        if (ShotCountImage != null && shotCountTextures.ContainsKey(card.ShotCount))
        {
            ShotCountImage.Texture = shotCountTextures[card.ShotCount];
        }

        if (TypeLabel != null) TypeLabel.Text = "";
        if (CardTypeImage != null) CardTypeImage.Texture = null;

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

        if (LuckyHeaderLabel != null) LuckyHeaderLabel.Text = card.Header;
        if (LuckyDescriptionLabel != null) LuckyDescriptionLabel.Text = card.Description;

        SetCardColor(LuckyCardPanel, CardType.Lucky);
    }

    public void DisplayCard(Card card)
    {
        if (card == null) return;

        var cardManager = Components.Instance.CardManager;
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
        Card drawnCard = Components.Instance.CardManager.DrawCard();
        DisplayFirstCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplaySecondCard()
    {
        Card drawnCard = Components.Instance.CardManager.DrawCard();
        DisplaySecondCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplayCard()
    {
        Card drawnCard = Components.Instance.CardManager.DrawCard();
        DisplayCard(drawnCard);
        return drawnCard;
    }

    public Card DrawAndDisplayLuckyCard()
    {
        Card luckyCard = Components.Instance.CardManager.DrawLuckyCard();
        if (luckyCard != null)
        {
            DisplayLuckyCard(luckyCard);
        }
        return luckyCard;
    }

    public void OnFrontCardPressed()
    {
        if (currentCard != null)
        {
            DisplaySecondCard(currentCard);

            Components.Instance.Animations.FlipFrontCard(FrontCardPanel, BackCardPanel);
        }
    }

    public void ProgressToNextCard()
    {
        if (TimerController != null)
        {
            TimerController.HideAllTimers();
            isTimerActive = false;
        }

        if (Components.Instance?.TurnManager != null)
        {
            Components.Instance.TurnManager.NextTurn();
        }

        if (Components.Instance?.CardManager?.ShouldShowLuckyCard() == true)
        {
            ShowLuckyCard();
        }
        else
        {
            ShowNextFrontCard();
        }
    }

    public void OnBackCardPressed()
    {
        if (TimerController != null && TimerController.IsAnyTimerRunning())
        {
            return;
        }

        if (TimerController != null)
        {
            TimerController.HideAllTimers();
            isTimerActive = false;
        }

        DisplayFirstCard(currentCard);
        Components.Instance.Animations.FlipBackCard(BackCardPanel, FrontCardPanel);
    }

    public void OnLuckyCardPressed()
    {
        HideLuckyCard();
        ShowNextFrontCard();
    }

    private void ShowNextFrontCard()
    {
        if (!gameStarted)
        {
            return; 
        }

        if (Components.Instance == null || Components.Instance.CardManager == null)
        {
            CallDeferred(nameof(ShowNextFrontCard));
            return;
        }

        currentCard = Components.Instance.CardManager.DrawCard();

        if (currentCard == null)
        {
            Components.Instance.Animations.AnimateDeckEmpty();
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
        if (LuckyCardPanel != null) LuckyCardPanel.Visible = false;

        if (FrontCardButton != null)
        {
            FrontCardButton.Disabled = false;
        }
        if (BackCardButton != null)
        {
            BackCardButton.Disabled = true;
        }
    }

    private void ShowBackCard()
    {
        if (currentCard != null)
        {
            DisplaySecondCard(currentCard);

            if (FrontCardPanel != null) FrontCardPanel.Visible = false;
            if (BackCardPanel != null) BackCardPanel.Visible = true;
            if (LuckyCardPanel != null) LuckyCardPanel.Visible = false;
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

        Card luckyCard = Components.Instance.CardManager.DrawLuckyCard();
        if (luckyCard != null)
        {
            DisplayLuckyCard(luckyCard);

            if (FrontCardPanel != null) FrontCardPanel.Visible = false;
            if (BackCardPanel != null) BackCardPanel.Visible = false;
            if (LuckyCardPanel != null) LuckyCardPanel.Visible = true;
        }
    }

    private void HideLuckyCard()
    {
        if (LuckyCardPanel != null) LuckyCardPanel.Visible = false;
    }

    private void SetCardColor(Panel panel, CardType cardType)
    {
        var cardManager = Components.Instance.CardManager;
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

        styleBox.BgColor = cardTypeColors[cardType];
    }
}
