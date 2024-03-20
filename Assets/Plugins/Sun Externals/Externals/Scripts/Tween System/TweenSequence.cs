using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    //------------------------------------//
    public class TweenSequence : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Active - Target")]
        [HorizontalGroup("Active - Target/TargetSplit", width: 0.5f), LabelWidth(100f)]
        public bool Active;
        [HorizontalGroup("Active - Target/TargetSplit", width: 0.5f), LabelWidth(100f)]
        public GameObject Target;

        [ShowIfGroup(path: "Tweener", Condition = "Active")]
        [FoldoutGroup("Tweener/Configs")]
        [HorizontalGroup("Tweener/Configs/ConfigSplit"), LabelWidth(100f)]
        public ActiveTarget ActiveTarget;
        [HorizontalGroup("Tweener/Configs/ConfigSplit1"), LabelWidth(100f)]
        public bool IsAutoplay;
        [HorizontalGroup("Tweener/Configs/ConfigSplit1", width: 0.5f), LabelWidth(100f), Sirenix.OdinInspector.ShowIf("IsAutoplay")]
        public Autoplay Direction;
        [HorizontalGroup("Tweener/Configs/ConfigSplit2"), LabelWidth(100f)]
        public Event DelayEvent;
        [HorizontalGroup("Tweener/Configs/ConfigSplit2", width: 0.5f), LabelWidth(100f), Sirenix.OdinInspector.HideIf(condition: "DelayEvent", optionalValue: Event.None)]
        public float DelayTime;

        [FoldoutGroup("Tweener/Tweener"), LabelText("Tween Append")]
        public List<TweenJoinSequence> AllTweens;

        [FoldoutGroup("Tweener/Events")]
        [HorizontalGroup("Tweener/Events/EventSplit"), LabelWidth(100f)]
        public Event EventStart;
        [HorizontalGroup("Tweener/Events/EventSplit"), LabelWidth(100f)]
        public Event EventFinish;
        [HorizontalGroup("Tweener/Events/EventSplit1"), Sirenix.OdinInspector.HideIf("EventStart", Event.None)]
        public UnityEvent OnStartEvent;
        [HorizontalGroup("Tweener/Events/EventSplit1"), Sirenix.OdinInspector.HideIf("EventFinish", Event.None)]
        public UnityEvent OnFinishEvent;

        public List<Tweener> AllTweenerInSequence { get; set; }
        public bool Animating { get; set; }
        public Sequence MainSequence { get; set; }

        #endregion

        #region Unity callback functions

        private void Reset()
        {
            if (Target == null) Target = gameObject;
        }

        private void OnEnable()
        {
            if (Active && IsAutoplay) Play(Direction == Autoplay.Forward);
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        #region Tween

        public void ResetToFromValue()
        {
            foreach (var tweenJoinSequence in AllTweens)
            {
                foreach (var tween in tweenJoinSequence.TweenBases)
                {
                    tween.ResetToFromValue();
                }
            }
        }

        public void ResetToToValue()
        {
            foreach (var tweenJoinSequence in AllTweens)
            {
                foreach (var tween in tweenJoinSequence.TweenBases)
                {
                    tween.ResetToToValue();
                }
            }
        }

        public void TweenForward()
        {
            if (Target == null) Target = gameObject;
            if (AllTweenerInSequence == null) AllTweenerInSequence = new List<Tweener>();
            foreach (var tweener in AllTweenerInSequence) tweener?.Kill(true);
            AllTweenerInSequence.Clear();
            MainSequence?.Kill();
            MainSequence = DOTween.Sequence();
            foreach (var tween in AllTweens)
            {
                tween.TweenForward();
                AllTweenerInSequence.AddRange(tween.AllTweenerInSequence);
                MainSequence.Append(tween.MainSequence);
            }
            MainSequence.OnStart(OnStartForward).OnComplete(OnCompleteForward).SetDelay(DelayEvent.HaveOption(Event.Forward) ? DelayTime : 0);
            MainSequence.SetUpdate(true);
            MainSequence.Pause();
        }

        public void TweenReverse()
        {
            if (Target == null) Target = gameObject;
            if (AllTweenerInSequence == null) AllTweenerInSequence = new List<Tweener>();
            foreach (var tweener in AllTweenerInSequence) tweener?.Kill(true);
            AllTweenerInSequence.Clear();
            MainSequence?.Kill();
            MainSequence = DOTween.Sequence();
            for (var i = AllTweens.Count - 1; i >= 0; i--)
            {
                AllTweens[i].TweenReverse();
                AllTweenerInSequence.AddRange(AllTweens[i].AllTweenerInSequence);
                MainSequence.Append(AllTweens[i].MainSequence);
            }
            MainSequence.OnStart(OnStartReverse).OnComplete(OnCompleteReverse).SetDelay(DelayEvent.HaveOption(Event.Reverse) ? DelayTime : 0);
            MainSequence.SetUpdate(true);
            MainSequence.Pause();
        }

        [FoldoutGroup("Buttons")]
        [HorizontalGroup("Buttons/ButtonSplit"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayForward()
        {
            if (!Active) return;
            TweenForward();
            MainSequence.Play();
        }

        [HorizontalGroup("Buttons/ButtonSplit"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayReverse()
        {
            if (!Active) return;
            TweenReverse();
            MainSequence.Play();
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
                foreach (var tweener in AllTweenerInSequence) tweener?.Kill(true);
                AllTweenerInSequence.Clear();
                MainSequence.Kill(true);
            }
        }

        [HorizontalGroup("Buttons/ButtonSplit1"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void Pause()
        {
            if (!Active) return;
            if (Animating)
            {
                Animating = false;
                MainSequence.Pause();
            }
        }

        [HorizontalGroup("Buttons/ButtonSplit1"), HideInEditorMode, Button(ButtonSizes.Large)]
        public void PlayContinue()
        {
            if (!Active) return;
            if (MainSequence.IsActive())
            {
                Animating = true;
                MainSequence.Play();
            }
        }

        #endregion

        #region Private functions

        private void OnStartForward()
        {
            if (ActiveTarget.HaveOption(ActiveTarget.ActiveOnStartForward))
                Target.SetActive(true);
            Animating = true;
            if (EventStart.HaveOption(Event.Forward))
                OnStartEvent?.Invoke();
        }

        private void OnCompleteForward()
        {
            if (EventFinish.HaveOption(Event.Forward))
                OnFinishEvent?.Invoke();
            Animating = false;
            if (ActiveTarget.HaveOption(ActiveTarget.DeactiveOnFinishForward))
                Target.SetActive(false);
        }

        private void OnStartReverse()
        {
            if (ActiveTarget.HaveOption(ActiveTarget.ActiveOnStartReverse))
                Target.SetActive(true);
            Animating = true;
            if (EventStart.HaveOption(Event.Reverse))
                OnStartEvent?.Invoke();
        }

        private void OnCompleteReverse()
        {
            if (EventFinish.HaveOption(Event.Reverse))
                OnFinishEvent?.Invoke();
            Animating = false;
            if (ActiveTarget.HaveOption(ActiveTarget.DeactiveOnFinishReverse))
                Target.SetActive(false);
        }

        #endregion
    }

    //------------------------------------//
    [Serializable]
    public class TweenJoinSequence
    {
        #region Variables

        [LabelText("Tween Join")]
        public List<TweenBase> TweenBases = new();
        public List<Tweener> AllTweenerInSequence = new();
        public Sequence MainSequence;

        #endregion

        #region Functions

        public void TweenForward()
        {
            MainSequence = DOTween.Sequence();
            AllTweenerInSequence.Clear();
            foreach (var tweenBase in TweenBases)
            {
                if (tweenBase.StyleForward == StyleLoop.Loop)
                {
                    Debug.LogWarning("Tween loop infinite. Auto skip this tween.");
                    continue;
                }

                tweenBase.TweenForward();
                AllTweenerInSequence.Add(tweenBase.MainTween);
                MainSequence.Join(tweenBase.MainTween);
            }
            MainSequence.Pause();
        }

        public void TweenReverse()
        {
            MainSequence = DOTween.Sequence();
            AllTweenerInSequence.Clear();
            for (var i = TweenBases.Count - 1; i >= 0; i--)
            {
                if (TweenBases[i].StyleReverse == StyleLoop.Loop || (TweenBases[i].SameReverse && TweenBases[i].StyleForward == StyleLoop.Loop))
                {
                    Debug.LogWarning("Tween loop infinite. Auto skip this tween.");
                    continue;
                }

                TweenBases[i].TweenReverse();
                AllTweenerInSequence.Add(TweenBases[i].MainTween);
                MainSequence.Join(TweenBases[i].MainTween);
            }
            MainSequence.Pause();
        }

        #endregion
    }
}