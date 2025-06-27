using Godot;

public partial class UIProjector3D : Node3D
{
    [Export] public SubViewport SubViewport;
    [Export] public Sprite3D Sprite3D;

    public override void _Ready()
    {
        // Load UI scene into SubViewport
        var uiScene = GD.Load<PackedScene>("res://Player1Control.tscn");
        Control uiInstance = uiScene.Instantiate<Control>();
        SubViewport.AddChild(uiInstance);

        // Set texture to Sprite3D
        var texture = SubViewport.GetTexture();
        Sprite3D.Texture = texture;

        // Optional: override material for transparency & no lighting
        var mat = new StandardMaterial3D
        {
            AlbedoTexture = texture,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha
        };
        Sprite3D.MaterialOverride = mat;
    }

    public override void _Process(double delta)
    {
        // Rotate freely on X and Y axis
        Sprite3D.RotateX((float)(delta * 0.5));
        Sprite3D.RotateY((float)(delta * 0.8));
    }
}
