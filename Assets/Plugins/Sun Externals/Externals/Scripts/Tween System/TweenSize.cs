using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    public class TweenSize : TweenBase
    {
        #region Variables

        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Vector2 FromValue;
        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Vector2 ToValue;

        private Vector2 _startValue;
        private RectTransform _rectTransform;

        #endregion

        #region Tweens

        protected override void CacheOnAddComponent()
        {
            _rectTransform = transform.GetComponent<RectTransform>();
            TargetType = TargetType.RectTransform;
            if (_rectTransform == null)
            {
                Debug.LogWarning($"No target in {gameObject.name}. Auto destroy tween.");
                DestroyImmediate(this);
            }
        }

        private void CheckCache(Vector2 resetValue)
        {
            _startValue = _rectTransform.sizeDelta;
            if (ResetWhenPlay) _startValue = resetValue;
        }

        private void SetValue(Vector2 value)
        {
            _rectTransform.sizeDelta = value;
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

                MainTween.SetUpdate(true);
                MainTween.Pause();
            }
        }

        public override void ResetToFromValue() => SetValue(FromValue);

        public override void ResetToToValue() => SetValue(ToValue);

        #endregion
    }
}