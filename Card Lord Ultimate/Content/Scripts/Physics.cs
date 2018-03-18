using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Card_Lord_Ultimate.Content.Scripts
{
    /// <summary>
    /// Main class for physics and collison calculation
    /// </summary>
    abstract class Physics
    {
        public static List<Physics> PhysList { get; private set; }

        /// <summary>
        /// Returns the force carried by this object
        /// </summary> <value>No</value>
        public float Force { get => /*Mass * Velocity.Length();*/ (float)Math.Pow((float)Velocity.Length(), 2) * (Mass * 0.5f); }

        public bool Static { get; private set; }

        public float Mass { get; private set; }

        /// <summary>
        /// The current velocity of the object measured in Constant.unit/s
        /// </summary>
        Vector2 vel;
        public Vector2 Velocity
        {
            get => Static ? Vector2.Zero : vel;

            private set => vel = value;
        }

        public abstract Rectangle Rect { get; } 

        public abstract Vector2 Position { get; set; }
        
        public abstract List<string> Tags { get; }

        public void UpdatePhysics(GameTime gameTime)
        {
            if (!Static)
            {
                Velocity += new Vector2(0, Constants.Gravity * ((float)gameTime.ElapsedGameTime.Milliseconds * 0.001f));
                Position += Velocity * Constants.pixelsPerUnit;
            }
        }

        public void ApplyVelocity(Vector2 velocity) => Velocity +=  velocity;

        public void SetVelocity(Vector2 velocity) => Velocity = velocity;

        public void AccelerateTowards(Vector2 targetVelocity, float acceleration, GameTime gameTime)
        {
            Vector2 difference = targetVelocity - Velocity;
            if (difference.Length() < gameTime.ElapsedGameTime.Milliseconds * 0.001f * acceleration)
            {
                difference.Normalize();
                difference *= gameTime.ElapsedGameTime.Milliseconds * 0.001f * acceleration;
                Velocity += difference;
            }
        }

        public void AccelerateX(float targetVelocity, float acceleration, GameTime gameTime)
        {
            if (targetVelocity - Velocity.X < gameTime.ElapsedGameTime.Milliseconds * 0.001f * acceleration)
            {

            }
        }

        public void Impact(Physics physics)
        {
            if (Static || physics.Static)
            {
                SetVelocity(Vector2.Zero);
                physics.SetVelocity(Vector2.Zero);
                return;
            }

            float thisForce = Force;
            Vector2 thisVelocity = Velocity;

            // Old solution
            //ApplyVelocity(physics.Velocity * (physics.Force / Force) * Constants.bounceShockModifier);
            //physics.ApplyVelocity(thisVelocity * (thisForce / physics.Force) * Constants.bounceShockModifier);

            // New solution
            ApplyVelocity(Normalized(physics.Velocity) * physics.Force * Constants.bounceShockModifier);
            physics.ApplyVelocity(Normalized(thisVelocity) * thisForce * Constants.bounceShockModifier);

            OnCollision(physics);
            physics.OnCollision(this);
        }

        public virtual void OnCollision(Physics hitPhysics) { }

        public void DestroyPhysics() => PhysList.Remove(this);

        /// <summary>
        /// Constructs a static object
        /// </summary>
        public Physics()
        {
            vel = Vector2.Zero;
            Mass = 0;
            Static = true;
            PhysList.Add(this);
        }

        public Physics(float mass)
        {
            vel = Vector2.Zero;
            Mass = mass;
            Static = false;
            PhysList.Add(this);
        }

        public Physics(Vector2 initialVelocity, float mass)
        {
            vel = initialVelocity;
            Mass = mass;
            Static = false;
            PhysList.Add(this);
        }

        public static void CreateList() => PhysList = new List<Physics>();

        public static Vector2 Normalized(Vector2 vector)
        {
            Vector2 newVector = vector;
            newVector.Normalize();
            return newVector;
        }
    }
}
