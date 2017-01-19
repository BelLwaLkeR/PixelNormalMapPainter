using System;
using SharpGL;
using SharpGL.Shaders;

public class SharpGLManager
{
	private OpenGL glInstance;

	SharpGLManager() {
		glInstance = new OpenGL();
	}
}
