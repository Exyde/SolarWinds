using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager _instance;

    [SerializeField][Range(.1f, 3f)] float _defaultTransitionDuration = 1f;
    [SerializeField][Range(.1f, 3f)] float _blackTimeDuration = .5f;

    [SerializeField] Image _screenFader;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _blackColor;


    private void Awake() {
        if (_instance == null){
            _instance = this;
        } else{
            Destroy(this);
        }
    }

    private void OnEnable() {
    }

    private void OnDisable(){
    }

    public IEnumerator FullFadeCycle(){
        yield return StartCoroutine(FadeToBlack(_defaultTransitionDuration));
        yield return new WaitForSeconds(_blackTimeDuration);
        yield return StartCoroutine(FadeToNormal(_defaultTransitionDuration));
    }

    public void FullFadeCycleCallback(){
        StartCoroutine(FullFadeCycle());
    }

    public IEnumerator FadeToBlack(float timeToFade){

        float timeElapsed = 0;

        while (timeElapsed < timeToFade){

            timeElapsed += Time.deltaTime;
            _screenFader.color = Color.Lerp(_normalColor, _blackColor, timeElapsed / timeToFade);
            yield return null;    
        }
    }

    public IEnumerator FadeToNormal(float timeToFade ){

        float timeElapsed = 0;

        while (timeElapsed < timeToFade){

            timeElapsed += Time.deltaTime;
            _screenFader.color = Color.Lerp(_blackColor, _normalColor, timeElapsed / timeToFade);
            yield return null;    
        }
    }

    [ContextMenu("Fade To Black")]
    public void FadeToBlackCallback(){
        StartCoroutine(FadeToBlack(_defaultTransitionDuration));
    }

    [ContextMenu("Fade To Normal")]
    public void FadeToNormalCallback(){
        StartCoroutine(FadeToNormal(_defaultTransitionDuration));
    }

}
