using System.Collections.Generic;
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
    public List<CardObject> PlayerHand;

    //int index = (int)suit * 13 + (int)rank; - for getting an index for a card sprite out of 52;

    void Start()
    {
        PlayerHand = new();
    }

    public void AddCards(List<CardObject> cards)
    {
        foreach (CardObject card in cards)
        {
            PlayerHand.Add(card);
        }

    }


    public void UpdateHand()
    {
        foreach (CardObject card in PlayerHand)
        {

        }
    }
}
