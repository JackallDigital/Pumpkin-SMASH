using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas mainMenu;
    public AudioSource audioSourceBG;
    public AudioSource audioSourceDeath;

    public bool hasSetCanvas = false;

    void Start()
    {
        mainMenu.gameObject.SetActive(true);
        audioSourceBG.Play();
    }

    void Update()
    {
        if (HammerController.Instance.isDead && !hasSetCanvas) {
            if(HammerController.Instance.livesLeftTillBellTolls <= 0)
                audioSourceDeath.Play();
            mainMenu.gameObject.SetActive(true);
            hasSetCanvas = true;
        }
    }

    public void setMenuCanvas() {
        hasSetCanvas = false;
        mainMenu.gameObject.SetActive(false);
        HammerController.Instance.isDead = false;
        HammerController.Instance.pumpkinsSmashed = 0;
        HammerController.Instance.livesLeftTillBellTolls = 5;
        HammerController.Instance.scoreText.text = "Pumpkins Smashed " + 0;
        HammerController.Instance.bellText.text = "Bell tolls in " + 5;
    }
}