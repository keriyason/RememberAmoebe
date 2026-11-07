using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private string endingScene = "Ending";

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        SceneTransition.Load(endingScene);
    }
}