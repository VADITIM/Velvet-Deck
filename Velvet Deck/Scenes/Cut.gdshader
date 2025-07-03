shader_type canvas_item;

uniform float cut_position : hint_range(0.0, 3.0) = 0.3;
uniform float cut_angle : hint_range(-10.0, 3.0) = 0.5;
uniform float right_offset_x : hint_range(-1.2, 1.2) = 0.05;
uniform float right_offset_y : hint_range(-1.2, 1.2) = -0.03;
uniform float cut_width : hint_range(0.0, 0.5) = 0.01;

void fragment() {
    vec2 uv = UV;
    
    // Calculate the diagonal cut line
    float diagonal_cut = (uv.x - cut_position) * cut_angle + uv.y;
    
    // Determine which side of the cut we're on
    bool is_right_side = uv.x > cut_position && diagonal_cut > 0.0;
    
    // Apply offset to the right side
    if (is_right_side) {
        uv.x -= right_offset_x;
        uv.y -= right_offset_y;
    }
    
    // Sample the texture with the modified UV coordinates
    vec4 tex_color = texture(TEXTURE, uv);
    
    // Create the cut effect by making the cut line transparent
    float cut_mask = 1.0;
    
    // Create a thin transparent line at the cut
    float cut_distance = abs(diagonal_cut);
    if (uv.x > cut_position - cut_width && uv.x < cut_position + cut_width) {
        cut_mask = smoothstep(0.0, cut_width * 0.5, cut_distance);
    }
    
    // Hide pixels that are outside the bounds after offset
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        tex_color.a = 0.0;
    }
    
    COLOR = tex_color;
    COLOR.a *= cut_mask;
}