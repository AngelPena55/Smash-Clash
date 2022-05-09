using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{

    public string newscene;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       // when the player touches the pillar that the boss drops 
       // We access the save manager and we store all the information the player has collected and earned
       // after that we load the new scene and play the game over again with the past progress the player had.
        PlayerCombat.useSave = 1;
        PlayerCombat.Level++;
        GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>().SavePlayer();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().LevelManager();
        SceneManager.LoadScene(newscene);
    }
}
