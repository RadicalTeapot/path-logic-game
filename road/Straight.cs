using Godot;
using System;

public class Straight : BaseRoad
{
    private static readonly Texture _straightTexture = GD.Load<Texture>("res://assets/objects/road/straight.png");
    private static readonly Texture _deadEndTexture = GD.Load<Texture>("res://assets/objects/road/dead_end.png");
    private void SetSprite(string signalType, string nodeName, string roadName)
    {
        GD.Print($"Set sprite {roadName} {signalType} {Name}");
        var roadSprite = GetNode<Sprite>("RoadPiece");
        if (_connectedRoads.Count != 2)
            roadSprite.Texture = Straight._deadEndTexture;
        else
            roadSprite.Texture = Straight._straightTexture;
    }

    public override void _Ready()
    {
        base._Ready();
        Connect(nameof(ConnectionsChanged), this, nameof(SetSprite));
    }

    public override bool UpdateConnected(bool connected)
    {
        bool updated = base.UpdateConnected(connected);
        if (updated)
            Animate(connected);
        return updated;
    }

    private async void Animate(bool showRoad)
    {
        if (AnimationDelay > 0)
        {
            var timer = GetNode<Timer>("Timer");
            timer.Start(AnimationDelay);
            await ToSignal(timer, "timeout");
        }

        if (showRoad)
        {
            var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            animationPlayer.Play("Appear");
            animationPlayer.Advance(0);
            Visible = true;
        }
        else
        {
            var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            animationPlayer.Play("Disappear");
            await ToSignal(animationPlayer, "animation_finished");
            Visible = false;
        }
    }
}
