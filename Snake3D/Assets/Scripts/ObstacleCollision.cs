using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollision : MonoBehaviour
{
    [SerializeField] private AudioClip obstacleHitSFX;
    private static AudioSource globalAudioSource;

    void Awake()
    {
        if (globalAudioSource == null)
        {
            GameObject audioObj = new GameObject("GlobalSFX");
            DontDestroyOnLoad(audioObj);
            globalAudioSource = audioObj.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnakeMain"))
        {
            PlayObstacleSound();
            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.StopMovement(true);
            }
            if (GameBoss.Instance != null)
            {
                GameBoss.Instance.GameOver();
            }
            else
            {
                Debug.LogError("Gameboss keeps not coming to work");
            }
        }
    }

    private void PlayObstacleSound()
    {
        if (obstacleHitSFX != null && globalAudioSource != null)
        {
            globalAudioSource.PlayOneShot(obstacleHitSFX);
        }
    }
}