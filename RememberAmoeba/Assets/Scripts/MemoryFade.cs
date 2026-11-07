
using System.Collections;
using UnityEngine;
 
[RequireComponent(typeof(CanvasGroup))]
public class MemoryFade : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float holdTime = 2f;
    [SerializeField] private AudioClip erasedSound;

    private CanvasGroup group;
    private AudioSource audioSource;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;

        audioSource = GetComponent<AudioSource>();
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
        yield return Fade(1f);

        popup.Hide();
        yield return new WaitForSecondsRealtime(holdTime);

        yield return Fade(0f);
        group.blocksRaycasts = false;
        popup.ReturnControl();
    }

    private IEnumerator Fade(float target)
    {
        float start = group.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }

        group.alpha = target;
    }
}