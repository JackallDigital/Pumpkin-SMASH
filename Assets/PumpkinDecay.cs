using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinDecay : MonoBehaviour
{
    private float m_DecayTime = 2f;
    private float t;
    private float randomDecayDelay;
    private Vector3 originalSize = Vector3.one * 5;
    private bool hasToDespawn = false;

    void Start()
    {
        t = 0;
        randomDecayDelay = Random.Range(1, 3);
        StartCoroutine(ChangePumpkinScale(Vector3.zero, originalSize, 0.5f));
    }

    void Update()
    {
        if (!HammerController.Instance.isDead) {
            if (t < m_DecayTime + randomDecayDelay) {
                t += Time.deltaTime;
            }
            else {
                hasToDespawn = true;
                StartCoroutine(ChangePumpkinScale(originalSize, Vector3.zero, 0.5f));
            }
        }
        else
            Spawner.Instance.DestroyObjectAndReleaseSpawnPoint(gameObject);
    }

    private IEnumerator ChangePumpkinScale(Vector3 startScale, Vector3 endScale, float scaleAnimationDuration) {
        float elapsedTime = 0;

        while (elapsedTime < scaleAnimationDuration) {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / scaleAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale; // Ensure it reaches the exact target scale.

        if (hasToDespawn) {
            if (--HammerController.Instance.livesLeftTillBellTolls == 0)
                HammerController.Instance.isDead = true;

            Spawner.Instance.DestroyObjectAndReleaseSpawnPoint(gameObject);
        }
    }
}
