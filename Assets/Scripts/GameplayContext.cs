using Blocks;
using CameraComponent;
using ColorComponent;
using FSM;

public class GameplayContext
{
    public StatesMachineModel StatesMachineModel { get; } = new();
    public IGradientModel GradientModel { get; init; }
    public ICameraMover CameraMover { get; init; }
    public IBlockFacade BlockFacade { get; init; }
    public CurrencyManager Currency { get; init; }
}