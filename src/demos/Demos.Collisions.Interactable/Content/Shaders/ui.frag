layout (location = 0) out vec4 outColor;

in vec2 fragUv;
in vec4 fragColor;

uniform sampler2D image;

void main()
{
	outColor = fragColor * texture(image, fragUv.st);
}
