using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    

    private string playerSays = "Hello world!";
    private string askForCard = "Player #, " + "CardRank" + " ?";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Setup(playerSays);  
    }

    //
    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.GetRenderedValues(false);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 margin = new(2f, 2f);

        backgroundSpriteRenderer.size = textSize + margin;
    }


}
