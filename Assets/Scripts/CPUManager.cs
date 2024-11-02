using System.Collections.Generic;
using UnityEngine;

public class CPUManager
{
    public int PlayerIndex;//which player the cpu is

    public CardRank LastAskedRank;
    void Start()
    {
    }



    public void ProcessTurn(List<Card> cards)
    {
        Debug.Log("processing turn for cpu" + PlayerIndex);
        List<List<Card>> matches = new();
        for (int i = 0; i < cards.Count; i++) 
        {
            foreach (var list in matches)
            {
                if (list.Contains(cards[i]))
                    continue;
            }
            List<Card> cardMatches = new();
            for (int j = 0; j < cards.Count; j++) 
            {
                if (cards[j] == cards[i]) continue;
                if (cards[i].Rank == cards[j].Rank)
                {
                    cardMatches.Add(cards[j]);
                }
            }
        }
        if (matches.Count > 0)
        {
            int topMatch = -1;
            int topMatchIndex = -1;

            foreach (var list in matches)
            {
                if (list.Count > topMatch)
                {
                    topMatch = list.Count;
                    topMatchIndex = matches.IndexOf(list);
                }
            }
            int randomPlayer = Random.Range(0, CardManager.Instance.NumberOfPlayers);
            if (randomPlayer == PlayerIndex)
            {
                randomPlayer -= 1; //because human player is always 0
            }
            if (topMatch != -1 && matches[topMatch][0].Rank != LastAskedRank)
            {

                CardManager.Instance.AskForCard(PlayerIndex, matches[topMatchIndex][0], randomPlayer);
            }
            else
                CardManager.Instance.AskForCard(PlayerIndex, cards[Random.Range(0, cards.Count)], randomPlayer);
        }
        int rndPlayer = Random.Range(0, CardManager.Instance.NumberOfPlayers);
        if (rndPlayer == PlayerIndex)
        {
            rndPlayer -= 1; //because human player is always 0
        }
        CardManager.Instance.AskForCard(PlayerIndex, cards[Random.Range(0, cards.Count)], rndPlayer);


    }

}
