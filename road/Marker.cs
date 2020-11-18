using Godot;

[Tool] public class Marker : BaseRoad
{
    [Export] public bool IsStart
    {
        get { return _isStart; }
        set { _isStart = value; if (_ready) SetSprite(); }
    }

    private static readonly Texture _startTexture = GD.Load<Texture>("res://assets/objects/road/start.png");
    private static readonly Texture _endTexture = GD.Load<Texture>("res://assets/objects/road/end.png");
    public override void _Ready()
    {
        _ready = true;
        SetSprite();
        base._Ready();
    }

    private void SetSprite()
    {
        GetNode<Sprite>("RoadPiece").Texture = _isStart ? Marker._startTexture : Marker._endTexture;
    }

    private bool _ready = false;
    private bool _isStart = true;
}
