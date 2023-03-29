using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Camera
    {
        public Matrix Transform { get; private set; }

    public void Follow(Player target, int mapWidth, int mapHeight)
    {
        float cameraPositionX = -target.Position.X + (target.gameScreenWidth / 2) - (target.Texture.Width / 2);
        float cameraPositionY = -target.Position.Y + (target.gameScreenHeight / 2) - (target.Texture.Height / 2);

        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
        Clamp(mapWidth, mapHeight, target.gameScreenWidth, target.gameScreenHeight);
    }

    public void Clamp(int mapWidth, int mapHeight, int screenWidth, int screenHeight)
    {
        float cameraPositionX = Transform.Translation.X;
        float cameraPositionY = Transform.Translation.Y;

        cameraPositionX = MathHelper.Clamp(cameraPositionX, -mapWidth + screenWidth, 0);
        cameraPositionY = MathHelper.Clamp(cameraPositionY, -mapHeight + screenHeight, 0);

        Transform = Matrix.CreateTranslation(cameraPositionX, cameraPositionY, 0);
    }

}

