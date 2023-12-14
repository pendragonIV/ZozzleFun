using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorHand : MonoBehaviour
{
    private void Start()
    {
        if(LevelManager.instance.currentLevelIndex == 0)
        {
            Vector3 destination = transform.localPosition + new Vector3(0, -.5f, 0);
            this.transform.DOLocalMove(destination,.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
