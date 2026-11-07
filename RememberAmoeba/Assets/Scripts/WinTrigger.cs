using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private string endingScene = "Ending"; //triggers to ending scene

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return; //player tag
        triggered = true; //if true trigger 
        SceneTransition.Load(endingScene); //transitions
    }
}