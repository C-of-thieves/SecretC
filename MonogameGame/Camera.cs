using Microsoft.Xna.Framework;

public class Camera
{
    public Matrix Transform { get; private set; }
    public int gameScreenWidth  { get; private set; } 
    public int gameScreenHeight { get; private set; }

    
    public Camera()
    {
        gameScreenWidth= 1800;
        gameScreenHeight= 1200;
    }
    public void Follow(Player target, int mapWidth, int mapHeight)
    {
        float cameraPositionX = -target.Position.X + (gameScreenWidth / 2) - (target.Texture.Width / 2);
        float cameraPositionY = -target.Position.Y + (gameScreenHeight / 2) - (target.Texture.Height / 2);
        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
        Clamp(mapWidth, mapHeight, gameScreenWidth, gameScreenHeight);
    }

    public void Clamp(int mapWidth, int mapHeight, int screenWidth, int screenHeight)
    {
        float cameraPositionX = Transform.Translation.X;
        float cameraPositionY = Transform.Translation.Y;

        cameraPositionX = MathHelper.Clamp(cameraPositionX, -mapWidth + screenWidth, 0);
        cameraPositionY = MathHelper.Clamp(cameraPositionY, -mapHeight + screenHeight, 0);

        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
    }

    public void UpdateResolution(int newWidth, int newHeight)
    {
        gameScreenWidth = newWidth;
        gameScreenHeight = newHeight;
    }

}

