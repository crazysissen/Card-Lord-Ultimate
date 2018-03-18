using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Card_Lord_Ultimate.Content.Scripts
{
    class PhysicsObject : Physics
    {
        Rectangle rectangle;
        public override Rectangle Rect { get => rectangle; }
    }
}
