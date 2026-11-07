
using System.Collections;
using UnityEngine;
 
[RequireComponent(typeof(CanvasGroup))] //calls to canvas group
public class MemoryFade : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f; //fade
    [SerializeField] private float holdTime = 2f; //time that the screen holds
    [SerializeField] private AudioClip erasedSound; //audio on click

    private CanvasGroup group; //refeernces canvas
    private AudioSource audioSource; //calls to audio source

    private void Awake()
    {
        group = GetComponent<CanvasGroup>(); //activates group
        group.alpha = 0f; //invis
        group.blocksRaycasts = false;

        audioSource = GetComponent<AudioSource>(); //plays audio source
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Play(ItemPopup popup)
    {
        if (erasedSound != null) audioSource.PlayOneShot(erasedSound); 
        StartCoroutine(Run(popup));
    }

    private IEnumerator Run(ItemPopup popup)
    {
        group.blocksRaycasts = true;
        yield return Fade(1f); //fade in

        popup.Hide();
        yield return new WaitForSecondsRealtime(holdTime); //holds

        yield return Fade(0f); //fade out
        group.blocksRaycasts = false;
        popup.ReturnControl();
    }

    private IEnumerator Fade(float target)
    {
        float start = group.alpha; //on start fade
        float t = 0f;

        while (t < fadeDuration) //tracks duration of fade
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }

        group.alpha = target;
    }
}