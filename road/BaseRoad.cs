using Godot;

public class BaseRoad : Node2D
{
    public enum RoadAreaType {
        PREVIOUS,
        NEXT
    }
    protected class ConnectedRoad
    {
        public BaseRoad road = null;
        public Area2D area = null;
    }

    public BaseRoad Next { get { return _next?.road; } }

    public bool Connected {
        get { return _connected; }
        set {
            if (value != _connected)
                HandleConnectedChanged(value);
            _connected = value;
        }
    }
    [Signal] public delegate void ConnectionsChanged();

    public override void _Ready()
    {
        _previous = null;
        _next = null;
        _connected = false;
        GetNode<Area2D>("Previous").Connect("area_entered", this, nameof(HandleAreaEntered), new Godot.Collections.Array{RoadAreaType.PREVIOUS});
        GetNode<Area2D>("Previous").Connect("area_exited", this, nameof(HandleAreaExited), new Godot.Collections.Array{RoadAreaType.PREVIOUS});
        GetNode<Area2D>("Next").Connect("area_entered", this, nameof(HandleAreaEntered), new Godot.Collections.Array{RoadAreaType.NEXT});
        GetNode<Area2D>("Next").Connect("area_exited", this, nameof(HandleAreaExited), new Godot.Collections.Array{RoadAreaType.NEXT});
    }

    virtual public void HandleAreaEntered(Area2D area, RoadAreaType type)
    {
        Node areaParent = area.GetParent();
        if (type == RoadAreaType.PREVIOUS && areaParent is BaseRoad previousParent)
        {
            _previous = new ConnectedRoad { road = previousParent, area = area };
            EmitSignal(nameof(ConnectionsChanged));
        }
        else if (type == RoadAreaType.NEXT && areaParent is BaseRoad nextParent)
        {
            _next = new ConnectedRoad { road = nextParent, area = area };
            EmitSignal(nameof(ConnectionsChanged));
        }
    }
    virtual public void HandleAreaExited(Area2D area, RoadAreaType type)
    {
        if (type == RoadAreaType.PREVIOUS && _previous.area == area)
        {
            _previous = null;
            EmitSignal(nameof(ConnectionsChanged));
        }
        else if (type == RoadAreaType.NEXT && _next.area == area)
        {
            _next = null;
            EmitSignal(nameof(ConnectionsChanged));
        }
    }

    virtual protected void HandleConnectedChanged(bool newValue) {}

    protected ConnectedRoad _previous;
    protected ConnectedRoad _next;
    protected bool _connected;
}
