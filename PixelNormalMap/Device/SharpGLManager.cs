using System.Text;
using System.Diagnostics;
using System.IO;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System.Drawing;
using System.Drawing.Imaging;
using PixelNormalMap.Utility.Type;

namespace PixelNormalMap.Device
{
	public class SharpGLManager
	{

		private static SharpGLManager instance;
		private OpenGL glInstance;
		private uint shaderProgram;
		public Vector3 lightPosition;

		uint vertShader;
		uint fragShader;
	
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
			lightPosition = new Vector3();
			glInstance.GetInteger(OpenGL.GL_MAX_TEXTURE_IMAGE_UNITS, new int[]{ 5 } );
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

		private void LoadTexture(string texturePath, string textureName, uint textureNo, uint glTexuteNo) {
			
			Texture texture = new Texture();
			texture.Create(glInstance, texturePath);

			glInstance.ActiveTexture(glTexuteNo);
			
			uint glTexture = texture.TextureName;
			glInstance.BindTexture(OpenGL.GL_TEXTURE_2D, glTexture);


			glInstance.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
			glInstance.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);

			int id = glInstance.GetUniformLocation(shaderProgram, textureName);

			glInstance.Uniform1ARB(id, textureNo);

			glInstance.BindTexture(OpenGL.GL_TEXTURE_2D, glTexture);
			

		}

		private void SetUniform1(string valueName,  float value) {
			glInstance.Uniform1(glInstance.GetUniformLocation(shaderProgram, valueName), value);
		}
		private void SetUniform3(string valueName, Vector3 value)
		{
			glInstance.Uniform3(glInstance.GetUniformLocation(shaderProgram, valueName), value.x, value.y, value.z);
		}
		private void SetUniform4(string valueName, Vector4 value)
		{
			glInstance.Uniform4(glInstance.GetUniformLocation(shaderProgram, valueName), value.x, value.y, value.z, value.w);
		}

		private void DrawSetup() {

			//glInstance.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			//glInstance.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);              // Black Background
			//glInstance.ClearDepth(1.0f);                                // Depth Buffer Setup
			//glInstance.UseProgram(shaderProgram);
			//glInstance.Disable(OpenGL.GL_DEPTH_TEST);                   // Disables Depth Testing
			//glInstance.Enable(OpenGL.GL_BLEND);                         // Enable Blending

			//glInstance.Enable(OpenGL.GL_TEXTURE_2D);

			//glInstance.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE);
			//LoadTexture("./Texture/texture.png"		, "texture");
			//LoadTexture("./Texture/normal.png"	, "normalMap");
			//instance.shaderProgram = glInstance.CreateProgram();
			//glInstance.AttachShader(shaderProgram, vertShader);
			//glInstance.AttachShader(shaderProgram, fragShader);
			//glInstance.LinkProgram(shaderProgram);

		}

		public void Draw(double windowWidth, double windowHeight)
		{
			//テクスチャ設定
			//  We need to load the texture from file.

			//  A bit of extra initialisation here, we have to enable textures.

			//DrawSetup();
			glInstance.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			glInstance.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);              // Black Background
			glInstance.ClearDepth(1.0f);                                // Depth Buffer Setup
			glInstance.UseProgram(shaderProgram);
			glInstance.Disable(OpenGL.GL_DEPTH_TEST);                   // Disables Depth Testing
			

			instance.shaderProgram = glInstance.CreateProgram();
			glInstance.AttachShader(shaderProgram, vertShader);
			glInstance.AttachShader(shaderProgram, fragShader);
			glInstance.LinkProgram(shaderProgram);


			//LoadTexture("./Texture/texture.png", "texture", 1);
			LoadTexture("./Texture/texture.png", "texture2"	, 0, OpenGL.GL_TEXTURE0);
			LoadTexture("./Texture/normal.png", "normalMap"	, 1, OpenGL.GL_TEXTURE1);

			////LightPosition
			SetUniform3("lightPosition", lightPosition);

			//LightColor
			SetUniform3("lightColor", new Vector3(1, 1, 1));


			//UseProgram
			glInstance.LoadIdentity();
			glInstance.Translate(windowWidth / 2, windowHeight / 2, 0.0f);
			glInstance.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
			glInstance.Begin(OpenGL.GL_QUADS);

			glInstance.Color(1.0f, 1.0f, 1.0f);
			glInstance.TexCoord(0, 1);	glInstance.Vertex(-windowWidth	/ 2, 0.0f, -windowHeight / 2);
			glInstance.TexCoord(1, 1);	glInstance.Vertex(windowWidth	/ 2, 0.0f, -windowHeight / 2);
			glInstance.TexCoord(1, 0);	glInstance.Vertex(windowWidth	/ 2, 0.0f,  windowHeight / 2);
			glInstance.TexCoord(0, 0);	glInstance.Vertex(-windowWidth	/ 2, 0.0f,  windowHeight / 2);
			
			glInstance.UseProgram(0);

			//  Flush OpenGL
			glInstance.End();
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