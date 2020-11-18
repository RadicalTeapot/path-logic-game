using Godot;

public class Corner : BaseRoad
{
    public override void _Ready()
    {
        GetNode<Area2D>("Click").Connect("input_event", this, nameof(Clicked));
        RotationDegrees = (int)GD.RandRange(0, 4) * 90;
        base._Ready();
    }

    public async void Clicked(Node viewport, InputEvent @event, int shapeIDX)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if ((ButtonList)mouseButtonEvent.ButtonIndex == ButtonList.Left && mouseButtonEvent.Pressed)
            {
                var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
                animationPlayer.Play("Rotate");
                await ToSignal(animationPlayer, "animation_finished");
                RotationDegrees = (RotationDegrees + 90) % 360;
                GetNode<Sprite>("RoadPiece").RotationDegrees = 0;
            }
        }
    }
}
