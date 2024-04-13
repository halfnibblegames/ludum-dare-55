shader_type canvas_item;

uniform float amount = 0;
uniform float severity = 1;

void fragment() {
    vec4 original = texture(SCREEN_TEXTURE, SCREEN_UV);
    float r = texture(SCREEN_TEXTURE, SCREEN_UV + vec2(amount * SCREEN_PIXEL_SIZE.x, 0)).r;
    float g = original.g;
    float b = texture(SCREEN_TEXTURE, SCREEN_UV - vec2(amount * SCREEN_PIXEL_SIZE.x, 0)).b;
    
    COLOR = severity * vec4(r, g, b, 1) + (1.0 - severity) * original;
}
