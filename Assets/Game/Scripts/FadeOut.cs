using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public GameObject gameStartScene;
    void Start()
    {
        FadeOutImg();
    }

    // Update is called once per frame
    void FadeOutImg()
    {
        Sequence sequence = DOTween.Sequence()
        .Append(transform.DOMoveX(55f, 3f).SetEase(Ease.OutQuad))
        .SetDelay(0.5f);
        sequence.Play().SetDelay(2f).OnComplete(() => {
            gameStartScene.SetActive(true);
        });
    }
}
