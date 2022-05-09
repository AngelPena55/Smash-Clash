using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int totem;
    public int abBoost;
    public int wealth;
    public int currenthealth;
    public int maxhealth;
    public int ability;
    public int totalArrows;
    public int maxArrows;
    public int level;

    //Collect the player data into readable variables
    public PlayerData (PlayerCombat player)
    {
        currenthealth = player.currentPlayerHealth;
        maxhealth = player.maxPlayerHealth;
        totem = player.totem;
        abBoost = player.attackBoostTotal;
        wealth = player.wealth;
        level = PlayerCombat.Level;
        ability = player.GetComponent<Spell>().CurrentSpell;
        totalArrows = player.GetComponent<Spell>().totalArrows;
        maxArrows = player.GetComponent<Spell>().maxArrows;
        
    }
   
}
