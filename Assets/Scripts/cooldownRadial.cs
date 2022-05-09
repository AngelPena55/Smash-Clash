using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldownRadial : MonoBehaviour
{
    public Image progressBar;
    public float cooldownTimer;
    public float cooldownTime;
    public bool isCooldown = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0f)
        {
            isCooldown = false;
            progressBar.fillAmount = 0f;
        }
        else
        {
            progressBar.fillAmount = (cooldownTimer / cooldownTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
        }
    }
}
