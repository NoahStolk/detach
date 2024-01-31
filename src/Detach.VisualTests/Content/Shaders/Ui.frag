#version 330
in vec2 fragUv;
in vec4 fragColor;

uniform sampler2D image;

layout (location = 0) out vec4 outColor;

void main()
{
	outColor = fragColor * texture(image, fragUv.st);
}
