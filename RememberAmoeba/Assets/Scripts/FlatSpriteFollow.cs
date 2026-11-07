using UnityEngine;

public class FlatSpriteFollow : MonoBehaviour
{
    [SerializeField] private Transform player; //Capsule
    [SerializeField] private Vector3 offset = new Vector3(0f, -0.95f, 0f); //capsule center
    [SerializeField] private bool turnWithPlayer = true; //untick to keep the sprite from turning

    private void LateUpdate()
    {
        if (player == null) return; //does nothing until the player is assigned

        transform.position = player.position + offset; //stays under the player
        float yaw = turnWithPlayer ? player.eulerAngles.y : 0f; //which way the player is facing
        transform.rotation = Quaternion.Euler(90f, yaw, 0f); //lies flat, turns with the player
    }
}