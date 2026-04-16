using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnakeMain"))
        {
            PlayCoinSound();

            if (GameBoss.Instance != null)
            {
                GameBoss.Instance.AddScore();
            }
            else
            {
                Debug.LogWarning("GameBoss не найден! Создайте GameBoss на сцене.");
            }

            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.GrowSnake();
            }

            Destroy(gameObject);
        }
    }

    private void PlayCoinSound()
    {
        if (coinPickupSFX != null)
        {
            AudioSource source = GetComponent<AudioSource>();
            if (source != null)
            {
                source.PlayOneShot(coinPickupSFX);
                return;
            }
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
        }
    }
}