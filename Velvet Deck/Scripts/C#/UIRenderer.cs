using Godot;
using System;

public partial class UIRenderer : Node
{
    [Export] public SubViewport subViewport;
    [Export] public Sprite3D sprite3D;

    public override void _Ready()
    {
        // Get the SubViewport’s texture
        Texture2D viewportTexture = subViewport.GetTexture();

        // Assign it to the Sprite3D
        sprite3D.Texture = viewportTexture;

        // Optional: ensure unshaded and transparent if needed
        var mat = new StandardMaterial3D();
        mat.AlbedoTexture = viewportTexture;
        mat.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        mat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
        sprite3D.MaterialOverride = mat;
    }
}
