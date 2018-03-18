using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Card_Lord_Ultimate.Content.Scripts;

namespace Card_Lord_Ultimate
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Main
        {
            get; private set;
        }

        public GraphicsDeviceManager Graphics
        {
            get; private set;
        }

        SpriteBatch spriteBatch;
        Texture2D card, bullet;
        Player player;
        Rectangle CardRectangle { get => player.Rect; }
        Rectangle GroundRectangle { get =>  }
        Vector2 moveDirection = new Vector2(0, 1), currentPosition;

        const float speed = 2, bulletSpeed = 10, minShootRadius = 30;
        const int sizeModifier = 2;

        bool leftMousePressed;

        public Game1()
        {
            Main = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            Physics.CreateList();
            player = new Player(new Rectangle(0, 0, card.Bounds.Width * sizeModifier, card.Bounds.Height * sizeModifier), 10);
            Bullet.bullets = new List<Bullet>();
            Mouse.SetCursor(MouseCursor.Arrow);
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            card = Content.Load<Texture2D>("Strike Card");
            bullet = Content.Load<Texture2D>("Health Icon");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Keys[] movementKeys = new Keys[] { Keys.D, Keys.S, Keys.A, Keys.W };
            Vector2[] movementVectors = new Vector2[] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
            Vector2 movementVector = Vector2.Zero;

            for (int i = 0; i < movementKeys.Length; i++)
            {
                if (Keyboard.GetState().IsKeyDown(movementKeys[i]))
                    movementVector += movementVectors[i];
            }

            if (movementVector != Vector2.Zero)
            {
                movementVector.Normalize();
                player.AccelerateTowards(movementVector, 1, gameTime);
            }


            MouseState state = Mouse.GetState();
            Vector2 direction = (state.Position - (CardRectangle.Location + (CardRectangle.Size.ToVector2() * 0.5f).ToPoint())).ToVector2();

            if (state.LeftButton == ButtonState.Pressed && !leftMousePressed && direction.Length() > minShootRadius)
            {
                direction.Normalize();
                new Bullet((CardRectangle.Location + (CardRectangle.Size.ToVector2() * 0.5f).ToPoint()).ToVector2(), direction, bulletSpeed, bullet);
            }

            //if (direction != vector2.zero)
            //{
            //    direction.normalize();
            //    currentposition += direction * speed * (gametime.elapsedgametime.milliseconds * 0.1f);
            //    cardrectangle.location = currentposition.topoint();
            //}

            for (int i = 0; i < Bullet.bullets.Count; i++)
            {
                Bullet.bullets[i].Update();
            }

            leftMousePressed = state.LeftButton == ButtonState.Pressed;

            bool[,] calculated = new bool[Physics.PhysList.Count, Physics.PhysList.Count];
            for (int i = 0; i < Physics.PhysList.Count; i++)
            {
                Physics.PhysList[i].UpdatePhysics(gameTime);
                
                for (int j = 0; j < Physics.PhysList.Count; j++)
                {
                    if (j != i && !calculated[i, j])
                    {
                        if (Physics.PhysList[i].Rect.Intersects(Physics.PhysList[j].Rect))
                        {
                            Physics.PhysList[i].Impact(Physics.PhysList[j]);
                            calculated[i, j] = true;
                        }
                    }
                }
            }

            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(card, CardRectangle, Color.White);
            for (int i = 0; i < Bullet.bullets.Count; i++)
            {
                Bullet.bullets[i].Draw(spriteBatch);
            } 
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
