using SimplexLab.LwfixPhysics.Jitter2.Collision.Shapes;
using SimplexLab.LwfixPhysics.Jitter2.LinearMath;

namespace SimplexLab.LwfixPhysics.Jitter2Demo;

/*
 * Jitter2 Colosseum Demo
 * This demo logic is ported from BepuPhysics2
 * https://github.com/bepu/bepuphysics2/blob/cfb5daa1837aef30a5437ac347ac583f2ffaf2b0/Demos/Demos/ColosseumDemo.cs
 * Original Copyright Ross Nordby.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

public class Demo28 : IDemo
{
    public string Name => "Colosseum";
    public string Description => "Colosseum of concentric ring walls and platforms to benchmark large scenes.";

    private static void CreateRingWall(World world, JVector position, JVector size, int height, Real radius)
    {
        Real circumference = Real.Two_PI * radius;
        int boxCountPerRing = (int)((Real)0.9 * circumference / size.Z);
        Real increment = Real.Two_PI / boxCountPerRing;
        for (int ringIndex = 0; ringIndex < height; ringIndex++)
        {
            for (int i = 0; i < boxCountPerRing; i++)
            {
                var body = world.CreateRigidBody();
                body.AddShape(new BoxShape(size));

                Real angle = (Real)((ringIndex & 1) == 0 ? i + 0.5 : i) * increment;
                body.Position = position + new JVector(-MathR.Cos(angle) * radius, (Real)(ringIndex + 0.5) * size.Y, MathR.Sin(angle) * radius);
                body.Orientation = JQuaternion.CreateFromAxisAngle(JVector.UnitY, angle);
            }
        }
    }

    private static void CreateRingPlatform(World world, JVector position, JVector size, Real radius)
    {
        Real innerCircumference = Real.Two_PI * (radius - (Real)0.5 * size.Z);
        int boxCount = (int)((Real)0.95 * innerCircumference / size.Y);
        Real increment = Real.Two_PI / boxCount;
        for (int i = 0; i < boxCount; i++)
        {
            Real angle = (Real)i * increment;

            var body = world.CreateRigidBody();
            body.AddShape(new BoxShape(size));

            body.Position = position + new JVector(-MathR.Cos(angle) * radius, (Real)0.5 * size.X, MathR.Sin(angle) * radius);
            body.Orientation = JQuaternion.CreateFromAxisAngle(JVector.UnitY, angle + Real.Half_PI) * JQuaternion.CreateFromAxisAngle(JVector.UnitZ, Real.Half_PI);
        }
    }

    private static JVector CreateRing(World world, JVector position, JVector size, Real radius, int heightPerPlatformLevel, int platformLevels)
    {
        for (int platformIndex = 0; platformIndex < platformLevels; ++platformIndex)
        {
            Real wallOffset = (Real)0.5 * size.Z - (Real)0.5 * size.X;
            CreateRingWall(world, position, size, heightPerPlatformLevel, radius + wallOffset);
            CreateRingWall(world, position, size, heightPerPlatformLevel, radius - wallOffset);

            CreateRingPlatform(world, position + new JVector(0, (Real)(heightPerPlatformLevel * size.Y), 0), size, radius);
            position.Y += (Real)(heightPerPlatformLevel * size.Y + size.X);
        }
        return position;
    }

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        var size = new JVector((Real)0.5, (Real)1, (Real)3);
        var layerPosition = new JVector();
        const int layerCount = 6;
        Real innerRadius = (Real)15;
        int heightPerPlatform = 3;
        int platformsPerLayer = 1;
        Real ringSpacing = (Real)0.5;

        for (int layerIndex = 0; layerIndex < layerCount; ++layerIndex)
        {
            int ringCount = layerCount - layerIndex;
            for (int ringIndex = 0; ringIndex < ringCount; ++ringIndex)
            {
                CreateRing(world, layerPosition, size, innerRadius + (Real)ringIndex * (size.Z + ringSpacing) + (Real)layerIndex * (size.Z - size.X), heightPerPlatform, platformsPerLayer);
            }
            layerPosition.Y += (Real)(platformsPerLayer * (size.Y * heightPerPlatform + size.X));
        }
    }
}
