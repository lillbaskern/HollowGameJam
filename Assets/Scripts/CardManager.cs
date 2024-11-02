using System;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(1)]
public class CardManager : MonoBehaviour
{
    //int index = (int)suit * 13 + (int)rank; - for getting an index for a card sprite out of 52;

    public static CardManager Instance;

    public int NumberOfDecks = 1; //how many decks the player(s?) have chosen to use for their game

    public int NumberOfPlayers = 2;

    public int CurrentPlayersTurn;

    public List<Card> Deck;

    public List<List<Card>> PlayerCards;

    public List<List<CardObject[]>> PlayerStacks;

    public GameObject CardObject;
    public GameObject CardSlotObject;

    public GameObject CardContainer;
    public GameObject PlayerHand;
    public Canvas Canvas;

    void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Deck = new();
        PlayerCards = new();
        PlayerStacks = new();
        for(int i = 0; i < NumberOfPlayers; i++)
        {
            PlayerStacks.Add(new());
        }
        SpawnCards();


        CurrentPlayersTurn = UnityEngine.Random.Range(0, NumberOfPlayers - 1);
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
            PlayerStacks.Add(new List<CardObject[]>());
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
            cardObject.transform.SetParent(CardContainer.transform);
            cardSlot.transform.SetParent(PlayerHand.transform);
        }


    }


    public void OnContextButtonClicked()
    {
        var selectedCards = HandVisualizer.Instance.SelectedCards;
        if (selectedCards.Count <= 0) return;


        if(selectedCards.Count < 4)
        {
            //prompt player for which player to ask later
            AskForCard(CurrentPlayersTurn, selectedCards[0].CardData, 1);
            return;
        }



        if(selectedCards.Count == 4)
        {
            bool cardsMatch = true;
            CardObject lastCard = null;
            foreach (CardObject card in selectedCards) 
            {
                if(lastCard == null)
                {
                    lastCard = card;
                    continue;
                }
                cardsMatch = lastCard.CardData.Rank == card.CardData.Rank;
                lastCard = card;
            }

            if (cardsMatch) 
            {
                //add stack to player stack
                PlayerStacks[CurrentPlayersTurn].Add(selectedCards.ToArray());

                for (int i = 0; i < selectedCards.Count; i++) 
                {
                    PlayerCards[0].Remove(selectedCards[i].CardData);
                    Destroy(selectedCards[i].gameObject);
                }

                Debug.Log($"added stack of {lastCard.CardData.Rank}s for player {CurrentPlayersTurn}");
                UpdatePlayerVisual();
                return;
            }
        }
    }

    public void AskForCard(int askingPlayer, Card card, int askedPlayer)
    {
        if (card == null) return;

        List<Card> foundCards = new List<Card>();

        for (int i = 0; i < PlayerCards[askedPlayer].Count; i++) 
        {
            if (PlayerCards[askedPlayer][i].Rank == card.Rank)
            {
                //maybe bluffing?
                foundCards.Add(PlayerCards[askedPlayer][i]);
            }
        }
        if (foundCards.Count > 0)
        {
            foreach (Card c in foundCards)
            {
                AddCard(c, askingPlayer);
            }

            RemovePlayerCards(foundCards.ToArray(), askedPlayer);
            Debug.Log("du får kort");
            return;
        }

        Debug.Log("finns i sjön");

        AddCard(PullRandom(), askingPlayer);

        NextTurn();
    }

    public void EndGame()
    {
        Debug.Log("Game over");
    }

    public void AddCard(Card card, int player) 
    {
        Debug.Log(player);
        PlayerCards[player].Add(card);
        if(player == 0)
        {
            HandVisualizer.Instance.UpdateHand(PlayerCards[0]);
        }

    }

    public void NextTurn()
    {
        if(Deck.Count < 1)
        {
            EndGame();
            return;
        }

        //CurrentPlayersTurn++;
        UpdatePlayerVisual();
    }

    public void UpdatePlayerVisual()
    {
        HandVisualizer.Instance.UpdateHand(PlayerCards[0]);
    }

    public void RemovePlayerCards(Card[] cards, int player)
    {
        for (int i = 0; i < cards.Length; i++) 
        {
            PlayerCards[player].Remove(cards[i]);
        }
    }

    public void SwapPlayerCards(CardObject[] cards, int fromPlayer, int toPlayer)
    {

    }

    public CardObject MakeNewCardObject(Card card)
    {

        GameObject cardObject = Instantiate(CardObject);

        GameObject cardSlot = Instantiate(CardSlotObject);

        cardObject.GetComponent<CardObject>().AddData(card, cardSlot.transform);
        cardObject.transform.SetParent(CardContainer.transform);
        cardSlot.transform.SetParent(PlayerHand.transform);

        return cardObject.GetComponent<CardObject>();
    }

    public void UpdatePlayerCards()
    {

    }


    public Card PullRandom()
    {
        //get random index to access random card in deck with
        int index = UnityEngine.Random.Range(0, Deck.Count);

        //store card in variable and remove from deck
        Card card = Deck[index];
        Deck.Remove(card);

        //add card to player hand
        return card;
    }
}
