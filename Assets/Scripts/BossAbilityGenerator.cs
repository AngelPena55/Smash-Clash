using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilityGenerator : MonoBehaviour
{
    public GameObject[] Abilites;
    public GameObject[] PlayerAbilities;
    public bool onetimespell = false;
    public int CurrentSpellNumber;
    private int player;

    // Start is called before the first frame update
    //Generates an ability for bosses and makes sure it doesnt match the player's ability
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().CurrentSpell;
        if (onetimespell == false)
        {
            CurrentSpellNumber = Random.Range(0, Abilites.Length);
            GetComponent<EnemyCombat>().spellPrefab = Abilites[CurrentSpellNumber];
            onetimespell = true;
            while (Abilites[CurrentSpellNumber] == PlayerAbilities[player])
            {
                Debug.Log(player);
                Debug.Log(CurrentSpellNumber);
                CurrentSpellNumber = Random.Range(0, Abilites.Length);
                GetComponent<EnemyCombat>().spellPrefab = Abilites[CurrentSpellNumber];
                onetimespell = true;
            }
        }
    }
}
