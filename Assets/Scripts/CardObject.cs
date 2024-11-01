using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    public Card CardData;

    public TextMeshProUGUI SuitText;
    public TextMeshProUGUI RankText;

    public void AddData(Card card)
    {
        CardData = card;

        if (CardData == null) return;

        SuitText.text = CardData.Suit.ToString();
        RankText.text = (int)CardData.Rank < 10? ((int)CardData.Rank).ToString(): CardData.Rank.ToString();
    }
}
