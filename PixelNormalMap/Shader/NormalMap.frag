#version 120

uniform Sampler2D texture

void main()
{
    gl_FragColor = texture2DProj(texture, gl_TexCoord[0]);
}