using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntryIntro : MonoBehaviour
{
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float holdTime = 3f;
    [SerializeField] private float fadeTime = 2f;

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

        group.alpha = 1f;

        yield return new WaitForSecondsRealtime(holdTime);

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }

        group.alpha = 0f;
        gameObject.SetActive(false);
    }
}
