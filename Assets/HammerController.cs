using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    private Animator animator;
    private bool isSwinging = false;
    public AudioSource pumkinSmashedSound;
    public AudioSource hammerSwingSound;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bellText;

    public int pumpkinsSmashed = 0;
    public int livesLeftTillBellTolls = 0;
    public bool isDead = true;

    public static HammerController Instance { get; private set; }

    private void Awake() {
        isDead = true;
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (!isDead && livesLeftTillBellTolls > 0) {
            if (Input.GetMouseButtonDown(0) && !isSwinging) {
                isSwinging = true;

                hammerSwingSound.time = 0.1f;
                hammerSwingSound.Play();
                animator.Play("HammerSmash");

                isSwinging = false;
            }
            bellText.text = "Bell tolls in " + livesLeftTillBellTolls;
        }
        else
            bellText.text = "Bell tolls in " + 0;
    }
    
    private IEnumerator OnTriggerEnter(Collider other) {
        scoreText.text = "Pumpkins Smashed "+ ++pumpkinsSmashed;
        pumkinSmashedSound.PlayDelayed(-0.5f);

        float flattenTime = 0.2f;
        float elapsedTime = 0;

        Vector3 endScale = new Vector3(5, 0.2f, 5);
        while (elapsedTime < flattenTime) {
            other.gameObject.transform.localScale = Vector3.Lerp(Vector3.one*5, endScale, elapsedTime / flattenTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        other.gameObject.transform.localScale = endScale;

        Spawner.Instance.DestroyObjectAndReleaseSpawnPoint(other.gameObject);
    }
}