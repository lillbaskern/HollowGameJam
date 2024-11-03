using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel;
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
    public GameObject ContextButton;
    public GameObject PlayerPromptPanel;

    
    
    public Canvas Canvas;

    public List<CPUManager> ComputerPlayers { get; private set; }

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
        ComputerPlayers = new();
        for(int i = 0; i < NumberOfPlayers; i++)
        {
            PlayerStacks.Add(new());
            if (i == 0) continue;
            CPUManager cpu = new CPUManager();
            cpu.PlayerIndex = i;
            ComputerPlayers.Add(cpu);
            
        }
        

    }

    private void Start()
    {
        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForSeconds(0.08f);
            SpawnCards();

            CurrentPlayersTurn = UnityEngine.Random.Range(0, NumberOfPlayers - 1);
            NextTurn();
        }
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
            CardObject cardObject = Instantiate(CardObject).GetComponent<CardObject>();

            GameObject cardSlot = Instantiate(CardSlotObject);

            int index = ((int)card.Suit) * 13 + ((int)card.Rank);



            Debug.Log("looking for sprite at " + index);
            Debug.Log(cardObject);
            Debug.Log(cardSlot);
            cardObject.AddData(card, cardSlot.transform, index);
            cardObject.transform.SetParent(CardContainer.transform);
            cardSlot.transform.SetParent(PlayerHand.transform);
        }


    }


    public void OnContextButtonClicked()
    {
        if (CurrentPlayersTurn != 0)
        {
            Debug.Log($"human player pressed button but it is {CurrentPlayersTurn}s turn!");
            return;
        }

        var selectedCards = HandVisualizer.Instance.SelectedCards;
        if (selectedCards.Count <= 0) return;

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

        if(selectedCards.Count < 4)
        {
            if(NumberOfPlayers > 2)
            {
                CardToAskFor = selectedCards[0].CardData;
                OpenPlayerPromptPanel();
                return;
            }

            //prompt player for which player to ask later
            AskForCard(CurrentPlayersTurn, selectedCards[0].CardData, 1);
        }

    }

    private void OpenPlayerPromptPanel()
    {
        PlayerPromptPanel.SetActive(true);

        for (int i = 0; i < NumberOfPlayers - 1; i++) 
        {
            PlayerPromptPanel.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetCardToAskFor(CardObject card)
    {
        CardToAskFor = card.CardData;
    }
    public Card CardToAskFor;
    public void PlayerAskForCard(int player)
    {
        if (CardToAskFor == null) return;

        AskForCard(0, CardToAskFor, player);
    }


    string[] playerNames = new string[4] { "You", "Fisherman", "Widow", "Mrs." };


    public bool CanContinue = false;
    public void AskForCard(int askingPlayer, Card card, int askedPlayer)
    {
        Debug.Log($"askforcard called. askingplayer = {askingPlayer}, card = {card.Rank}, askedplayer = {askedPlayer}");
        StartCoroutine(AskForCardRoutine(askingPlayer, card, askedPlayer));
    }

    public IEnumerator AskForCardRoutine(int askingPlayer, Card card, int askedPlayer)
    {
        if (card == null) yield break;
        HandVisualizer.Instance.CanShowButton = false;

        List<Card> foundCards = new List<Card>();

        string prompt = $"{playerNames[askedPlayer]}...";
        Debug.Log(prompt);


        CanContinue = false;
        DialogueManager.Instance.ShowPrompt(prompt);
        AudioManager.Instance.PlaySound("ding");
        yield return new WaitUntil(() => CanContinue);

        
        yield return new WaitUntil(() => CanContinue);
        AudioManager.Instance.PlaySound("ding");
        prompt = $"Do you have any {card.Rank}s?";
        Debug.Log(prompt);

        CanContinue = false;
        DialogueManager.Instance.ShowPrompt(prompt);
        yield return new WaitUntil(() => CanContinue);

        Debug.Log($"{askingPlayer} asking {askedPlayer} for any {card.Rank}s");
        for (int i = 0; i < PlayerCards[askedPlayer].Count; i++)
        {
            if (PlayerCards[askedPlayer][i].Rank == card.Rank)
            {
                Debug.Log($"cards found in player {i}s hand matching rank {card.Rank}");
                //maybe bluffing?
                foundCards.Add(PlayerCards[askedPlayer][i]);
            }
        }




        if (foundCards.Count > 0)
        {
            prompt = "Yes, I do...";
            Debug.Log(prompt);

            CanContinue = false;
            AudioManager.Instance.PlaySound("yes");
            DialogueManager.Instance.ShowPrompt(prompt);
            yield return new WaitUntil(() => CanContinue);

            foreach (Card c in foundCards)
            {
                AddCard(c, askingPlayer);
            }

            RemovePlayerCards(foundCards.ToArray(), askedPlayer);
            if (CurrentPlayersTurn != 0)
            {
                PromptCPU(CurrentPlayersTurn);
            }
            yield break;
        }
        prompt = "No, go fish...";
        Debug.Log(prompt);

        CanContinue = false;
        DialogueManager.Instance.ShowPrompt(prompt);
        AudioManager.Instance.PlaySound("no");
        yield return new WaitUntil(() => CanContinue);

        AddCard(PullRandom(), askingPlayer);
        HandVisualizer.Instance.CanShowButton = false;

        NextTurn();
    }

    private void PromptCPU(int currentPlayersTurn)
    {
        ComputerPlayers[CurrentPlayersTurn - 1].ProcessTurn(PlayerCards[currentPlayersTurn]);
    }

    public void EndGame()
    {
        int currentLeader = -1;
        int currentLeaderStacks = -1;


        for (int i = 0; i < PlayerStacks.Count; i++)
        {
            
            if (PlayerStacks[i].Count > currentLeaderStacks)
            {
                currentLeader = i;
                currentLeaderStacks = PlayerStacks[i].Count;
            }
        }

        if (currentLeader != 0)
            AudioManager.Instance.PlaySound("lose");
        Debug.Log($"Game finished! Player {currentLeader} won! (0 is human)");
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

        CurrentPlayersTurn = ++CurrentPlayersTurn%NumberOfPlayers;

        Debug.Log("current players turn: " + CurrentPlayersTurn);

        if (CurrentPlayersTurn > 0)
        {
            HandVisualizer.Instance.CanShowButton = false;
            PromptCPU(CurrentPlayersTurn);
        }
        else
        {
            HandVisualizer.Instance.CanShowButton = true;
        }


        UpdatePlayerVisual();
    }

    public void UpdatePlayerVisual()
    {
        if (CurrentPlayersTurn == 0) 
        {
            ContextButton.SetActive(true);
        }
        if(HandVisualizer.Instance != null)
            HandVisualizer.Instance.UpdateHand(PlayerCards[0]);
    }

    public void RemovePlayerCards(Card[] cards, int player)
    {
        for (int i = 0; i < cards.Length; i++) 
        {
            PlayerCards[player].Remove(cards[i]);
        }
        UpdatePlayerVisual();
    }

    public CardObject MakeNewCardObject(Card card)
    {

        GameObject cardObject = Instantiate(CardObject);

        GameObject cardSlot = Instantiate(CardSlotObject);

        int index = (int)card.Suit * 13 + (int)card.Rank;
        cardObject.GetComponent<CardObject>().AddData(card, cardSlot.transform, index);
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
