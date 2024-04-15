shader_type canvas_item;

uniform float amount : hint_range(0.0, 1.0) = 0;
uniform float severity : hint_range(0.0, 1.0) = 1;
uniform sampler2D noise;

void fragment() {
    vec4 original = texture(SCREEN_TEXTURE, SCREEN_UV);
    float mult = texture(noise, SCREEN_UV).r;
    float offset = amount * mult;
    float r = texture(SCREEN_TEXTURE, SCREEN_UV + vec2(offset * SCREEN_PIXEL_SIZE.x, 0)).r;
    float g = original.g;
    float b = texture(SCREEN_TEXTURE, SCREEN_UV - vec2(offset * SCREEN_PIXEL_SIZE.x, 0)).b;

    COLOR = severity * vec4(r, g, b, 1) + (1.0 - severity) * original;
}
