shader_type canvas_item;

uniform sampler2D front_tex;
uniform sampler2D back_tex;
uniform float flip_progress : hint_range(0.0, 1.0);
uniform float direction; // 1.0 = flip right, -1.0 = flip left

void fragment() {
    float angle = flip_progress * 3.14159;

    float scale_x = cos(angle);
    float perspective_shift = sin(angle) * 0.3 * direction;

    // Remap UVs around center
    vec2 uv = UV - vec2(0.5);
    uv.x = uv.x / max(scale_x, 0.001) + perspective_shift;
    uv += vec2(0.5);

    // Blend between front and back
    float blend = smoothstep(0.4, 0.6, flip_progress);
    vec4 front_color = texture(front_tex, uv);
    vec4 back_color = texture(back_tex, uv);
    vec4 color = mix(front_color, back_color, blend);

    // Optional fade for realism
    float fade = abs(cos(angle));
    color.rgb *= fade;

    COLOR = color;
}
