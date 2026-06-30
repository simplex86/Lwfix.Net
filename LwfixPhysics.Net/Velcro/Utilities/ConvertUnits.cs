/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
*/

using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Utilities
{
    /// <summary>Convert units between display and simulation units.</summary>
    public static class ConvertUnits
    {
        private static Fixed32 _displayUnitsToSimUnitsRatio = (Fixed32)100.0;
        private static Fixed32 _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

        public static void SetDisplayUnitToSimUnitRatio(Fixed32 displayUnitsPerSimUnit)
        {
            _displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            _simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        public static Fixed32 ToDisplayUnits(Fixed32 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Fixed32 ToDisplayUnits(int simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToDisplayUnits(Vector2 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static void ToDisplayUnits(ref Vector2 simUnits, out Vector2 displayUnits)
        {
            Vector2.Multiply(ref simUnits, _displayUnitsToSimUnitsRatio, out displayUnits);
        }

        public static Vector3 ToDisplayUnits(Vector3 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToDisplayUnits(Fixed32 x, Fixed32 y)
        {
            return new Vector2(x, y) * _displayUnitsToSimUnitsRatio;
        }

        public static void ToDisplayUnits(Fixed32 x, Fixed32 y, out Vector2 displayUnits)
        {
            displayUnits = Vector2.Zero;
            displayUnits.X = x * _displayUnitsToSimUnitsRatio;
            displayUnits.Y = y * _displayUnitsToSimUnitsRatio;
        }

        public static Fixed32 ToSimUnits(Fixed32 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static Fixed32 ToSimUnits(int displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static Vector2 ToSimUnits(Vector2 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static Vector3 ToSimUnits(Vector3 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static void ToSimUnits(ref Vector2 displayUnits, out Vector2 simUnits)
        {
            Vector2.Multiply(ref displayUnits, _simUnitsToDisplayUnitsRatio, out simUnits);
        }

        public static Vector2 ToSimUnits(Fixed32 x, Fixed32 y)
        {
            return new Vector2(x, y) * _simUnitsToDisplayUnitsRatio;
        }

        public static void ToSimUnits(Fixed32 x, Fixed32 y, out Vector2 simUnits)
        {
            simUnits = Vector2.Zero;
            simUnits.X = x * _simUnitsToDisplayUnitsRatio;
            simUnits.Y = y * _simUnitsToDisplayUnitsRatio;
        }
    }
}
