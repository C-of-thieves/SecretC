﻿using Microsoft.Xna.Framework.Content;
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
    private static Texture2D sandTexture;
    private static Texture2D waterTexture;
    private static Random random;

    public static void Load(ContentManager content)
    {
        playerTexture = content.Load<Texture2D>("Default size/Ships/ship (6)");
        enemyTexture1 = content.Load<Texture2D>("Default size/Ships/ship (1)");
        enemyTexture2 = content.Load<Texture2D>("Default size/Ships/ship (2)");
        enemyTexture3 = content.Load<Texture2D>("Default size/Ships/ship (3)");
        enemyTexture4 = content.Load<Texture2D>("Default size/Ships/ship (4)");
        enemyTexture5 = content.Load<Texture2D>("Default size/Ships/ship (10)");
        sandTexture = content.Load<Texture2D>("Default size/Tiles/tile_18");
        waterTexture = content.Load<Texture2D>("Default size/Tiles/tile_73");
    }

    public static Texture2D GetPlayerTexture()
    {
        return playerTexture;
    }

    public static Texture2D GetSandTexture()
    {
        return sandTexture;
    }

    public static Texture2D GetWaterTexture() { return waterTexture;}

    public static Texture2D GetEnemyTexture()
    {
        random = new Random();
        int num = random.Next(5);

        return num switch
        {
            1 => enemyTexture1,
            2 => enemyTexture2,
            3 => enemyTexture3,
            4 => enemyTexture4,
            5 => enemyTexture5,
            _ => enemyTexture1,
        };
    }
}