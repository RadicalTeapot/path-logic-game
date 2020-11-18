using Godot;

public class Corner : BaseRoad
{
    [Signal]
    public delegate void Rotated();

    public override void _Ready()
    {
        GetNode<Area2D>("Click").Connect("input_event", this, nameof(Clicked));
        RotationDegrees = (int)GD.RandRange(0, 4) * 90;
        base._Ready();
    }

    public void Clicked(Node viewport, InputEvent @event, int shapeIDX)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if ((ButtonList)mouseButtonEvent.ButtonIndex == ButtonList.Left && mouseButtonEvent.Pressed)
            {
                RotationDegrees = (RotationDegrees + 90) % 360;
            }
        }
    }

    public override void HandleAreaEntered(Area2D area, RoadAreaType type)
    {
        base.HandleAreaEntered(area, type);
        EmitSignal(nameof(Rotated));
    }

    public override void HandleAreaExited(Area2D area, RoadAreaType type)
    {
        base.HandleAreaExited(area, type);
        EmitSignal(nameof(Rotated));
    }
}
