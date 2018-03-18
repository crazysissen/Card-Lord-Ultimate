using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Card_Lord_Ultimate.Content.Scripts
{
    class Bullet
    {
        public static List<Bullet> bullets;
        public Vector2 Position
        {
            get; private set;
        }

        Vector2 direction;
        float speed;
        Texture2D sprite;

        Point size = new Point(10, 10);

        public Bullet(Vector2 pos, Vector2 dir, float bulletSpeed, Texture2D bulletSprite)
        {
            bullets.Add(this);
            speed = bulletSpeed;
            direction = dir;
            sprite = bulletSprite;
            Position = pos;
        }

        public void Update()
        {
            Position += direction * speed;
            if (Position.X > Game1.Main.Graphics.PreferredBackBufferWidth || Position.Y > Game1.Main.Graphics.PreferredBackBufferHeight || Position.X < 0 || Position.Y < 0)
            {
                Destroy();
            }
        }

        public void Draw(SpriteBatch sb) => sb.Draw(sprite, new Rectangle(Position.ToPoint(), size), Color.White);

        public void Destroy() => bullets.Remove(this);
    }
}
