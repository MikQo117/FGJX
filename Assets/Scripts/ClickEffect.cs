using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField]
    private Transform sprite;
    [SerializeField]
    new private SpriteRenderer renderer;
    [SerializeField]
    private Gradient colorOverLifetime;
    [SerializeField]
    private AnimationCurve sizeOverLifetime;
    [SerializeField]
    private float lifetime;

    private float currentLifetime;

    public void Play()
    {
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        bool finished = false;
        while (finished)
        {
            if (currentLifetime <= lifetime)
            {
                finished = true;
                currentLifetime = lifetime;
            }
            renderer.color = colorOverLifetime.Evaluate(currentLifetime / lifetime);
            sprite.localScale = Vector3.one * sizeOverLifetime.Evaluate(currentLifetime / lifetime);
            currentLifetime += Time.deltaTime;
            yield return null;
        }
    }
}
