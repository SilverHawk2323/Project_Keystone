using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public int maxCardAmount = 6;
    public int resources;
    public int currentCardAmount;
    public int CardsInDeck;
    public CardBase[] cardsInHand;
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

    [ContextMenu("Draw Cards")]
    public void DrawCards()
    {
        for(int x = 0; x < maxCardAmount; x++)
        {
            if (!cardPositions[x].GetComponent<CardPosition>().spotTaken)
            {
                CardBase current = cardsInDeck[0];
                if (current == null) return;
                cardsInDeck.Remove(current);
                CardBase cardSpawned = Instantiate(current, cardPositions[x].position, Quaternion.Euler(0, -90, -90));
                cardsInHand[x] = cardSpawned;
                cardPositions[x].GetComponent<CardPosition>().spotTaken = true;
                currentCardAmount++;
            }
        }
        int i = 0;
        foreach(var card in cardsInHand)
        {
            card.cardProperty.teamNumber = teamNumber;
            i++;
        }
    }

    public void RemoveSpawnedCard(CardBase targetCard)
    {
        int i = Array.IndexOf(cardsInHand,targetCard);
        print(i);
        cardPositions[i].GetComponent<CardPosition>().spotTaken = false;
        cardsInHand[i] = null;
    }
}
