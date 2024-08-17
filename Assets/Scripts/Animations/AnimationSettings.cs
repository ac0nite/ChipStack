using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animations
{
    [CreateAssetMenu(menuName = "Gameplay/AnimationSettings", fileName = "AnimationSettings", order = 0)]
    public class AnimationSettings : ScriptableObject
    {
        public InitialDropAnimation InitialDrop;
        public DropAnimation Drop;
        public SplitAnimation Split;
    }

    [Serializable]
    public class InitialDropAnimation
    {
        [Title("tween move", titleAlignment: TitleAlignments.Centered)]
        public Vector3 BeginPosition;
        public TweenComponent.Settings Move;
        
        [Title("animator", titleAlignment: TitleAlignments.Centered)]
        public AnimationBase.Settings DownAnimation;
        public AnimationBase.Settings HitAnimation;
    }
    
    [Serializable]
    public class DropAnimation
    {
        [Title("tween move", titleAlignment: TitleAlignments.Centered)]
        public TweenComponent.Settings Move;
        
        [Title("animator", titleAlignment: TitleAlignments.Centered)]
        public AnimationBase.Settings TopHitAnimation;
        public AnimationBase.Settings MiddleHitAnimation;
        public AnimationBase.Settings BottomHitAnimation;
    }
    
    [Serializable]
    public class SplitAnimation
    {
        [Title("tween move", titleAlignment: TitleAlignments.Centered)]
        public TweenComponent.Settings Move;
        
        [Title("animator", titleAlignment: TitleAlignments.Centered)]
        public AnimationBase.Settings TopHitAnimation;
        public AnimationBase.Settings MiddleHitAnimation;
        public AnimationBase.Settings BottomHitAnimation;
    }
}