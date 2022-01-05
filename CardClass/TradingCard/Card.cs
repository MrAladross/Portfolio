using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite sprite;
    public int cost;
    public int movement;
    public int health;
    public int damage;
    public int range;
    public string cardName;
    public enum Attribute {
        Physical, Magic
    };
    public Attribute attribute;

    public Card()
    {
        sprite = null;
        cost = 0;
        movement = 0;
        health = 0;
        damage = 0;
        range = 0;
        cardName = "default card name";
        attribute = Attribute.Physical;
    }

    public Card(Sprite spriteImage, int Cost, int Movement, int Health, int Damage, int Range, string Name, Attribute _attribute){
        sprite = spriteImage;
        cost = Cost;
        movement = Movement;
        health = Health;
        damage = Damage;
        range = Range;
        cardName = Name;
        attribute = _attribute;
    }

}
