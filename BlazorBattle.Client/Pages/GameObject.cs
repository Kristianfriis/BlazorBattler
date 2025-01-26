namespace BlazorBattle.Client.Pages;

public class GameObject
{
    public int PosY { get; set; }
    public int PosX { get; set; }
    private int _speed = 0;
    
    public int HitBoxWidth { get; set; }
    public int HitBoxHeight { get; set; }

    private readonly IServiceProvider _serviceProvider;

    public GameObject(int speed, int hitBoxWidth, int hitBoxHeight, IServiceProvider serviceProvider)
    {
        PosY = 0;
        PosX = 0;
        _speed = speed;
        HitBoxHeight = hitBoxHeight;
        HitBoxWidth = hitBoxWidth;
        
        _serviceProvider = serviceProvider;
    }
    
    public void UpdatePosition()
    {
        var inputServiceInstance = _serviceProvider.GetService<IInputService>();
        var gameServiceInstance = _serviceProvider.GetService<IGameService>();
        if (inputServiceInstance != null && gameServiceInstance != null)
        {
            switch (inputServiceInstance.Direction)
            {
                case Direction.Up:
                    if(PosY > 0)
                        PosY -= _speed;
                    break;
                case Direction.Down:
                    if (PosY < (gameServiceInstance.CanvasHeight - HitBoxHeight))
                        PosY += _speed;
                    break;
                case Direction.Left:
                    if(PosX > 0)
                        PosX -= _speed;
                    break;
                case Direction.Right:
                    if(PosX < (gameServiceInstance.CanvasWidth - HitBoxWidth))
                        PosX += _speed;
                    break;
                default:
                    break;
            }
        }
    }
}