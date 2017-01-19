using System.Text;
using System.Diagnostics;
using System.IO;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System.Drawing;
using System.Drawing.Imaging;

namespace PixelNormalMap.Device
{
	public class SharpGLManager
	{

		private static SharpGLManager instance;
		private OpenGL glInstance;
		private uint shaderProgram;
		private System.Drawing.Imaging.BitmapData bitmap;

		uint vertShader;
		uint fragShader;

		uint[] textureData;
		uint[] normalMapData;



		private SharpGLManager() { }

		public static SharpGLManager GetInstance()
		{
			if (instance == null) { instance = new SharpGLManager(); }
			return instance;
		}

		public static SharpGLManager GetInstance(OpenGL glInstance)
		{
			GetInstance();
			instance.Initialize(glInstance);
			return instance;
		}

		public void Initialize(OpenGL glInstance) {
			instance.glInstance = glInstance;
			instance.LoadShader("./Shader/NormalMap.vert", "./Shader/NormalMap.frag");

		}

		public void WindowResize()
		{
			glInstance.MatrixMode(OpenGL.GL_PROJECTION);
			glInstance.LoadIdentity();
			glInstance.Ortho2D(
				0,
				glInstance.RenderContextProvider.Width,
				glInstance.RenderContextProvider.Height,
				0);

			glInstance.MatrixMode(OpenGL.GL_MODELVIEW);
		}


		public void Draw(double windowWidth, double windowHeight)
		{
			instance.shaderProgram = glInstance.CreateProgram();
			glInstance.AttachShader(shaderProgram, vertShader);
			glInstance.AttachShader(shaderProgram, fragShader);
			glInstance.LinkProgram(shaderProgram);



			//テクスチャ設定
			textureData = new uint[1];
			uint[] textures = new uint[1];
			SharpGL.OpenGL gl = glInstance;
			gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);                  // Black Background
			gl.ClearDepth(1.0f);                            // Depth Buffer Setup
			gl.Disable(OpenGL.GL_DEPTH_TEST);                       // Disables Depth Testing
			gl.Enable(OpenGL.GL_BLEND);                         // Enable Blending
			gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE);
			//  We need to load the texture from file.

			//  A bit of extra initialisation here, we have to enable textures.
			gl.Enable(OpenGL.GL_TEXTURE_2D);

			Texture texture = new Texture();
			texture.Create(gl, @"D:\Texture.png");

			textures[0] = texture.TextureName;
			gl.BindTexture(OpenGL.GL_TEXTURE_2D, textures[0]);

			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);

			
			glInstance.Uniform1(glInstance.GetUniformLocation(shaderProgram, "texture"), textures[0]);


			//UseProgram
			glInstance.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			glInstance.UseProgram(shaderProgram);

			glInstance.LoadIdentity();
			glInstance.Translate(windowWidth / 2, windowHeight / 2, 0.0f);
			glInstance.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
			glInstance.Begin(OpenGL.GL_QUADS);

			glInstance.Color(1.0f, 1.0f, 1.0f);
			glInstance.TexCoord(0, 1);	glInstance.Vertex(-windowWidth	/ 2, 0.0f, -windowHeight / 2);
			glInstance.TexCoord(1, 1);	glInstance.Vertex(windowWidth	/ 2, 0.0f, -windowHeight / 2);
			glInstance.TexCoord(1, 0);	glInstance.Vertex(windowWidth	/ 2, 0.0f,  windowHeight / 2);
			glInstance.TexCoord(0, 0);	glInstance.Vertex(-windowWidth	/ 2, 0.0f,  windowHeight / 2);

			glInstance.End();

			glInstance.UseProgram(0);

			//  Flush OpenGL
			glInstance.Flush();
		}

		private void LoadShader(string vertexShaderPath, string fragmentShaderPath) {
			LoadShader(EShaderType.Vertex	, vertexShaderPath);
			LoadShader(EShaderType.Fragment	, fragmentShaderPath);
		}

		private void LoadShader(EShaderType shaderType, string shaderPath) {

			string shaderScript = Utility.DataLoader.TextLoader(shaderPath);

			switch (shaderType) {
				case EShaderType.Vertex:
					LoadVertexShader(shaderScript);
					break;
				case EShaderType.Fragment:
					LoadFragmentShader(shaderScript);
					break;
				default:
					break;
			}
		}

		private void LoadVertexShader(string shaderScript) {
			uint vertShader = glInstance.CreateShader(OpenGL.GL_VERTEX_SHADER);
			glInstance.ShaderSource(vertShader, shaderScript);
			glInstance.CompileShader(vertShader);

			StringBuilder builder = new StringBuilder(2048);
			glInstance.GetShaderInfoLog(vertShader, 2048, System.IntPtr.Zero, builder);
			string resoult = builder.ToString();
			Debug.Assert(resoult.Equals(""), "VertexShaderCompileError!! " + resoult);

			this.vertShader = vertShader;
			//glInstance.AttachShader(shaderProgram, vertShader);

		}

		private void LoadFragmentShader(string shaderScript) {
			uint fragShader = glInstance.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
			glInstance.ShaderSource(fragShader, shaderScript);
			glInstance.CompileShader(fragShader);

			StringBuilder builder = new StringBuilder(2048);
			glInstance.GetShaderInfoLog(fragShader, 2048, System.IntPtr.Zero, builder);
			string resoult = builder.ToString();

			Debug.Assert(resoult.Equals(""), "FragmentShaderCompileError!! " + resoult);

			this.fragShader = fragShader;
//			glInstance.AttachShader(shaderProgram, fragShader);

		}
	}
}