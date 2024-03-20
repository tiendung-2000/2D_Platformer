using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace SunExternals.Shared
{
    public static class DOTHelper
    {
        //
        public static Tweener SetEaseOrCurve(this Tweener tweener, bool usingEase, Ease ease, AnimationCurve curve)
        {
            if (usingEase) tweener.SetEase(ease);
            else tweener.SetEase(curve);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(int from, int to, float duration, Action<int> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(float from, float to, float duration, Action<float> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(Color from, Color to, float duration, Action<Color> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(Vector2 from, Vector2 to, float duration, Action<Vector2> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(Vector3 from, Vector3 to, float duration, Action<Vector3> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(Quaternion from, Vector3 to, float duration, Action<Quaternion> onUpdate = null, Ease ease = Ease.Linear)
        {
            var tmp = from;
            return DOTween.To(() => tmp, setter => tmp = setter, to, duration)
                .OnUpdate(() => onUpdate?.Invoke(tmp))
                .SetEase(ease);
        }
        
        //
        public static Tweener AddTweener(this GameObject target, int from, int to, float duration, Action<int> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this GameObject target, float from, float to, float duration, Action<float> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this GameObject target, Color from, Color to, float duration, Action<Color> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this GameObject target, Vector2 from, Vector2 to, float duration, Action<Vector2> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this GameObject target, Vector3 from, Vector3 to, float duration, Action<Vector3> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this GameObject target, Quaternion from, Vector3 to, float duration, Action<Quaternion> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Sequence AddSequence(this GameObject target, List<Tween> tweens, Action<bool> animating = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true)
        {
            var sequence = DOTween.Sequence();
            foreach (var tween in tweens) sequence.Append(tween);
            sequence.OnStart(() =>
            {
                if (activeOnStart) target.SetActive(true);
                animating?.Invoke(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.SetActive(false);
                animating?.Invoke(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (sequence.IsBackwards() && activeOnStart && sequence.ElapsedPercentage() <= 0f)
                {
                    target.SetActive(false);
                    animating?.Invoke(false);
                }
            }).OnUpdate(() =>
            {
                if (sequence.IsBackwards() && deactiveOnComplete && sequence.ElapsedPercentage() >= .9825f)
                {
                    target.SetActive(true);
                    animating?.Invoke(true);
                }
            });
            return sequence;
        }
        
        
        
        //
        public static Tweener AddTweener(this Transform target, int from, int to, float duration, Action<int> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this Transform target, float from, float to, float duration, Action<float> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this Transform target, Color from, Color to, float duration, Action<Color> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this Transform target, Vector2 from, Vector2 to, float duration, Action<Vector2> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this Transform target, Vector3 from, Vector3 to, float duration, Action<Vector3> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Tweener AddTweener(this Transform target, Quaternion from, Vector3 to, float duration, Action<Quaternion> onUpdate = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true, Ease ease = Ease.Linear)
        {
            var tmp = from;
            var tweener = DOTween.To(() => tmp, setter => tmp = setter, to, duration);
            tweener.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (tweener.IsBackwards() && activeOnStart && tweener.ElapsedPercentage() <= 0f) target.gameObject.SetActive(false);
            }).OnUpdate(() =>
            {
                onUpdate?.Invoke(tmp);
                if (tweener.IsBackwards() && deactiveOnComplete && tweener.ElapsedPercentage() >= .9825f) target.gameObject.SetActive(true);
            }).SetEase(ease);
            return tweener;
        }
        
        //
        public static Sequence AddSequence(this Transform target, List<Tween> tweens, Action<bool> animating = null, Action onStart = null, Action onComplete = null, bool activeOnStart = true, bool deactiveOnComplete = true)
        {
            var sequence = DOTween.Sequence();
            foreach (var tween in tweens) sequence.Append(tween);
            sequence.OnStart(() =>
            {
                if (activeOnStart) target.gameObject.SetActive(true);
                animating?.Invoke(true);
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                if (deactiveOnComplete) target.gameObject.SetActive(false);
                animating?.Invoke(false);
                onComplete?.Invoke();
            }).OnStepComplete(() =>
            {
                if (sequence.IsBackwards() && activeOnStart && sequence.ElapsedPercentage() <= 0f)
                {
                    target.gameObject.SetActive(false);
                    animating?.Invoke(false);
                }
            }).OnUpdate(() =>
            {
                if (sequence.IsBackwards() && deactiveOnComplete && sequence.ElapsedPercentage() >= .9825f)
                {
                    target.gameObject.SetActive(true);
                    animating?.Invoke(true);
                }
            });
            return sequence;
        }
    }
}