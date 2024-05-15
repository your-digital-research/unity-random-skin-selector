using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class SkinView : MonoBehaviour
{
    #region SERIALIZED_VARIABLES

    [Header("References")]
    [SerializeField] private Image starImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image background;
    [SerializeField] private Outline outline;

    [Header("Colors")]
    [SerializeField] private Color outlineLockedColor;
    [SerializeField] private Color outlineUnlockedColor;
    [SerializeField] private Color outlineSelectedColor;

    #endregion

    #region PRIVATE_VARIABLES

    private SkinModel skinModel;

    #endregion

    #region EVENTS

    public UnityAction<int, SkinView> OnSkinClick;
    public UnityAction<int, SkinView> OnSkinDestroy;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void Init(SkinModel model)
    {
        InitModel(model);
        UpdateView(model);
    }

    public void OnPointerClick()
    {
        OnSkinClick?.Invoke(skinModel.Id, this);
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void InitModel(SkinModel model)
    {
        skinModel = model;
        skinModel.OnStateChanged += OnStateChanged;
    }

    private void UpdateView(SkinModel model)
    {
        OnStateChanged(SkinState.Unknown, model.State);
    }

    private void OnStateChanged(SkinState previousState, SkinState newState)
    {
        bool forceLock = previousState == SkinState.Unknown;
        bool forceUnlock = previousState == SkinState.Unknown || previousState == SkinState.Selected;

        switch (newState)
        {
            case SkinState.Unknown:
                Unknown();
                break;
            case SkinState.Locked:
                Lock(forceLock);
                break;
            case SkinState.Unlocked:
                Unlock(forceUnlock);
                break;
            case SkinState.Selected:
                Select();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void Unknown()
    {
        background.raycastTarget = false;
        outline.effectColor = Color.white;
    }

    private void Lock(bool force)
    {
        background.raycastTarget = false;
        lockImage.gameObject.SetActive(true);
        starImage.gameObject.SetActive(false);

        if (force)
        {
            outline.effectColor = outlineLockedColor;
        }
        else
        {
            DOTween.Kill(outline);
            outline.DOColor(outlineLockedColor, 0.25f);
        }
    }

    private async void Unlock(bool force = false)
    {
        DOTween.Kill(background.transform);
        DOTween.Kill(outline);

        StopPulse();

        if (force)
        {
            background.raycastTarget = true;
            outline.effectColor = outlineUnlockedColor;

            starImage.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            outline.DOColor(outlineUnlockedColor, 0.25f);

            await HideLock();
            await ShowStar();

            background.raycastTarget = true;
        }
    }

    private void Select()
    {
        DOTween.Kill(background.transform);
        DOTween.Kill(outline);

        StartPulse();

        background.raycastTarget = false;
        outline.DOColor(outlineSelectedColor, 0.25f);
    }

    private async Task HideLock()
    {
        lockImage.gameObject.SetActive(true);
        lockImage.transform.localScale = Vector3.one;
        lockImage.transform.rotation = Quaternion.identity;

        Sequence hideSequence = DOTween.Sequence();

        Tween scaleTween = lockImage.transform
            .DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .SetDelay(0);

        hideSequence.Append(scaleTween);
        hideSequence.Play();

        await hideSequence.AsyncWaitForCompletion();
    }

    private async Task ShowStar()
    {
        starImage.gameObject.SetActive(true);
        starImage.transform.localScale = Vector3.zero;
        starImage.transform.rotation = Quaternion.identity;

        Sequence showSequence = DOTween.Sequence();

        Tween scaleTween = starImage.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutSine);
        Tween rotateTween = starImage.transform
            .DORotate(Vector3.forward * 360 * 3, 1.0f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack)
            .SetDelay(0);

        showSequence.Append(scaleTween);
        showSequence.Join(rotateTween);
        showSequence.Play();

        await showSequence.AsyncWaitForCompletion();
    }

    private void StartPulse()
    {
        background.transform
            .DOScale(Vector3.one * 1.1f, 0.75f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopPulse()
    {
        background.transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        OnSkinDestroy?.Invoke(skinModel.Id, this);
        skinModel.OnStateChanged -= OnStateChanged;
    }

    #endregion
}