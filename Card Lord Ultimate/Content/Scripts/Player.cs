using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Card_Lord_Ultimate.Content.Scripts
{
    class Player : Physics
    {
        Rectangle playerRect;

        List<string> tags;

        const float jumpForce = 5;

        bool lastFrameJump;

        public override Rectangle Rect { get => playerRect; }

        public override Vector2 Position { get; set; }

        public override List<string> Tags { get => tags; }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) && !lastFrameJump)
            {
                ApplyVelocity(new Vector2(0, -1) * jumpForce);
            }

            lastFrameJump = state.IsKeyDown(Keys.Space);

            playerRect.Location = Position.ToPoint();
        }

        public override void OnCollision(Physics hitPhysics)
        {
            
        }

        public Player(Rectangle rectangle, float physicsMass) : base(physicsMass)
        {
            playerRect = rectangle;
        }
    }
}
