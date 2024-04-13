shader_type canvas_item;

uniform float amount = 1.0;

void fragment() {
    vec4 bg = texture(SCREEN_TEXTURE, SCREEN_UV);
    float avg = (bg.r + bg.g + bg.b) / 3.0;
    COLOR = amount * vec4(avg, avg, avg, 1.0) + (1.0 - amount) * bg;
}
