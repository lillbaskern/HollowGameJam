using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CPUManager
{
    public int PlayerIndex;//which player the cpu is

    public CardRank LastAskedRank;
    public static List<int> AskedPlayersBuffer;
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



        Debug.Log($"matches for cpu player {PlayerIndex}, {matches.Count}");

        foreach (var list in matches) 
        {
            Debug.Log($"{list[0]}, {list.Count}");
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

            bool playeraskedTooManyTimesInARow = true;
            for(int i = 0; i < AskedPlayersBuffer.Count; i++)
            {
                playeraskedTooManyTimesInARow = AskedPlayersBuffer[i] == randomPlayer;
            }

            if (playeraskedTooManyTimesInARow)
            {
                randomPlayer = Random.Range(0, CardManager.Instance.NumberOfPlayers);



                if (randomPlayer == PlayerIndex)
                {
                    randomPlayer -= 1; //because human player is always 0
                }
            }

            if (topMatch != -1 && matches[topMatch][0].Rank != LastAskedRank)
            {
                Debug.Log("matches found and this is first turn for cpu, asking for card from cpumanager");
                CardManager.Instance.AskForCard(PlayerIndex, matches[topMatchIndex][0], randomPlayer);
            }
            else
                CardManager.Instance.AskForCard(PlayerIndex, cards[Random.Range(0, cards.Count)], randomPlayer);

            AskedPlayersBuffer.Add(randomPlayer);
            
            if (AskedPlayersBuffer.Count > 3) 
                AskedPlayersBuffer.RemoveAt(0);

            Debug.Log("asked for card from cpu manager after a match was found");
            return;
        }
        
        
        
        int rndPlayer = Random.Range(0, CardManager.Instance.NumberOfPlayers);
        if (rndPlayer == PlayerIndex)
        {
            rndPlayer -= 1; //because human player is always 0
        }
        Debug.Log("asking for card from cpu manager, no matches");
        CardManager.Instance.AskForCard(PlayerIndex, cards[Random.Range(0, cards.Count)], rndPlayer);
    }

}
