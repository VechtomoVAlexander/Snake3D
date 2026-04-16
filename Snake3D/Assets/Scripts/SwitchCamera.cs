using System.Collections;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam1; // CameraTopView
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam2; // CameraFirstPerson
    [SerializeField] private PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(StopPlayerFor(5f, () =>
            {
                CameraManager.SwitchCamera(cam1);
            }));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(StopPlayerFor(5f, () =>
            {
                CameraManager.SwitchCamera(cam2);
            }));
        }
    }

    private IEnumerator StopPlayerFor(float seconds, System.Action afterStop)
    {
        if (playerMovement != null)
        {
            playerMovement.StopMovement(true);
        }

        afterStop?.Invoke();

        yield return new WaitForSeconds(seconds);

        if (playerMovement != null)
        {
            playerMovement.StopMovement(false);
        }
    }
}