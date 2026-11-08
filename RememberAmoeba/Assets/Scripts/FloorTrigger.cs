using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (respawnPoint == null)
        {
            Debug.LogWarning("RespawnTrigger: drag a respawn point into the slot", this);
            return;
        }

        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null) controller.enabled = false;
        other.transform.position = respawnPoint.position;
        if (controller != null) controller.enabled = true;
    }
}
