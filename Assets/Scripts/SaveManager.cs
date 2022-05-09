using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject player;

    //Sends the player data collected from PlayerData.cs and saves it using SaveSystem.cs
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player.GetComponent<PlayerCombat>());
    }
    //Returns the data back to the player using SaveSystem.cs
    public void LoadPlayer()
    {
        PlayerCombat combat = player.GetComponent<PlayerCombat>();
        
        PlayerData data = SaveSystem.LoadPlayer();

        combat.totem = data.totem;
        if (combat.totem > 0)
        {
            combat.totemPanel.SetActive(true);
        }
        combat.totemNum.text = ""+ combat.totem;
        combat.attackBoostTotal = data.abBoost;
        if (combat.attackBoostTotal > 0)
        {
            combat.abPanel.SetActive(true);
        }
        combat.abNum.text = "" + combat.attackBoostTotal;
        combat.wealth = data.wealth;
        combat.currencyUI.text = "" + combat.wealth;
        combat.currentPlayerHealth = data.currenthealth;
        combat.playerhealthbar.SetHealth(combat.currentPlayerHealth);
        combat.maxPlayerHealth = data.maxhealth;
        PlayerCombat.Level = data.level;
        combat.levelNum.text = "" + PlayerCombat.Level;

        player.GetComponent<Spell>().CurrentSpell = data.ability;
        player.GetComponent<Spell>().maxArrows = data.maxArrows;
        player.GetComponent<Spell>().totalArrows = data.totalArrows;
        player.GetComponent<Spell>().arrowNum.text = "" + player.GetComponent<Spell>().totalArrows;
    }
}
