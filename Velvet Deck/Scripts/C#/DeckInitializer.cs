using System.Collections.Generic;

public static class DeckInitializer
{
    public static List<Card> InitializeDrinkDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Drink, "Take a sip of your drink. Enjoy a refreshing drink together", "Sip & Savor", 1, 30f));
        deck.Add(new Card(CardType.Drink, "Share a drink with your partner. Take turns drinking from the same glass", "Share the Love", 1, 30f));
        deck.Add(new Card(CardType.Drink, "Finish your drink. Drink up and move on to the next round", "Bottoms Up", 2, 30f));
        deck.Add(new Card(CardType.Drink, "Take a shot together. Cheers and take a shot simultaneously", "Synchronized Shot", 3, 30f));
        deck.Add(new Card(CardType.Drink, "Make a toast to your relationship. Raise your glasses and celebrate", "Cheers to Us", 1, 30f));
        deck.Add(new Card(CardType.Drink, "Drink without using your hands. Get creative with how you drink", "Hands-Free Fun", 2, 30f));
        deck.Add(new Card(CardType.Drink, "Feed your partner a drink. Help your partner drink", "Feeding Time", 1, 30f));

        return deck;
    }

    public static List<Card> InitializeForeplayDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Foreplay, "Give your partner a sensual massage. Use your hands to relax and excite", "Massage Magic", 2));
        deck.Add(new Card(CardType.Foreplay, "Kiss your partner passionately for 30 seconds. A deep, romantic kiss", "Passionate Kiss", 1));
        deck.Add(new Card(CardType.Foreplay, "Whisper something seductive in your partner's ear. Share your desires quietly", "Sweet Whispers", 1));
        deck.Add(new Card(CardType.Foreplay, "Slowly undress each other. Take your time removing clothes", "Slow Reveal", 3));
        deck.Add(new Card(CardType.Foreplay, "Give your partner a neck massage. Focus on the neck and shoulders", "Neck Caress", 1));
        deck.Add(new Card(CardType.Foreplay, "Kiss your partner's favorite spot. Find that special place", "Secret Spot", 2));

        return deck;
    }

    public static List<Card> InitializeFunDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Fun, "Tell a funny story about your childhood. Share a memorable and amusing moment", "Memory Lane", 1));
        deck.Add(new Card(CardType.Fun, "Dance together to your favorite song. Move to the rhythm together", "Dance Party", 2));
        deck.Add(new Card(CardType.Fun, "Play a quick game of rock, paper, scissors. Best of three wins", "Quick Game", 1));
        deck.Add(new Card(CardType.Fun, "Do your best impression of a celebrity. Act like someone famous", "Star Performance", 1));
        deck.Add(new Card(CardType.Fun, "Sing a song together. Harmonize or take turns", "Duet Time", 2));
        deck.Add(new Card(CardType.Fun, "Tell each other a joke. Make your partner laugh", "Comedy Hour", 1));
        deck.Add(new Card(CardType.Fun, "Act out your favorite movie scene. Recreate a memorable moment", "Movie Magic", 3));

        return deck;
    }

    public static List<Card> InitializeSexDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Sex, "Try a new position. Experiment with something different", "New Heights", 3));
        deck.Add(new Card(CardType.Sex, "Take turns being in control. Switch who's leading", "Power Play", 2));
        deck.Add(new Card(CardType.Sex, "Focus on pleasing your partner. Make it all about them", "Pure Pleasure", 2));
        deck.Add(new Card(CardType.Sex, "Use only your hands. Get creative with touch", "Hands Only", 1));
        deck.Add(new Card(CardType.Sex, "Switch positions every minute. Keep changing things up", "Rapid Fire", 3));

        return deck;
    }

    public static List<Card> InitializeRoleplayDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Roleplay, "Act like strangers meeting for the first time. Pretend you've never met before", "First Encounter", 2));
        deck.Add(new Card(CardType.Roleplay, "One of you is a teacher, the other a student. Classic classroom scenario", "School Days", 2));
        deck.Add(new Card(CardType.Roleplay, "Doctor and patient roleplay. Medical examination time", "Check-up Time", 3));
        deck.Add(new Card(CardType.Roleplay, "Boss and employee scenario. Workplace power dynamics", "Office Hours", 2));
        deck.Add(new Card(CardType.Roleplay, "Pretend you're on your first date. Recreate that nervous excitement", "First Date", 1));
        deck.Add(new Card(CardType.Roleplay, "Celebrity and fan encounter. One is famous, one is starstruck", "Meet & Greet", 1));

        return deck;
    }

    public static List<Card> InitializeLoveDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Love, "Tell your partner three things you love about them. Share your deepest feelings", "Love List", 1));
        deck.Add(new Card(CardType.Love, "Share your favorite memory together. Reminisce about good times", "Memory Box", 1));
        deck.Add(new Card(CardType.Love, "Write a love note to each other. Put your feelings on paper", "Love Letters", 2));
        deck.Add(new Card(CardType.Love, "Talk about your future dreams together. Plan your life ahead", "Dream Big", 2));
        deck.Add(new Card(CardType.Love, "Give your partner a heartfelt compliment. Say something meaningful", "Sweet Words", 1));
        deck.Add(new Card(CardType.Love, "Share what made you fall in love. Remember the beginning", "First Love", 1));
        deck.Add(new Card(CardType.Love, "Plan your next romantic date. Design the perfect evening", "Date Night", 2));

        return deck;
    }

    public static List<Card> InitializeLuckyDeck()
    {
        var deck = new List<Card>();

        deck.Add(new Card(CardType.Lucky, "Skip the next turn. Take a break from the action", "Free Pass", 1));
        deck.Add(new Card(CardType.Lucky, "Draw an extra card. Get another chance", "Bonus Round", 1));
        deck.Add(new Card(CardType.Lucky, "Choose any card type for your next draw. Pick your preference", "Your Choice", 1));
        deck.Add(new Card(CardType.Lucky, "Swap roles with your partner. Switch things up", "Role Reversal", 1));
        deck.Add(new Card(CardType.Lucky, "Double the effect of your next card. Make it count twice", "Double Down", 2));
        deck.Add(new Card(CardType.Lucky, "Get a free massage from your partner. Relax and enjoy", "Lucky Touch", 1));

        return deck;
    }

    public static Dictionary<CardType, List<Card>> InitializeAllDecks()
    {
        return new Dictionary<CardType, List<Card>>
        {
            { CardType.Drink, InitializeDrinkDeck() },
            { CardType.Foreplay, InitializeForeplayDeck() },
            { CardType.Fun, InitializeFunDeck() },
            { CardType.Sex, InitializeSexDeck() },
            { CardType.Roleplay, InitializeRoleplayDeck() },
            { CardType.Love, InitializeLoveDeck() },
            { CardType.Lucky, InitializeLuckyDeck() }
        };
    }
}
