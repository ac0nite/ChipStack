using System;
using Gameplay;
using Remainders;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animations
{
    [CreateAssetMenu(menuName = "Gameplay/AnimationSettings", fileName = "AnimationSettings", order = 0)]
    public class AnimationSettings : ScriptableObject
    {
        [FoldoutGroup("Group 1")]
        public InitialDropAnimation InitialDrop;
        [FoldoutGroup("Group 2")]
        public DropAnimation Drop;
        [FoldoutGroup("Group 2")]
        public SplitAnimation Split;
        
        public InitialDropAnimationDebug InitialDropDebug;
        public DropAnimationDebug DropDebug;
        public RemainderDebugAnimation RemainderDebug;
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
    public class InitialDropAnimationDebug
    {
        public Vector3 BeginPosition;
        public AnimationComponent.Settings Move;
        [Space]
        public AnimationBase.Settings FlyDown;
        public AnimationBase.Settings FlyTouchDown;
        public AnimationBase.Settings Landing;
    }
    
    [Serializable]
    public class DropAnimationDebug
    {
        public AnimationComponent.Settings Move;
        [Space]
        public AnimationBase.Settings DropHard;
        public AnimationBase.Settings DropMiddle;
        public AnimationBase.Settings DropLight;
        public AnimationBase.Settings Stretching;
    }

    [Serializable]
    public class RemainderDebugAnimation
    {
        public AnimationComponent.Settings Move;
        public AnimationComponent.Settings Down;
    }
    
    [Serializable]
    public class DropAnimation
    {
        [Title("tween move", titleAlignment: TitleAlignments.Centered)]
        public TweenComponent.Settings Move;
        
        [Title("animator", titleAlignment: TitleAlignments.Centered)]
        public AnimationBase.Settings DropMoveAnimation;
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