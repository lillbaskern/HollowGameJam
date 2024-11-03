using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    //singleton
    public static DialogueManager Instance;

    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;


    

    private string playerSays = "Hello world!";
    private string askForCard = "Player #, " + "CardRank" + " ?";
    private float SecondMultiplier = 0.3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
            Instance = this;

        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        Setup(playerSays);

    }


    //returns false until prompt is over
    public void ShowPrompt(string prompt)
    {
        StartCoroutine(ShowPromptRoutine(prompt));
    }

    IEnumerator ShowPromptRoutine(string prompt)
    {
        int i = 0;
        textMeshPro.text = prompt;

        while (textMeshPro.alpha < 255)
        {
            backgroundSpriteRenderer.color = new(backgroundSpriteRenderer.color.r, backgroundSpriteRenderer.color.g, backgroundSpriteRenderer.color.b, -40 + i * Time.deltaTime); //times 2 so background appears faster than text;

            textMeshPro.color = new(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, i * Time.deltaTime);
            i++;
            yield return null;
        }


        yield return new WaitForSeconds(prompt.Length * SecondMultiplier);

        i = 100;
        while (textMeshPro.alpha >= 0)
        {
            backgroundSpriteRenderer.color = new(backgroundSpriteRenderer.color.r, backgroundSpriteRenderer.color.g, backgroundSpriteRenderer.color.b, i * 2); //times 2 so background appears faster than text;

            textMeshPro.color = new(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, i);
            i--;
            yield return null;
        }
        CardManager.Instance.CanContinue = true;

        yield return null;
    }

    //
    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.GetRenderedValues(false);
        textMeshPro.ForceMeshUpdate();

        
        textMeshPro.color = new(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0);

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 margin = new(2f, 2f);

        backgroundSpriteRenderer.size = textSize + margin;
    }


}
