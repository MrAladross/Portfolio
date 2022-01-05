using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    [SerializeField]
    private Card _card;
    [SerializeField]
    private Player _player;

    public CardInfo()
    {
        _card = new Card();
        _player = new Player();
    }

    public void SetCard(Card c)
    {
        _card = c;
    }
    public void SetPlayer(Player p)
    {
        _player = p;
    }
    public Card GetCard()
    {
        return _card;
    }
    public Player GetPlayer()
    {
        return _player;
    }
}
