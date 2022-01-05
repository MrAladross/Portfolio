
using UnityEngine;
using System;
[Serializable]
class Card 
{
    private int _value;
    public enum Suit { hearts, diamonds, clubs, spades };
    private Suit _suit;

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
    public int GetValue(){
         return _value;
    }
    public Suit GetSuit(){
         return _suit;
    }
    


}
