using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable CheckNamespace

namespace TweenSystem
{
    public class TweenAlpha : TweenBase
    {
        #region Variables

        [BoxGroup("Tweener/From - To"), LabelWidth(100f), PropertyRange(0f, 1f)]
        public float FromValue;
        [BoxGroup("Tweener/From - To"), LabelWidth(100f), PropertyRange(0f, 1f)]
        public float ToValue;

        private float _startValue;
        private RectTransform _rectTransform;
        private MeshRenderer _meshRenderer;
        private SpriteRenderer _spriteRenderer;
        private CanvasGroup _canvasGroup;
        private Graphic _graphic;
        private Color _cacheColor;
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
                _canvasGroup = transform.GetComponent<CanvasGroup>();
                if (_canvasGroup != null) TargetType = TargetType.CanvasGroup;
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
        }

        private void CheckCache(float resetValue)
        {
            switch (TargetType)
            {
                case TargetType.MeshRenderer:
                    if (_meshRenderer == null) _meshRenderer = transform.GetComponent<MeshRenderer>();
                    if (_cachePropertyBlock == null) _cachePropertyBlock = new MaterialPropertyBlock();
                    _meshRenderer.GetPropertyBlock(_cachePropertyBlock);
                    _cacheColor = _cachePropertyBlock.GetColor(PrefsColor);
                    _startValue = _cacheColor.a;
                    break;
                case TargetType.SpriteRenderer:
                    if (_spriteRenderer == null) _spriteRenderer = transform.GetComponent<SpriteRenderer>();
                    _cacheColor = _spriteRenderer.color;
                    _startValue = _cacheColor.a;
                    break;
                case TargetType.CanvasGroup:
                    if (_canvasGroup == null) _canvasGroup = transform.GetComponent<CanvasGroup>();
                    _startValue = _canvasGroup.alpha;
                    break;
                case TargetType.Graphic:
                    if (_graphic == null) _graphic = transform.GetComponent<Graphic>();
                    _cacheColor = _graphic.color;
                    _startValue = _cacheColor.a;
                    break;
            }

            if (ResetWhenPlay) _startValue = resetValue;
        }

        private void SetValue(float value)
        {
            switch (TargetType)
            {
                case TargetType.MeshRenderer:
                    _cacheColor.a = value;
                    _cachePropertyBlock.SetColor(PrefsColor, _cacheColor);
                    _meshRenderer.SetPropertyBlock(_cachePropertyBlock);
                    break;
                case TargetType.SpriteRenderer:
                    _cacheColor.a = value;
                    _spriteRenderer.color = _cacheColor;
                    break;
                case TargetType.CanvasGroup:
                    _canvasGroup.alpha = value;
                    break;
                case TargetType.Graphic:
                    _cacheColor.a = value;
                    _graphic.color = _cacheColor;
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