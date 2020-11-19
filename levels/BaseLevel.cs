using System.Collections.Generic;
using Godot;

public class BaseLevel : Node2D
{
	public override void _Ready()
	{
		_roadChanged = true;
		var corners = GetNode("Road/Corners").GetChildren();
		foreach (Corner corner in corners)
			corner.Connect(nameof(Corner.ConnectionsChanged), this, nameof(HandleCornerRotated));

		var children = GetNode("Road/Straights").GetChildren();
		_straights = new List<Straight>();
		foreach (var child in children)
		{
			if (child is Straight straight)
				_straights.Add(straight);
		}

		foreach (var straight in _straights)
			straight.Visible = false;
	}

	public void HandleCornerRotated(string signalType, string nodeName, string roadName)
	{
		GD.Print($"{nodeName} rotated, {roadName} {signalType}");
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
		GD.Print("Build road");
		var start = GetNode<Marker>("Road/Markers/Start");
		var end = GetNode<Marker>("Road/Markers/End");
		BaseRoad currentRoad = start;
		List<BaseRoad> visited = new List<BaseRoad>();
		while (currentRoad != null)
		{
			visited.Add(currentRoad);
			if (currentRoad.ConnectedRoads.Count > 0 && !visited.Contains(currentRoad.ConnectedRoads[0]))
				currentRoad = currentRoad.ConnectedRoads[0];
			else if (currentRoad.ConnectedRoads.Count > 1 && !visited.Contains(currentRoad.ConnectedRoads[1]))
				currentRoad = currentRoad.ConnectedRoads[1];
			else
				currentRoad = null;
		}

		int counter = 0;
		float animDelay = 0.05f;
		// TODO Re-order straight by their location in visited
		foreach (var straight in _straights)
		{
			straight.AnimationDelay = counter * animDelay;
			if (straight.UpdateConnected(visited.Contains(straight)))
				counter++;
		}

		if (visited.Contains(end))
		{
			GD.Print("Level complete");
		}
		visited.Clear();
	}

	private bool _roadChanged;
	private List<Straight> _straights;
}
