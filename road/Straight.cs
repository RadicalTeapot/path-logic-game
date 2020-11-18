using Godot;
using System;

public class Straight : BaseRoad
{
    private static readonly Texture _straightTexture = GD.Load<Texture>("res://assets/objects/road/straight.png");
    private static readonly Texture _deadEndTexture = GD.Load<Texture>("res://assets/objects/road/dead_end.png");
    private void SetSprite()
    {
        var roadSprite = GetNode<Sprite>("RoadPiece");
        roadSprite.RotationDegrees = 0;
        if (_previous != null && _next == null)
        {
            roadSprite.Texture = Straight._deadEndTexture;
        }
        else if (_previous == null && _next != null)
        {
            roadSprite.RotationDegrees = 180;
            roadSprite.Texture = Straight._deadEndTexture;
        }
        else
        {
            roadSprite.Texture = Straight._straightTexture;
        }
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

    public override void HandleAreaEntered(Area2D area, RoadAreaType type)
    {
        base.HandleAreaEntered(area, type);
        // GD.Print($"{Name} entered previous {_previous?.road.Name} next {_next?.road.Name}");
        SetSprite();
    }

    public override void HandleAreaExited(Area2D area, RoadAreaType type)
    {
        base.HandleAreaExited(area, type);
        // GD.Print($"{Name} exited previous {_previous?.road.Name} next {_next?.road.Name}");
        SetSprite();
    }
}
