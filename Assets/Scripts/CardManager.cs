using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //int index = (int)suit * 13 + (int)rank; - for getting an index for a card sprite out of 52;

    public int NumberOfDecks = 1; //how many decks the player(s?) have chosen to use for their game

    public int NumberOfPlayers = 2;

    public List<Card> Deck;

    public List<List<Card>> PlayerCards;

    public List<List<Card[]>> PlayerStacks;

    public GameObject CardObject;
    public GameObject CardSlotObject;

    public GameObject PlayerHand;

    public Canvas Canvas;

    void Start()
    {
        Deck = new();
        PlayerCards = new();
        PlayerStacks = new();
        SpawnCards();
    }

    private void SpawnCards()
    {
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

            foreach (Card card in PlayerCards[j])
            {
                Debug.Log($"player {j + 1}'s {card.Suit}, {card.Rank}");
            }
        }

        foreach (Card card in PlayerCards[0])
        {
            GameObject cardObject = Instantiate(CardObject);

            GameObject cardSlot = Instantiate(CardSlotObject);

            cardObject.GetComponent<CardObject>().AddData(card, cardSlot.transform);
            cardObject.transform.SetParent(Canvas.transform);
            cardSlot.transform.SetParent(PlayerHand.transform);
        }
    }

    public void PullRandom(List<Card> hand)
    {
        //get random index to access random card in deck with
        int index = UnityEngine.Random.Range(0, Deck.Count);

        //store card in variable and remove from deck
        Card card = Deck[index];
        Deck.Remove(card);

        //add card to player hand
        hand.Add(card);
    }
}
