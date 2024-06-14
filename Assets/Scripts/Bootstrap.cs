using Blocks;
using CameraComponent;
using ColorComponent;
using Core.FSM.Base;
using Environment;
using FSM;
using FSM.States;
using Settings;
using UI;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EnvironmentKeeper _environmentKeeper;
        
    private StatesMachine _stateMachine;
    private GameplayContext _gameplayContext;

    private void Awake()
    {
        var settings = GameplaySettings.Instance;
        _gameplayContext = new GameplayContext()
        {
            GradientModel = new GradientModel(settings.GradientSettings),
            CameraMover = new CameraMover(_environmentKeeper.CameraComponent, settings.CameraMovementSettings),
            BlockFacade = new BlockFacade(settings.BlockModelSettings),
            Currency = new CurrencyManager(settings.RewardSettings)
        };
            
        ScreenManager.Create(_environmentKeeper.ScreenViewKeeper, _gameplayContext);
            
        _stateMachine = GameplayStateMachine.Create(_gameplayContext);
            
        _gameplayContext.StatesMachineModel.ChangeStateEvent += _stateMachine.NextState; 
    }

    private void Start()
    {
        _gameplayContext.StatesMachineModel.ChangeState<LoadingState>();
    }
}