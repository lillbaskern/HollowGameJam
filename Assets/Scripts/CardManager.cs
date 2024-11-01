using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int NumberOfDecks = 1; //how many decks the player(s?) have chosen to use for their game

    public int NumberOfPlayers = 2;

    public List<Card> Deck;

    public List<List<Card>> PlayerCards;

    public List<List<Card[]>> PlayerStacks;

    public CardObject CardObject;

    void Start()
    {
        Deck = new();
        PlayerCards = new();
        PlayerStacks = new();

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

        int handSize = NumberOfPlayers > 2 ? 5 : 7;


        for (int j = 0; j < NumberOfPlayers; j++)
        {
            PlayerCards.Add(new List<Card>());
            PlayerStacks.Add(new List<Card[]>());
            for (int i = 0; i < handSize; i++)
            {
                int index = UnityEngine.Random.Range(0, Deck.Count);

                Card card = Deck[index];
                Deck.Remove(card);

                //add card to player hand
                PlayerCards[j].Add(card);

            }
            
            foreach(Card card in PlayerCards[j])
            {
                Debug.Log($"player {j + 1}'s {card.Suit}, {card.Rank}");
            }
        
        }
    }
}
