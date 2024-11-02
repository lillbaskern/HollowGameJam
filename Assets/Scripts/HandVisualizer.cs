using System.Collections.Generic;
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
    public static HandVisualizer Instance {  get; private set; }

    public List<Transform> PlayerHand {  get; private set; }


    

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

    }


    public void UpdateHand()
    {
        PlayerHand.Clear();
        for (int i = 0; i < PlayerHand.Count; i++)
        {
            PlayerHand.Add(transform.GetChild(i));
        }
    }


}
