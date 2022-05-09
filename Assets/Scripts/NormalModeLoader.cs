using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalModeLoader : MonoBehaviour
{
    public string newscene;
    public void LoadNormal()
    {
        PlayerCombat.useSave = 1;
        PlayerCombat.normalMode = true;
        SceneManager.LoadScene(newscene);
    }    
    public void LoadHard()
    {
        PlayerCombat.Level = 0;
        PlayerCombat.useSave = 0;
        PlayerCombat.normalMode = false;
        SceneManager.LoadScene(newscene);
    }
}
