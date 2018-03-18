using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Lord_Ultimate
{
    struct Constants
    {
        public const float gravity = 9.81f, bounceShockModifier = 1.01f;
        public const int pixelsPerUnit = 1;

        public static float Gravity { get => gravity * pixelsPerUnit; }
    }
}
