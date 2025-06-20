using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int maxCardAmount = 6;
    public int currentResources = 4;
    private int maxResources = 4;
    public int currentCardAmount;
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
                //Get a random card in the deck
                CardBase current = cardsInDeck[UnityEngine.Random.Range(0, cardsInDeck.Count)]; //Important to note, the system namespace and the UnityEngine namespace both have random, UnityEngine is the one we want.
                if (current == null) return;
                cardsInDeck.Remove(current);
                CardBase cardSpawned = Instantiate(current, cardPositions[x].position, Quaternion.Euler(0, -90, -90));
                cardsInHand[x] = cardSpawned;
                cardSpawned.SetOriginalCardPosition(cardPositions[x].transform);
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

    public void DeployPhase()
    {
        DrawCards();
        currentResources = maxResources;
        maxResources++;
        maxResources = Mathf.Clamp(maxResources, 4, 10);
        foreach (CardBase card in cardsInHand)
        {
            TextMeshProUGUI text = card.GetComponentInChildren<TextMeshProUGUI>();
            if (card is WarriorCard)
            {
                text.text = "Warrior Unit costs 1\n Spawns Warrior";
            }
            if (card is ShieldCard)
            {
                text.text = "Shield Unit costs 3\n Spawns Shield";
            }
            if (card is RangerCard)
            {
                text.text = "Ranger Unit costs 2\n Spawns Ranger ";
            }
        }
    }

    public void BattlePhase()
    {
        foreach(var card in cardsInHand)
        {
            if (card == null)
            {
                continue;
            }
            TextMeshProUGUI text = card.GetComponentInChildren<TextMeshProUGUI>();
            if(card is WarriorCard)
            {
                text.text = "Warrior Unit costs 1\n Increases the damage of all friendly units by 1";
            }
            if(card is ShieldCard)
            {
                text.text = "Shield Unit costs 3\n Adds 4 shields to all friendly units";
            }
            if(card is RangerCard)
            {
                text.text = "Ranger Unit costs 2\n Multiplies the attack speed for all friendly units by 2";
            }
            
        }
    }
}
