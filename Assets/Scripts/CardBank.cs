using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBank : MonoBehaviour
{
    //singleton
    public static CardBank Instance;

    private static readonly List<string> suitOrder = new List<string> { "clubs", "diamond", "hearts", "spades" };
    public Sprite[] sprites;

    private void Awake()
    {

        Instance = this;
        sprites = Resources.LoadAll<Sprite>("UI Cards");
        //sprites = Resources.LoadAll("/UI Cards", typeof(Sprite));
        Debug.Log("spritecount is: "+sprites.Length);
        // Sort sprites by suit and then by rank
        sprites = sprites
            .OrderBy(sprite =>
            {
                // Extract suit from filename, handling both "_" and "-" separators
                string filename = sprite.name.ToLower(); // Make filename lowercase to avoid case sensitivity issues
                string[] parts = filename.Split(new char[] { '_', '-' });

                foreach(var part in parts) Debug.Log(part);

                // Expect "cards" as first part, "suit" as second part
                string suit = parts.Length > 1 ? parts[1] : null;

                if (!suitOrder.Contains(suit))
                {
                    Debug.LogWarning($"Unexpected suit in filename: {filename}");
                }

                return suitOrder.IndexOf(suit); // Order by suit using predefined order
            })
            .ThenBy(sprite =>
            {
                // Extract rank from filename (after "-" or "_" before the rank number)
                string filename = sprite.name;
                var match = System.Text.RegularExpressions.Regex.Match(filename, @"[-_](\d+)");

                if (!match.Success)
                {
                    Debug.LogWarning($"Could not determine rank for filename: {filename}");
                    return int.MaxValue; // Set a default large number if rank is not found
                }

                string rankPart = match.Groups[1].Value;
                return int.Parse(rankPart); // Order by rank
            })
            .ToArray();
    }
    public Sprite GetSprite(int index) 
    {
        Debug.Log($"returning sprite {sprites[index].name}, from index {index}");
        return sprites[index];
    } 

}
