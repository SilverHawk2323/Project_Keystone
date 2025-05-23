using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxCardAmount = 6;
    public int resources;
    public int currentCardAmount;
    public int CardsInDeck;
    public List<CardBase> cardsInHand = new List<CardBase>();
    public List<CardBase> cardsInDeck = new List<CardBase>();
    public int teamNumber;
    public Transform[] cardPositions;

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
        for (int i = 0; currentCardAmount < cardPositions.Length; i++)
        {

        }
        foreach(var card in cardsInHand)
        {
            card.cardProperty.teamNumber = teamNumber;
            card.transform.position = cardPositions[0].position;
            card.transform.rotation = cardPositions[0].rotation;
        }
    }
}
