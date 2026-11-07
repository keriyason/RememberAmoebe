using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance; //other scripts can reference 

    [SerializeField] private float fadeDuration = 0.5f; //fade tieme

    private CanvasGroup group;//controls the canva black creen
    private bool loading; //loading time

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] // loads on game start
    private static void Init()
    {
        if (instance != null) return;
        instance = new GameObject("SceneTransition").AddComponent<SceneTransition>();
    }

    public static void Load(string sceneName) //scene name in inspector
    {
        if (instance != null && !instance.loading)
            instance.StartCoroutine(instance.Transition(sceneName, -1));
    }

    public static void Load(int buildIndex) //calls the scene build number 
    {
        if (instance != null && !instance.loading)
            instance.StartCoroutine(instance.Transition(null, buildIndex));
    }

    private void Awake()
    {
        if (instance != null && instance != this) //if there is a second copy delete
        { 
            Destroy(gameObject);
            return;
        }

        instance = this; //keeps object alive when scene changes
        DontDestroyOnLoad(gameObject);

        Canvas canvas = gameObject.AddComponent<Canvas>(); //makes a canvas that in on top
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        group = gameObject.AddComponent<CanvasGroup>(); //start invisible and then fades in
        group.alpha = 0f;
        group.blocksRaycasts = false;

        GameObject panel = new GameObject("Fade"); //makes a black image to fade in the screen
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
        loading = true; //while changing scenes
        group.blocksRaycasts = true;

        yield return Fade(1f); //fades to black

        AsyncOperation op = sceneName != null
            ? SceneManager.LoadSceneAsync(sceneName) //loading the scene
            : SceneManager.LoadSceneAsync(buildIndex);

        while (!op.isDone) //waits until loding is done
            yield return null; 

        yield return Fade(0f); //fade in

        group.blocksRaycasts = false; // lets u click
        loading = false;
    }

    private IEnumerator Fade(float target) //black screeb to clear
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