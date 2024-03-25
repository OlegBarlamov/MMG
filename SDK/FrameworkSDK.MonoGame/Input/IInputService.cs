namespace FrameworkSDK.MonoGame.InputManagement
{
    public interface IInputService
    {
        IKeyboardProvider Keyboard { get; }
        
        IMouseProvider Mouse { get; }
        
        IGamePadProvider GamePads { get; }
    }
}