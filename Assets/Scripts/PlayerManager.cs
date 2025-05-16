using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxCardAmount = 6;
    public int resources;
    public int currentCardAmount;
    public int CardsInDeck;
    public List<ScriptableCardBase> cards;
    public int teamNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCards()
    {
        foreach(var card in cards)
        {
            card.teamNumber = teamNumber;
        }
    }
}
