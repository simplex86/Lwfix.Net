using System.Collections.Generic;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.Explosion
{
    /// <summary>This is a comparer used for detecting angle difference between rays</summary>
    internal class RayDataComparer : IComparer<Fixed32>
    {
        int IComparer<Fixed32>.Compare(Fixed32 a, Fixed32 b)
        {
            Fixed32 diff = a - b;
            if (diff > 0)
                return 1;
            if (diff < 0)
                return -1;
            return 0;
        }
    }
}
