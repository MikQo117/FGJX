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
    private ObjectPool pool;
    private Transform target;

    public void Play(Transform target)
    {
        pool = FindObjectOfType<ObjectPool>();
        currentLifetime = 0f;
        this.target = target;
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        bool finished = false;
        while (!finished && target.gameObject.activeSelf)
        {
            if (currentLifetime >= lifetime)
            {
                finished = true;
                currentLifetime = lifetime;
            }
            transform.position = target.position;
            renderer.color = colorOverLifetime.Evaluate(currentLifetime / lifetime);
            sprite.localScale = Vector3.one * sizeOverLifetime.Evaluate(currentLifetime / lifetime);
            currentLifetime += Time.deltaTime;
            yield return null;
        }
        pool.ReturnItem(gameObject);
    }
}
