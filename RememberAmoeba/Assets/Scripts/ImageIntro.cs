using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageIntro : MonoBehaviour
{
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float holdTime = 3f;
    [SerializeField] private float fadeOutTime = 2f;

    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        if (group == null) group = gameObject.AddComponent<CanvasGroup>();

        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return new WaitForSecondsRealtime(startDelay);

        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSecondsRealtime(holdTime);
        yield return Fade(1f, 0f, fadeOutTime);

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        group.alpha = to;
    }
}
