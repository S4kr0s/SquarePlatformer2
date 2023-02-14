using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageManager : MonoBehaviour
{
    [SerializeField] private Image imageWorld1;
    [SerializeField] private Image imageWorld2;
    [SerializeField] private Image imageWorld3;

    public void SwitchToImage(int worldId)
    {
        switch (worldId)
        {
            case 1:
                StartCoroutine(TransitionToFullOverTime(imageWorld1, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld2, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld3, 1f));
                return;
            case 2:
                StartCoroutine(TransitionToFullOverTime(imageWorld2, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld1, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld3, 1f));
                return;
            case 3:
                StartCoroutine(TransitionToFullOverTime(imageWorld3, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld1, 1f));
                StartCoroutine(TransitionToTransparentOverTime(imageWorld2, 1f));
                return;
            default:
                return;
        }
    }

    IEnumerator TransitionToTransparentOverTime(Image image, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            if (image.color.a > Color.Lerp(Color.white, new Color(1, 1, 1, 0), normalizedTime).a)
                image.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), normalizedTime);
            yield return null;
        }
        //without this, the value will end at something like 0.9992367
        image.color = new Color(1, 1, 1, 0);
    }
    IEnumerator TransitionToFullOverTime(Image image, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            image.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, normalizedTime);
            yield return null;
        }
        //without this, the value will end at something like 0.9992367
        image.color = Color.white;
    }
}
