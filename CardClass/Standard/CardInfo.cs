using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    [SerializeField]
    private Card card;
    [SerializeField]
    private Player player;

    public CardInfo()
    {
        card = new Card();
        player = new Player();
    }

    public void SetCard(Card c)
    {
        card = c;
    }
    public void SetPlayer(Player p)
    {
        player = p;
    }
}
