using System;

// ReSharper disable CheckNamespace

namespace SunExternals.Shared
{
    public static class EditorHelper
    {
        public static void ActionOnEditorPlaying(Action action)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                action?.Invoke();
            }
#endif
        }
    }
}