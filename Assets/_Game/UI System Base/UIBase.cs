using Sirenix.OdinInspector;
using System;
using TweenSystem;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace UISystem
{
    //------------------------------------//
    [Flags]
    public enum UsingTween
    {
        None, Show = 1 << 1, Hide = 1 << 2
    }

    //------------------------------------//
    public abstract class UIBase : MonoBehaviour
    {
        #region Variables

        [field: BoxGroup("Config"), EnumToggleButtons, LabelWidth(80f), SerializeField]
        public UILayer UILayer { get; set; }

        [field: HorizontalGroup("Config/Split", 0.35f), LabelWidth(80f), SerializeField, LabelText("Safe Area")]
        public bool UsingSafeArea { get; set; }

        [field: HorizontalGroup("Config/Split", 0.65f), LabelWidth(80f), SerializeField, Sirenix.OdinInspector.ShowIf("UsingSafeArea")]
        public RectTransform SafePanel { get; set; }

        [field: HorizontalGroup("Config/Split1", 0.35f), LabelWidth(80f), SerializeField]
        public float DelayClick { get; set; }

        [field: HorizontalGroup("Config/Split1", 0.65f), EnumToggleButtons, LabelWidth(80f), SerializeField]
        public UsingTween UsingTween { get; set; }

        [field: HorizontalGroup("Config/Split2"), LabelWidth(80f), SerializeField, Sirenix.OdinInspector.ShowIf("@UsingTweenShow()")]
        public TweenSequence TweenShow { get; set; }

        [field: HorizontalGroup("Config/Split2"), LabelWidth(80f), SerializeField, Sirenix.OdinInspector.ShowIf("@UsingTweenHide()")]
        public TweenSequence TweenHide { get; set; }

        public string UIID { get; set; }
        public bool IsShow { get; set; }
        public bool IsInitialize { get; set; }
        public bool FirstShowing { get; set; }

        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > lastTimeClick + DelayClick;
                if (result) lastTimeClick = Time.unscaledTime;
                return result;
            }
        }

        private float lastTimeClick;
        private RectTransform cacheRect;

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (cacheRect == null) cacheRect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            //cacheRect.SetStretch();
            ApplySafeArea();
        }

        #endregion

        #region Safe area

        private void ApplySafeArea()
        {
            if (UsingSafeArea)
            {
                if (SafePanel == null)
                {
                    Debug.LogWarning($"Check SafePanel in {transform.name}");
                    return;
                }

                var safeArea = Screen.safeArea;
                if (Screen.width > 0 && Screen.height > 0)
                {
                    var anchorMin = safeArea.position;
                    var anchorMax = safeArea.position + safeArea.size;
                    anchorMin.x /= Screen.width;
                    anchorMin.y /= Screen.height;
                    anchorMax.x /= Screen.width;
                    anchorMax.y /= Screen.height;

                    if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                    {
                        SafePanel.anchorMin = anchorMin;
                        SafePanel.anchorMax = anchorMax;
                    }
                }
            }
        }

        #endregion

        #region Functions

        [BoxGroup("Buttons")]
        [Button, HorizontalGroup("Buttons/1", 0.33f)]
        public virtual void Initialize()
        {
            IsInitialize = true;
            FirstShowing = true;
        }

        [Button, HorizontalGroup("Buttons/1", 0.33f)]
        public void Show()
        {
            if (IsShow) return;
            if (UsingTweenShow()) TweenShow.PlayForward();
            else gameObject.SetActive(true);
            ShowInside();
            if (FirstShowing)
            {
                FirstShowing = false;
                ShowFirst();
            }
            else
            {
                ShowNotFirst();
            }

            IsShow = true;
        }

        [Button, HorizontalGroup("Buttons/1", 0.33f)]
        public void Hide()
        {
            if (!IsShow) return;
            HideInside();
            if (UsingTweenHide()) TweenHide.PlayReverse();
            else gameObject.SetActive(false);
            IsShow = false;
        }

        public virtual void ShowInside()
        {
        }

        public virtual void ShowFirst()
        {
        }

        public virtual void ShowNotFirst()
        {
        }

        public virtual void HideInside()
        {
        }

        public virtual void OnBackPressed()
        {
        }

        #endregion

        #region Private functions

        public bool UsingTweenShow()
        {
            return UsingTween.HasFlag(UsingTween.Show);
        }

        public bool UsingTweenHide()
        {
            return UsingTween.HasFlag(UsingTween.Hide);
        }

        #endregion
    }
}