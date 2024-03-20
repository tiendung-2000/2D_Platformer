using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    public class TweenColor : TweenBase
    {
        #region Variables

        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Color FromValue;
        [BoxGroup("Tweener/From - To"), LabelWidth(100f)]
        public Color ToValue;

        private Color _startValue;
        private RectTransform _rectTransform;
        private MeshRenderer _meshRenderer;
        private SpriteRenderer _spriteRenderer;
        private Graphic _graphic;
        private MaterialPropertyBlock _cachePropertyBlock;
        private static readonly int PrefsColor = Shader.PropertyToID("_Color");

        #endregion

        #region Tweens

        protected override void CacheOnAddComponent()
        {
            _rectTransform = transform.GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                _meshRenderer = transform.GetComponent<MeshRenderer>();
                if (_meshRenderer != null) TargetType = TargetType.MeshRenderer;
                else
                {
                    _spriteRenderer = transform.GetComponent<SpriteRenderer>();
                    if (_spriteRenderer != null) TargetType = TargetType.SpriteRenderer;
                    else
                    {
                        Debug.LogWarning($"No target in {gameObject.name}. Auto destroy tween.");
                        DestroyImmediate(this);
                    }
                }
            }
            else
            {
                _graphic = transform.GetComponent<Graphic>();
                if (_graphic != null) TargetType = TargetType.Graphic;
                else
                {
                    Debug.LogWarning($"No target in {gameObject.name}. Auto destroy tween.");
                    DestroyImmediate(this);
                }
            }
        }

        private void CheckCache(Color resetValue)
        {
            switch (TargetType)
            {
                case TargetType.MeshRenderer:
                    if (_meshRenderer == null) _meshRenderer = transform.GetComponent<MeshRenderer>();
                    if (_cachePropertyBlock == null) _cachePropertyBlock = new MaterialPropertyBlock();
                    _meshRenderer.GetPropertyBlock(_cachePropertyBlock);
                    _startValue = _cachePropertyBlock.GetColor(PrefsColor);
                    break;
                case TargetType.SpriteRenderer:
                    if (_spriteRenderer == null) _spriteRenderer = transform.GetComponent<SpriteRenderer>();
                    _startValue = _spriteRenderer.color;
                    break;
                case TargetType.Graphic:
                    if (_graphic == null) _graphic = transform.GetComponent<Graphic>();
                    _startValue = _graphic.color;
                    break;
            }

            if (ResetWhenPlay) _startValue = resetValue;
        }

        private void SetValue(Color value)
        {
            switch (TargetType)
            {
                case TargetType.MeshRenderer:
                    _cachePropertyBlock.SetColor(PrefsColor, value);
                    _meshRenderer.SetPropertyBlock(_cachePropertyBlock);
                    break;
                case TargetType.SpriteRenderer:
                    _spriteRenderer.color = value;
                    break;
                case TargetType.Graphic:
                    _graphic.color = value;
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