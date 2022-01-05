
using UnityEngine;
using System;
[Serializable]
public class Card 
{
    public int _value;
    public enum Suit { hearts, diamonds, clubs, spades };
    public Suit _suit;

    public Card()
    {
        _value = 1;
        _suit = Suit.hearts;
    }
    public Card(int value, Suit suit)
    {
        _value = value;
            _suit = suit;
    }


}
