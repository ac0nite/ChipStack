using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animations
{
    [CreateAssetMenu(menuName = "Gameplay/AnimationSettings", fileName = "AnimationSettings", order = 0)]
    public class AnimationSettings : ScriptableObject
    {
        public InitialDropSettings InitialDrop;
    }

    [Serializable]
    public class InitialDropSettings
    {
        [Title("tween move", titleAlignment: TitleAlignments.Centered)]
        public Vector3 BeginPosition;
        public TweenComponent.Settings Move;
        
        [Title("animator", titleAlignment: TitleAlignments.Centered)]
        public AnimationBase.Settings DownAnimation;
        public AnimationBase.Settings HitAnimation;
    }
}