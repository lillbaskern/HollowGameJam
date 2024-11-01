using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int NumberOfDecks = 1; //how many decks the player(s?) have chosen to use for their game

    public int NumberOfPlayers = 2;

    public List<Card> Deck;

    public List<List<Card>> PlayerCards;

    void Start()
    {
        Deck = new();

        for (int j = 0; j < NumberOfDecks; j++) 
        {
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 13; k++)
                {
                    Deck.Add(new Card((CardSuits)i, (CardRank)k));
                }
            }
        }

        foreach (Card card in Deck)
        {
            Debug.Log($"{card.Suit}, {card.Rank}");
        }
    }
}
