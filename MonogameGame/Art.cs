using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

static class Art
{
    private static Texture2D playerTexture;
    private static Texture2D enemyTexture1;
    private static Texture2D enemyTexture2;
    private static Texture2D enemyTexture3;
    private static Texture2D enemyTexture4;
    private static Texture2D enemyTexture5;
    private static Random random;

    public static void Load(ContentManager content)
    {
        playerTexture = content.Load<Texture2D>("Default size/Ships/ship (6)");
        enemyTexture1 = content.Load<Texture2D>("Default size/Ships/ship (1)");
        enemyTexture2 = content.Load<Texture2D>("Default size/Ships/ship (2)");
        enemyTexture3 = content.Load<Texture2D>("Default size/Ships/ship (3)");
        enemyTexture4 = content.Load<Texture2D>("Default size/Ships/ship (4)");
        enemyTexture5 = content.Load<Texture2D>("Default size/Ships/ship (10)");
    }

    public static Texture2D GetPlayerTexture()
    {
        return playerTexture;
    }

    public static Texture2D GetEnemyTexture()
    {
        random = new Random();
        int num = random.Next(5);

        switch (num)
        {
            case 1:
                return enemyTexture1;
            case 2:
                return enemyTexture2;
            case 3:
                return enemyTexture3;
            case 4:
                return enemyTexture4;
            case 5:
                return enemyTexture5;
            default:
                return enemyTexture1;
        }
    }
}