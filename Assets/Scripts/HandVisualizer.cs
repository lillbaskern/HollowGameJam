using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HandVisualizer : MonoBehaviour
{
    public static HandVisualizer Instance {  get; private set; }

    public List<Transform> PlayerHand {  get; private set; }

    public List<CardObject> SelectedCards { get; set; }

    public Button ContextButton;

    public Transform CardContainer;
    

    void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        PlayerHand = new();
        SelectedCards = new List<CardObject>();

        UpdateHand();
    }


    private void FixedUpdate()
    {
        if (SelectedCards.Count <= 0 && ContextButton.gameObject.activeSelf)
        {
            ContextButton.gameObject.SetActive(false);
            return;
        }
        if (SelectedCards.Count <= 0)
            return;
        else
            ContextButton.gameObject.SetActive(true);

        if(SelectedCards.Count >= 1 && SelectedCards.Count <4)
        {
            CardObject latestCard = null;
            bool matchingCards = true;
            foreach (var card in SelectedCards)
            {
                if (latestCard != null)
                    matchingCards = latestCard.CardData.Rank == card.CardData.Rank;

                latestCard = card;
            }
            if (matchingCards)
                ContextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ask for cards";
            else
                ContextButton.gameObject.SetActive(false);
            return;
        }


        bool cardsMatch = true;
        CardObject lastCard = null;
        int matchCount = 0;

        foreach (var card in SelectedCards) 
        {
            if(lastCard  != null)
                cardsMatch = lastCard.CardData.Rank == card.CardData.Rank;

            if(cardsMatch)
                matchCount++;
            lastCard = card;
        }
        Debug.Log(matchCount);
        if (cardsMatch && matchCount >= 4)
        {
            ContextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stack cards";
            ContextButton.gameObject.SetActive(true);
        }
        else
        {
            ContextButton.gameObject.SetActive(false);
        }

    }


    public void UpdateHand()
    {
        SelectedCards.Clear();
        PlayerHand.Clear();
        for (int i = 0; i < PlayerHand.Count; i++)
        {
            PlayerHand.Add(CardContainer.GetChild(i));
        }

    }

    public void UpdateHand(List<Card> cards)
    {
        foreach (var card in SelectedCards) 
        {
            card.selected = false;
        }
        SelectedCards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cardInHand = CardContainer.GetChild(i).GetComponent<CardObject>();
            if (!cards.Contains(cardInHand.CardData))
            {
                Destroy(cardInHand.gameObject);
            }
        }

        for (int i = 0; i < cards.Count; i++) 
        {
            bool cardIsInHand = false;
            for (int j = 0; j < transform.childCount; j++) 
            {
                var cardInHand = CardContainer.GetChild(j).GetComponent<CardObject>();
                if(cardInHand.CardData == cards[i])
                    cardIsInHand = true;
            }
            if (!cardIsInHand)
            {
                var cardObject = CardManager.Instance.MakeNewCardObject(cards[i]); 
            }
        }
    }
}
