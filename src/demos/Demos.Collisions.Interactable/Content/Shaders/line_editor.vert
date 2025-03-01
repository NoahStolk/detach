layout (location = 0) in vec3 aPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec4 fragPos;

void main()
{
	vec4 translation = model * vec4(aPosition, 1.0);
	gl_Position = projection * view * translation;

	fragPos = translation;
}
