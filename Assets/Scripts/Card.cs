using UnityEngine;

public class Card
{
    public Card(CardSuits suit, CardRank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public CardRank Rank { get; }
    public CardSuits Suit { get; }
}
