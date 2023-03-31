using Microsoft.Xna.Framework;

public class Camera
{
    public Matrix Transform { get; private set; }

    public void Follow(Player target, int mapWidth, int mapHeight)
    {
        var cameraPositionX = -target.Position.X + target.gameScreenWidth / 2 - target.Texture.Width / 2;
        var cameraPositionY = -target.Position.Y + target.gameScreenHeight / 2 - target.Texture.Height / 2;

        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
        Clamp(mapWidth, mapHeight, target.gameScreenWidth, target.gameScreenHeight);
    }

    public void Clamp(int mapWidth, int mapHeight, int screenWidth, int screenHeight)
    {
        var cameraPositionX = Transform.Translation.X;
        var cameraPositionY = Transform.Translation.Y;

        cameraPositionX = MathHelper.Clamp(cameraPositionX, -mapWidth + screenWidth, 0);
        cameraPositionY = MathHelper.Clamp(cameraPositionY, -mapHeight + screenHeight, 0);

        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
    }
}