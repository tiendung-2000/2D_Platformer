using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    //------------------------------------//
    public abstract class TweenBase : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Active - Target")]
        [HorizontalGroup("Active - Target/0", 0.25f), LabelWidth(100)]
        public bool Active;
        [HorizontalGroup("Active - Target/0"), LabelWidth(100), Sirenix.OdinInspector.DisableIf("@true")]
        public TargetType TargetType;

        [ShowIfGroup("Tweener", Condition = "Active")]
        [FoldoutGroup("Tweener/Configs")]
        [HorizontalGroup("Tweener/Configs/0", 0.25f), LabelWidth(100)]
        public bool ResetWhenPlay;
        [HorizontalGroup("Tweener/Configs/0"), LabelWidth(100)]
        public ActiveTarget ActiveTarget;
        [HorizontalGroup("Tweener/Configs/1", 0.25f), LabelWidth(100)]
        public bool IsAutoplay;
        [HorizontalGroup("Tweener/Configs/1"), LabelWidth(100), Sirenix.OdinInspector.EnableIf("IsAutoplay")]
        public Autoplay Direction;
        [HorizontalGroup("Tweener/Configs/2", 0.5f), LabelWidth(100)]
        public Event DelayEvent;
        [HorizontalGroup("Tweener/Configs/2", 0.5f), LabelWidth(100), Sirenix.OdinInspector.EnableIf("@DelayEvent != Event.None")]
        public float DelayTime;

        [FoldoutGroup("Tweener/Tweener")]
        [HorizontalGroup("Tweener/Tweener/0", 0.25f), LabelWidth(100)]
        public bool SameReverse;
        [HorizontalGroup("Tweener/Tweener/0"), LabelWidth(100)]
        public float Duration;

        [BoxGroup("Tweener/Tweener/Forward")]
        [HorizontalGroup("Tweener/Tweener/Forward/1", 0.25f), LabelWidth(96), LabelText("Using Ease")]
        public bool UsingEaseForward;
        [HorizontalGroup("Tweener/Tweener/Forward/1"), LabelWidth(98), LabelText("Ease"), Sirenix.OdinInspector.ShowIf("UsingEaseForward")]
        public Ease EaseForward;
        [HorizontalGroup("Tweener/Tweener/Forward/1"), LabelWidth(98), LabelText("Curve"), Sirenix.OdinInspector.HideIf("UsingEaseForward")]
        public AnimationCurve CurveForward;
        [BoxGroup("Tweener/Tweener/Forward"), LabelWidth(96), LabelText("Style")]
        public StyleLoop StyleForward;
        [HorizontalGroup("Tweener/Tweener/Forward/2"), LabelWidth(96), LabelText("Loop Type"), Sirenix.OdinInspector.HideIf("@StyleForward == StyleLoop.Once")]
        public LoopType LoopTypeForward;
        [HorizontalGroup("Tweener/Tweener/Forward/2"), LabelWidth(96), LabelText("Loop Count"), Sirenix.OdinInspector.ShowIf("@StyleForward == StyleLoop.LoopWithCount")]
        public int LoopCountForward;

        [HideIfGroup("Tweener/Tweener/Reverse", Condition = "SameReverse")]
        [BoxGroup("Tweener/Tweener/Reverse/Reverse")]
        [HorizontalGroup("Tweener/Tweener/Reverse/Reverse/1", 0.25f), LabelWidth(96), LabelText("Using Ease")]
        public bool UsingEaseReverse;
        [HorizontalGroup("Tweener/Tweener/Reverse/Reverse/1"), LabelWidth(98), LabelText("Ease"), Sirenix.OdinInspector.ShowIf("UsingEaseReverse")]
        public Ease EaseReverse;
        [HorizontalGroup("Tweener/Tweener/Reverse/Reverse/1"), LabelWidth(98), LabelText("Curve"), Sirenix.OdinInspector.HideIf("UsingEaseReverse")]
        public AnimationCurve CurveReverse;
        [BoxGroup("Tweener/Tweener/Reverse/Reverse"), LabelWidth(96), LabelText("Style")]
        public StyleLoop StyleReverse;
        [HorizontalGroup("Tweener/Tweener/Reverse/Reverse/2"), LabelWidth(96), LabelText("Loop Type"), Sirenix.OdinInspector.HideIf("@StyleReverse == StyleLoop.Once")]
        public LoopType LoopTypeReverse;
        [HorizontalGroup("Tweener/Tweener/Reverse/Reverse/2"), LabelWidth(96), LabelText("Loop Count"), Sirenix.OdinInspector.ShowIf("@StyleReverse == StyleLoop.LoopWithCount")]
        public int LoopCountReverse;

        [FoldoutGroup("Tweener/Events")]
        [HorizontalGroup("Tweener/Events/1"), LabelWidth(100)]
        public Event EventStart;
        [HorizontalGroup("Tweener/Events/1"), LabelWidth(100)]
        public Event EventFinish;
        [HorizontalGroup("Tweener/Events/2"), Sirenix.OdinInspector.HideIf("@EventStart == Event.None")]
        public UnityEvent OnStartEvent;
        [HorizontalGroup("Tweener/Events/2"), Sirenix.OdinInspector.HideIf("@EventFinish == Event.None")]
        public UnityEvent OnFinishEvent;

        public bool Animating { get; set; }
        public Tweener MainTween { get; set; }

        #endregion

        #region Unity callback functions
        protected virtual void Awake()
        {
            CacheOnAddComponent();
        }

        private void OnEnable()
        {
            if (Active && IsAutoplay) Play(Direction == Autoplay.Forward);
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Reset()
        {
            CacheOnAddComponent();
        }

        #endregion

        #region Functions

        public abstract void TweenForward();
        public abstract void TweenReverse();
        public abstract void ResetToFromValue();
        public abstract void ResetToToValue();

        [FoldoutGroup("Buttons")]
        [HorizontalGroup("Buttons/ButtonSplit"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayForward()
        {
            if (!Active) return;
            TweenForward();
            MainTween.Play();
        }

        [HorizontalGroup("Buttons/ButtonSplit"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayReverse()
        {
            if (!Active) return;
            TweenReverse();
            MainTween.Play();
        }

        public void Play(bool forward = true)
        {
            if (!Active) return;
            if (forward) PlayForward();
            else PlayReverse();
        }

        [HorizontalGroup("Buttons/ButtonSplit1"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void Stop()
        {
            if (!Active) return;
            if (Animating)
            {
                Animating = false;
                MainTween.Kill(true);
            }
        }

        [HorizontalGroup("Buttons/ButtonSplit1"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void Pause()
        {
            if (!Active) return;
            if (Animating)
            {
                Animating = false;
                MainTween.Pause();
            }
        }

        [HorizontalGroup("Buttons/ButtonSplit1"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayContinue()
        {
            if (!Active) return;
            if (MainTween.IsActive())
            {
                Animating = true;
                MainTween.Play();
            }
        }

        #endregion

        #region Protected functions

        protected abstract void CacheOnAddComponent();

        protected void OnStartForward()
        {
            if (ActiveTarget.HaveOption(ActiveTarget.ActiveOnStartForward))
                gameObject.SetActive(true);
            Animating = true;
            if (EventStart.HaveOption(Event.Forward))
                OnStartEvent?.Invoke();
        }

        protected void OnCompleteForward()
        {
            if (EventFinish.HaveOption(Event.Forward))
                OnFinishEvent?.Invoke();
            Animating = false;
            if (ActiveTarget.HaveOption(ActiveTarget.DeactiveOnFinishForward))
                gameObject.SetActive(false);
        }

        protected void OnStartReverse()
        {
            if (ActiveTarget.HaveOption(ActiveTarget.ActiveOnStartReverse))
                gameObject.SetActive(true);
            Animating = true;
            if (EventStart.HaveOption(Event.Reverse))
                OnStartEvent?.Invoke();
        }

        protected void OnCompleteReverse()
        {
            if (EventFinish.HaveOption(Event.Reverse))
                OnFinishEvent?.Invoke();
            Animating = false;
            if (ActiveTarget.HaveOption(ActiveTarget.DeactiveOnFinishReverse))
                gameObject.SetActive(false);
        }

        #endregion
    }

    //------------------------------------//
    public static class TweenHelper
    {
        #region Functions

        public static Tweener SetEaseOrCurve(this Tweener tweener, bool usingEase, Ease ease, AnimationCurve curve)
        {
            if (usingEase) tweener.SetEase(ease);
            else tweener.SetEase(curve);
            return tweener;
        }

        public static Tweener AddTweener(float from, float to, float duration, Action<float> onUpdate = null)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(Ease.Linear);
        }

        public static Tweener AddTweener(Color from, Color to, float duration, Action<Color> onUpdate = null)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(Ease.Linear);
        }

        public static Tweener AddTweener(Vector3 from, Vector3 to, float duration, Action<Vector3> onUpdate = null)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(Ease.Linear);
        }

        public static Tweener AddTweener(Vector2 from, Vector2 to, float duration, Action<Vector2> onUpdate = null)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(Ease.Linear);
        }

        public static Tweener AddTweener(Quaternion from, Vector3 to, float duration, Action<Quaternion> onUpdate = null)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(Ease.Linear);
        }

        #endregion
    }

    //------------------------------------//
    public static class EnumHelper
    {
        #region Functions

        public static bool HaveOption<T>(this T field, T current) where T : Enum
        {
            return field.HasFlag(current);
        }

        #endregion
    }

    //------------------------------------//
    [Flags]
    public enum ActiveTarget
    {
        None,
        ActiveOnStartForward = 1 << 1,
        DeactiveOnFinishForward = 1 << 2,
        ActiveOnStartReverse = 1 << 3,
        DeactiveOnFinishReverse = 1 << 4
    }

    //------------------------------------//
    [Flags]
    public enum Event
    {
        None,
        Forward = 1 << 1,
        Reverse = 1 << 2
    }

    //------------------------------------//
    public enum Autoplay
    {
        Forward,
        Reverse
    }

    //------------------------------------//
    public enum StyleLoop
    {
        Once,
        Loop,
        LoopWithCount
    }

    //------------------------------------//
    public enum TargetType
    {
        MeshRenderer,
        SpriteRenderer,
        CanvasGroup,
        Graphic,
        Transform,
        RectTransform
    }
}