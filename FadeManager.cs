using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ALPHA {
    ZERO, LITTLE, MAX, NORMAL 
}

public enum DURATION {
    SHORT, NORMAL, LONG, IMMEDIATE, SLITELONG
}

public class FadeManager : MonoBehaviour {
    public static Dictionary<ALPHA, float> alphaDic = new Dictionary<ALPHA, float>() {
        [ALPHA.MAX] = 1f,
        [ALPHA.ZERO] = 0f,
        [ALPHA.LITTLE] = 0.3f,
        [ALPHA.NORMAL] = 0.5f
    };

    public static Dictionary<DURATION, float> durationDic = new Dictionary<DURATION, float>() {
        [DURATION.LONG] = 1f,
        [DURATION.SLITELONG] = 0.7f,
        [DURATION.NORMAL] = 0.5f,
        [DURATION.SHORT] = 0.3f,
        [DURATION.IMMEDIATE] = 0f
    };


    public static FadeManager makeInstance(GameObject gm) {
        FadeManager fadeManager = gm.GetComponent<FadeManager>();
        if (fadeManager != null) {
            return fadeManager;
        }
        return (gm.AddComponent<FadeManager>()) as FadeManager;
    } 

    private void checkTargetHasImageOrText(Transform target, Action<Graphic> graphicCallback, Action<Image> imageCallback = null, Action<Text> textCallback = null) {
        if (!target) {
            return;
        }
        Image targetImage = target.GetComponent<Image>();
        if (targetImage != null) {
            if (imageCallback != null) {
                imageCallback(targetImage);
            } else {
                graphicCallback(targetImage);
            }
        }
        
        Text targetText = target.GetComponent<Text>();
        if (targetText != null) {
            if (textCallback != null) {
                textCallback(targetText);
            } else {
                graphicCallback(targetText);
            }
        }
    }

    // 해당 타겟이 CanvasRenderer가 있는지 확인하고 있다면 callBack을 수행한다.
    private void checkTargetHasCanvasRenderer(Transform target, Action<CanvasRenderer> callBack) {
        if (!target) // 만약 목표가 없다면 그냥 종료한다.
            return;
        CanvasRenderer canvasRenderer = target.GetComponent<CanvasRenderer>();
        if (canvasRenderer != null) {
            callBack(canvasRenderer);
        }
    }

    // 투명도가 100%인 경우만 RaycastTarget으로 설정한다.
    private void setRaycastTarget(Graphic target, ALPHA alpha) => target.raycastTarget = alpha == ALPHA.MAX;

    public void fadeAll(Transform parent, ALPHA alpha, DURATION duration, Dictionary<string, ALPHA> ignoreDic = null, bool isSetRaycastTarget = true, bool ignoreTimescale = false) {
        ALPHA targetAlpha = alpha;
        if (ignoreDic != null) {
            if (ignoreDic.ContainsKey(parent.name))
                targetAlpha = ignoreDic[parent.name];
        }
        checkTargetHasImageOrText(parent, (Graphic graphic) => {
            graphic.CrossFadeAlpha(alphaDic[targetAlpha], durationDic[duration], ignoreTimescale);
            if (isSetRaycastTarget)
                setRaycastTarget(graphic, targetAlpha);
        });
        if (parent.childCount > 0) {
            for (int i = 0; i < parent.childCount; i++) {
                if (parent.GetChild(i).gameObject.activeSelf) {
                    fadeAll(parent.GetChild(i), targetAlpha, duration, ignoreDic, isSetRaycastTarget, ignoreTimescale);
                }
            }
        }
    }

    public void fadeTarget(Graphic target, ALPHA alpha, DURATION duration, bool isSetRayCastTarget = true, bool ignoreTimescale = false) {
        target.CrossFadeAlpha(alphaDic[alpha], durationDic[duration], ignoreTimescale);
        if (isSetRayCastTarget)
            setRaycastTarget(target, alpha);
    }

    public void setAlphaAll(Transform parent, ALPHA alpha, Dictionary<string, ALPHA> ignoreDic = null, bool isSetRaycastTarget = true) {
        ALPHA targetAlpha = alpha;
        if (ignoreDic != null)
            if (ignoreDic.ContainsKey(parent.name))
                targetAlpha = ignoreDic[parent.name];
        setAlphaTarget(parent, targetAlpha, isSetRaycastTarget);
        if (parent.childCount > 0) {
            for (int i = 0; i < parent.childCount; i++) {
                if (parent.GetChild(i).gameObject.activeSelf) {
                    setAlphaAll(parent.GetChild(i), targetAlpha, ignoreDic, isSetRaycastTarget);
                }
            }
        }
    }

    public void setAlphaTarget(Transform target, ALPHA alpha, bool isSetRaycastTarget = true) {
        checkTargetHasCanvasRenderer(target, (CanvasRenderer canvasRenderer) => {
            canvasRenderer.SetAlpha(alphaDic[alpha]);
        });
        if (isSetRaycastTarget)
            checkTargetHasImageOrText(target, (Graphic graphic) => {
                setRaycastTarget(graphic, alpha);
            });
    }

    public void flashTargetAlpha(Graphic target, ALPHA alpha, float duration, int count) {
        StartCoroutine(flashTarget(target, alphaDic[alpha], duration, count));
    }

    private IEnumerator flashTarget(Graphic target, float alpha, float duration, int count) {
        float ownAlpha = target.GetComponent<CanvasRenderer>().GetAlpha();
        Debug.Log(ownAlpha);
        for (int i = 0; i < count; i++) {
            target.CrossFadeAlpha(alpha, duration, false);
            yield return new WaitForSeconds(duration);
            target.CrossFadeAlpha(ownAlpha, duration, false);
            yield return new WaitForSeconds(duration);
        }
    }

    //public void moveFadingObejctAll(RectTransform parent, Vector2 delta, ALPHA alpha, float duration) {
    //    fadeAll(parent, alpha, duration);
    //    StartCoroutine(movingObject(parent, delta, duration));
    //}

    //private IEnumerator movingObject(RectTransform target, Vector2 delta, float duration) {
    //    Vector2 targetPosition = target.anchoredPosition + delta;
    //    Vector2 moveSpeed = Vector2.zero;
    //    while () {
    //        yield return null;
    //        target.anchoredPosition = Vector2.SmoothDamp(target.anchoredPosition, targetPosition, ref moveSpeed, duration);
    //    }
    //}
}
