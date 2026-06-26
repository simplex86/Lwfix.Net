using System;
using System.Text;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using ImGui = ImGuiNET.ImGui;

namespace SimplexLab.Fixed.Physics.JDemo;

public partial class Playground
{
    private const string GlobalControls =
        "[Controls]\n" +
        "WASD - Move camera\n" +
        "Right Mouse (hold) - Rotate camera\n" +
        "Left Mouse (hold) - Grab object\n" +
        "Scroll Wheel - Adjust grab distance\n" +
        "Space - Shoot cube\n" +
        "M - Toggle multi-threading";

    private readonly double[] debugTimes = new double[(int)World.Timings.Last];
    private readonly StringBuilder gcText = new();
    private readonly float[] physicsTime = new float[100];

    private double totalTime;
    private int samplingRate = 5;
    private int accSteps;
    private double lastTime;
    private ushort frameCount;
    private ushort fps = 100;

    private bool objectsSectionOpen = true;
    private bool optionsSectionOpen = true;
    private bool debugDrawSectionOpen;
    private bool broadphaseSectionOpen;
    private bool timingsSectionOpen = true;
    private bool gcSectionOpen = true;

    protected override void DrawCustomOverlay()
    {
        BuildDemoOverlay();
    }

    private void UpdateDisplayText()
    {
        if (Time - lastTime > 1.0d)
        {
            lastTime = Time;
            fps = frameCount;
            frameCount = 0;
        }

        frameCount++;

        accSteps += 1;
        if (accSteps < samplingRate) return;

        accSteps = 0;

        gcText.Clear();

        World.DebugTimings.CopyTo(debugTimes);
        totalTime = 0;
        for (int i = 0; i < debugTimes.Length; i++) totalTime += debugTimes[i];

        for (int i = physicsTime.Length; i-- > 1;)
            physicsTime[i] = physicsTime[i - 1];

        physicsTime[0] = (float)totalTime;

        gcText.Append("gen0: ").Append(GC.CollectionCount(0))
              .Append("; gen1: ").Append(GC.CollectionCount(1))
              .Append("; gen2: ").AppendLine(GC.CollectionCount(2).ToString());
        gcText.Append("pause total: ").Append(GC.GetTotalPauseDuration().TotalSeconds).AppendLine(" s");
    }

    private void BuildDemoOverlay()
    {
        // Set ImGui window position to top-left.
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(16, 16), ImGuiNET.ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(280, 0), ImGuiNET.ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Jitter2 Demo", ImGuiNET.ImGuiWindowFlags.NoResize))
        {
            ImGui.End();
            return;
        }

        ImGui.TextColored(new System.Numerics.Vector4(1.0f, 0.78f, 0.35f, 1.0f), $"{fps} fps");
        ImGui.Separator();

        // Demo selector
        ImGui.Text("Demo");
        for (int i = 0; i < Demos.Count; i++)
        {
            var demo = Demos[i];
            bool selected = i == SelectedDemoIndex;
            if (ImGui.Selectable($"Demo {i:00} - {demo.Name}", selected))
            {
                SelectDemo(i);
            }
            if (ImGui.IsItemHovered() && !string.IsNullOrEmpty(demo.Description))
            {
                ImGui.SetTooltip(demo.Description);
            }
        }

        ImGui.Separator();

        if (ImGui.CollapsingHeader("Objects", ref objectsSectionOpen))
        {
            World.SpanData data = World.RawData;
            ImGui.Text($"Islands:  {World.Islands.Count}/{World.Islands.ActiveCount}");
            ImGui.Text($"Bodies:   {data.RigidBodies.Length}/{data.ActiveRigidBodies.Length}");
            ImGui.Text($"Arbiter:  {data.Contacts.Length}/{data.ActiveContacts.Length}");
            ImGui.Text($"Constr:   {data.Constraints.Length}/{data.ActiveConstraints.Length}");
            ImGui.Text($"SmallC:   {data.SmallConstraints.Length}/{data.ActiveSmallConstraints.Length}");
            ImGui.Text($"Proxies:  {World.DynamicTree.Proxies.Count}/{World.DynamicTree.Proxies.ActiveCount}");
        }

        if (ImGui.CollapsingHeader("Options", ref optionsSectionOpen))
        {
            bool allowDeactivation = World.AllowDeactivation;
            if (ImGui.Checkbox("Allow Deactivation", ref allowDeactivation))
                World.AllowDeactivation = allowDeactivation;

            bool auxiliaryContacts = World.EnableAuxiliaryContactPoints;
            if (ImGui.Checkbox("Auxiliary Flat Surface", ref auxiliaryContacts))
                World.EnableAuxiliaryContactPoints = auxiliaryContacts;

            ImGui.Checkbox("Multithreading", ref multiThread);
        }

        if (ImGui.CollapsingHeader("Debug Draw", ref debugDrawSectionOpen))
        {
            ImGui.Checkbox("Islands", ref debugDrawIslands);
            ImGui.Checkbox("Contacts", ref debugDrawContacts);
            ImGui.Checkbox("Shapes", ref debugDrawShapes);
            ImGui.Checkbox("Tree", ref debugDrawTree);
            ImGui.SliderInt("Tree Depth", ref debugDrawTreeDepth, 1, 12);
        }

        if (ImGui.CollapsingHeader("Broadphase", ref broadphaseSectionOpen))
        {
            ImGui.Text($"Proxies: {World.DynamicTree.Proxies.Count}");
            ImGui.Text($"Active:  {World.DynamicTree.Proxies.ActiveCount}");
            ImGui.Text($"Updated: {World.DynamicTree.UpdatedProxyCount}");
        }

        if (ImGui.CollapsingHeader("Timings", ref timingsSectionOpen))
        {
            ImGui.Text($"Total: {totalTime * 1000.0:F2} ms");
            ImGui.PlotLines("##PhysicsTime", ref physicsTime[0], physicsTime.Length);

            for (int i = 0; i < debugTimes.Length; i++)
            {
                string name = ((World.Timings)i).ToString();
                ImGui.Text($"{name,-20}{debugTimes[i] * 1000.0:F3} ms");
            }
        }

        if (ImGui.CollapsingHeader("GC", ref gcSectionOpen))
        {
            ImGui.TextUnformatted(gcText.ToString());
        }

        if (ImGui.CollapsingHeader("Controls"))
        {
            ImGui.TextUnformatted(GlobalControls);
        }

        if (SelectedDemoIndex >= 0 && SelectedDemoIndex < Demos.Count)
        {
            var demo = Demos[SelectedDemoIndex];
            if (!string.IsNullOrEmpty(demo.Controls))
            {
                ImGui.Separator();
                ImGui.TextUnformatted(demo.Controls);
            }
        }

        ImGui.End();
    }
}
