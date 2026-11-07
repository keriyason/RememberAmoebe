using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName; //title

    private void OnTriggerEnter(Collider other) //if player enters on trigger collider w player name tag it will transition the scene
    {
        if (other.CompareTag("Player"))
            SceneTransition.Load(sceneName);
    }
}
