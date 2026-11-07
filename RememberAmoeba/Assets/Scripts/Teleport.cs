using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination; // where the player teleports
    [SerializeField] private float cooldown = 1.5f; //second before teleport can be used

    private static float lastTeleportTime = -999f; //checks for the last teleport

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] //resets the timer each time u press play
    private static void ResetCooldown()
    {
        lastTeleportTime = -999f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // referenfces the player
        if (Time.time < lastTeleportTime + cooldown) return; // do nothing if player just teleported

        lastTeleportTime = Time.time; //recalls when the player last teleported

        CharacterController controller = other.GetComponent<CharacterController>(); //gets players charcter controller

        if (controller != null) controller.enabled = false; //turns controller off
        other.transform.position = destination.position; //teles player
        if (controller != null) controller.enabled = true; //turns controller on
    }
}