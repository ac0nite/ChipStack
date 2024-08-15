using Animations;
using Blocks;
using CameraComponent;
using ColorComponent;
using Core.Utils;
using Gameplay;
using UI.Screens;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "Settings/GameplaySettings", order = 0)]
    public class GameplaySettings : SettingsScriptableObject<GameplaySettings>
    {
        public ScreensSettings ScreensSettings;
        public Movement.Settings MovementSettings;
        public CameraHelper.Settings CameraSettings;
        public GradientModel.Settings GradientSettings;
        public CameraMovement.Settings CameraMovementSettings;
        public BlockFacade.Settings BlockModelSettings;
        public PreparingGameplay.Settings PreparingGameplay;
        public CurrencyManager.Settings RewardSettings;
        public RoundHandler.Settings RoundSettings;
        public AnimationSettings AnimationSettings;
    }
}