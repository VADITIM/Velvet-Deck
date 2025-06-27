shader_type canvas_item;

uniform vec2 pivot = vec2(0.5, 0.5); // rotation center

void fragment() {
    vec2 uv = UV - pivot;

    // Degrees to radians manually
    float deg_to_rad = 0.0174533;
    float angle_x = -10.0 * deg_to_rad;
    float angle_y = 30.0 * deg_to_rad;

    float sx = tan(angle_y);
    float sy = tan(angle_x);

    // Explicitly cast to float (avoids type issues)
    float a = 1.0;
    float b = sy;
    float c = sx;
    float d = 1.0;

    mat2 skew = mat2(vec2(a, b), vec2(c, d));

    uv = skew * uv;
    uv += pivot;

    // Discard anything out of bounds
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
        discard;
    }

    COLOR = texture(TEXTURE, uv) * texture(TEXTURE, UV);
}
