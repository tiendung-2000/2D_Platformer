using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    public class TweenPosition : TweenBase
    {
        #region Variables

        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public bool ReverseIncremental;
        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Vector3 FromValue;
        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Vector3 ToValue;

        private Vector3 _startValue;
        private RectTransform _rectTransform;

        #endregion

        #region Unity callback functions

        private void OnValidate()
        {
            if (ReverseIncremental)
            {
                LoopCountForward = 0;
                LoopCountReverse = 0;
                StyleForward = StyleLoop.Once;
                StyleReverse = StyleLoop.Once;
            }
        }

        #endregion

        #region Tweens

        protected override void CacheOnAddComponent()
        {
            _rectTransform = transform.GetComponent<RectTransform>();
            TargetType = _rectTransform != null ? TargetType.RectTransform : TargetType.Transform;
        }

        private void CheckCache(Vector3 resetValue)
        {
            switch (TargetType)
            {
                case TargetType.Transform:
                    _startValue = transform.localPosition;
                    break;
                case TargetType.RectTransform:
                    _startValue = _rectTransform.anchoredPosition;
                    break;
            }

            if (ResetWhenPlay) _startValue = resetValue;
        }

        private void SetValue(Vector3 value)
        {
            switch (TargetType)
            {
                case TargetType.Transform:
                    transform.localPosition = value;
                    break;
                case TargetType.RectTransform:
                    _rectTransform.anchoredPosition = value;
                    break;
            }
        }

        public override void TweenForward()
        {
            CheckCache(FromValue);
            SetTween();
            return;

            void SetTween()
            {
                MainTween?.Kill();
                switch (StyleForward)
                {
                    case StyleLoop.Once:
                        MainTween = TweenHelper.AddTweener(_startValue, ToValue, Duration, SetValue)
                            .OnStart(OnStartForward)
                            .OnComplete(OnCompleteForward)
                            .SetEaseOrCurve(UsingEaseForward, EaseForward, CurveForward)
                            .SetDelay(DelayEvent.HaveOption(Event.Forward) ? DelayTime : 0);
                        break;
                    case StyleLoop.Loop:
                        MainTween = TweenHelper.AddTweener(_startValue, ToValue, Duration, SetValue)
                            .OnStart(OnStartForward)
                            .SetEaseOrCurve(UsingEaseForward, EaseForward, CurveForward)
                            .SetDelay(DelayEvent.HaveOption(Event.Forward) ? DelayTime : 0)
                            .SetLoops(-1, LoopTypeForward);
                        break;
                    case StyleLoop.LoopWithCount:
                        MainTween = TweenHelper.AddTweener(_startValue, ToValue, Duration, SetValue)
                            .OnStart(OnStartForward)
                            .OnComplete(OnCompleteForward)
                            .SetEaseOrCurve(UsingEaseForward, EaseForward, CurveForward)
                            .SetDelay(DelayEvent.HaveOption(Event.Forward) ? DelayTime : 0)
                            .SetLoops(LoopCountForward, LoopTypeForward);
                        break;
                }

                MainTween.SetUpdate(true);
                MainTween.Pause();
            }
        }

        public override void TweenReverse()
        {
            CheckCache(ToValue);
            SetTween();
            return;

            void SetTween()
            {
                var style = SameReverse ? StyleForward : StyleReverse;
                var usingEase = SameReverse ? UsingEaseForward : UsingEaseReverse;
                var curve = SameReverse ? CurveForward : CurveReverse;
                var ease = SameReverse ? EaseForward : EaseReverse;
                var loopType = SameReverse ? LoopTypeForward : LoopTypeReverse;
                var loopCount = SameReverse ? LoopCountForward : LoopCountReverse;

                MainTween?.Kill();
                if (!ReverseIncremental)
                {
                    switch (style)
                    {
                        case StyleLoop.Once:
                            MainTween = TweenHelper.AddTweener(_startValue, FromValue, Duration, SetValue)
                                .OnStart(OnStartReverse)
                                .OnComplete(OnCompleteReverse)
                                .SetEaseOrCurve(usingEase, ease, curve)
                                .SetDelay(DelayEvent.HaveOption(Event.Reverse) ? DelayTime : 0);
                            break;
                        case StyleLoop.Loop:
                            MainTween = TweenHelper.AddTweener(_startValue, FromValue, Duration, SetValue)
                                .OnStart(OnStartReverse)
                                .SetEaseOrCurve(usingEase, ease, curve)
                                .SetDelay(DelayEvent.HaveOption(Event.Reverse) ? DelayTime : 0)
                                .SetLoops(-1, loopType);
                            break;
                        case StyleLoop.LoopWithCount:
                            MainTween = TweenHelper.AddTweener(_startValue, FromValue, Duration, SetValue)
                                .OnStart(OnStartReverse)
                                .OnComplete(OnCompleteReverse)
                                .SetEaseOrCurve(usingEase, ease, curve)
                                .SetDelay(DelayEvent.HaveOption(Event.Reverse) ? DelayTime : 0)
                                .SetLoops(loopCount * 2, loopType);
                            break;
                    }
                }
                else
                {
                    MainTween = TweenHelper.AddTweener(_startValue, _startValue + (ToValue - FromValue), Duration, SetValue)
                        .OnStart(OnStartReverse)
                        .OnComplete(OnCompleteReverse)
                        .SetEaseOrCurve(usingEase, ease, curve)
                        .SetDelay(DelayEvent.HaveOption(Event.Reverse) ? DelayTime : 0);
                }

                MainTween.SetUpdate(true);
                MainTween.Pause();
            }
        }

        public override void ResetToFromValue() => SetValue(FromValue);

        public override void ResetToToValue() => SetValue(ToValue);

        #endregion
    }
}