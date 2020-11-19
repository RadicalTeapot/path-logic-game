using System.Collections.Generic;
using Godot;

public class BaseRoad : Node2D
{
    public List<BaseRoad> ConnectedRoads { get { return _connectedRoads; } }

    public float AnimationDelay;
    [Signal] public delegate void ConnectionsChanged(string signalType, string nodeName, string roadName);

    public override void _Ready()
    {
        _connectedRoads = new List<BaseRoad>();
        _connected = false;
        AnimationDelay = 0;
        GetNode<Area2D>("Connections").Connect("area_entered", this, nameof(HandleAreaEntered));
        GetNode<Area2D>("Connections").Connect("area_exited", this, nameof(HandleAreaExited));
    }

    virtual public void HandleAreaEntered(Area2D area)
    {
        if (area.GetParent() is BaseRoad road && !_connectedRoads.Contains(road))
        {
            _connectedRoads.Add(road);
            EmitSignal(nameof(ConnectionsChanged), "entered", Name, road.Name);
        }
    }
    virtual public void HandleAreaExited(Area2D area)
    {
        if (area.GetParent() is BaseRoad road && _connectedRoads.Contains(road))
        {
            _connectedRoads.Remove(road);
            EmitSignal(nameof(ConnectionsChanged), "exited", Name, road.Name);
        }
    }

    virtual public bool UpdateConnected(bool connected)
    {
        bool changed = _connected != connected;
        _connected = connected;
        return changed;
    }

    protected List<BaseRoad> _connectedRoads;
    protected bool _connected;
}
