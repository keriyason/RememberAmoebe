using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;

    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup group;
    private bool loading;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (instance != null) return;
        instance = new GameObject("SceneTransition").AddComponent<SceneTransition>();
    }

    public static void Load(string sceneName)
    {
        if (instance != null && !instance.loading)
            instance.StartCoroutine(instance.Transition(sceneName, -1));
    }

    public static void Load(int buildIndex)
    {
        if (instance != null && !instance.loading)
            instance.StartCoroutine(instance.Transition(null, buildIndex));
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;

        GameObject panel = new GameObject("Fade");
        panel.transform.SetParent(transform, false);
        Image image = panel.AddComponent<Image>();
        image.color = Color.black;
        RectTransform rect = image.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private IEnumerator Transition(string sceneName, int buildIndex)
    {
        loading = true;
        group.blocksRaycasts = true;

        yield return Fade(1f);

        AsyncOperation op = sceneName != null
            ? SceneManager.LoadSceneAsync(sceneName)
            : SceneManager.LoadSceneAsync(buildIndex);

        while (!op.isDone)
            yield return null;

        yield return Fade(0f);

        group.blocksRaycasts = false;
        loading = false;
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