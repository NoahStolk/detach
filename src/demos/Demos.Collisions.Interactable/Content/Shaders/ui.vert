layout (location = 0) in vec2 position;
layout (location = 1) in vec2 uv;
layout (location = 2) in vec4 color;

uniform mat4 projectionMatrix;

out vec2 fragUv;
out vec4 fragColor;

void main()
{
	fragUv = uv;
	fragColor = color;
	gl_Position = projectionMatrix * vec4(position.xy, 0, 1);
}
