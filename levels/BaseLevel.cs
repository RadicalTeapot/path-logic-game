using System.Collections.Generic;
using Godot;

public class BaseLevel : Node2D
{
    public override void _Ready()
    {
        _roadChanged = true;
        var corners = GetNode("Road/Corners").GetChildren();
        foreach (Corner corner in corners)
            corner.Connect(nameof(Corner.Rotated), this, nameof(HandleCornerRotated));
    }

    public void HandleCornerRotated()
    {
        _roadChanged = true;
    }

    public override void _Process(float delta)
    {
        if (_roadChanged)
        {
            BuildRoad();
            _roadChanged = false;
        }
    }

    private void BuildRoad()
    {
        // GD.Print("build road");
        HideStraights();
        var start = GetNode<Marker>("Road/Markers/Start");
        var end = GetNode<Marker>("Road/Markers/End");
        BaseRoad currentRoad = start;
        List<BaseRoad> visited = new List<BaseRoad>();
        while (currentRoad != null && !visited.Contains(currentRoad))
        {
            visited.Add(currentRoad);
            currentRoad.Visible = true;
            currentRoad = currentRoad.Next;

        }
        if (visited.Contains(end))
        {
            GD.Print("Level complete");
        }
        visited.Clear();
    }

    private void HideStraights()
    {
        var straights = GetNode("Road/Straights").GetChildren();
        foreach (Straight straight in straights)
            straight.Visible = false;
    }

    private bool _roadChanged;
}
