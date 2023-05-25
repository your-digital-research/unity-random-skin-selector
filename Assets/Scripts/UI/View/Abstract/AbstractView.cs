using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class AbstractView : MonoBehaviour
{
    #region SERIALIZED_VARIABLES

    [Header("Abstract View References")]
    [SerializeField] protected Image background;
    [SerializeField] protected CanvasGroup canvasGroup;

    #endregion

    #region PUBLIC_FUNCTIONS

    public virtual void Show(bool force = true, float delay = 0, float duration = 1)
    {
        gameObject.SetActive(true);
        background.raycastTarget = false;
        canvasGroup.alpha = 0;

        if (force)
        {
            gameObject.SetActive(true);
            background.raycastTarget = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup
                .DOFade(1, duration)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    background.raycastTarget = true;
                    canvasGroup.alpha = 1;
                });
        }
    }

    public virtual void Hide(bool force = true, float delay = 0, float duration = 1)
    {
        gameObject.SetActive(true);
        background.raycastTarget = false;
        canvasGroup.alpha = 1;

        if (force)
        {
            gameObject.SetActive(false);
            background.raycastTarget = false;
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup
                .DOFade(0, duration)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    background.raycastTarget = false;
                    canvasGroup.alpha = 0;
                });
        }
    }

    #endregion
}