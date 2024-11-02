using System.Collections.Generic;
using UnityEngine;

public class CPUManager : MonoBehaviour
{
    void Start()
    {
    }

    public void ProcessTurn(List<Card> cards)
    {
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
            int topMatch = 0;
            int topMatchIndex = - 1;

            foreach(var list in matches)
            {
                if (list.Count > topMatch) 
                {
                    topMatch = list.Count;
                    topMatchIndex = matches.IndexOf(list);
                }
            }
        }
        
       
    }

}
