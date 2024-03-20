using System;

// ReSharper disable CheckNamespace

namespace SunExternals.Externals.TweenSystem
{
    //
    [Flags] public enum ActiveTarget
    {
        None,
        ActiveOnStartForward = 1 << 1,
        DeactiveOnFinishForward = 1 << 2,
        ActiveOnStartReverse = 1 << 3,
        DeactiveOnFinishReverse = 1 << 4
    }
    
    //
    [Flags] public enum Event
    {
        None,
        Forward = 1 << 1,
        Reverse = 1 << 2
    }
    
    //
    public enum Autoplay
    {
        Forward,
        Reverse
    }
    
    //
    public enum StyleLoop 
    { 
        Once, 
        Loop, 
        LoopWithCount 
    }
    
    //
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