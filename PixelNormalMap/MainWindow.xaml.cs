using System.Windows;
using SharpGL;
using SharpGL.Shaders;
using System.Text;
using System.IO;

namespace PixelNormalMap
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OpenGLControl_Loaded(object sender, RoutedEventArgs e)
		{

		}
		private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			OpenGL gl = args.OpenGL;

			//string vertexSource		= "./Shader/NormalMap.vert";
			//string fragmentSource	= "./Shader/NormalMap.frag";

			//ShaderProgram shaderProgram = new ShaderProgram();
			//shaderProgram.Create(gl, vertexSource, fragmentSource, null);

			uint vertShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
			string vertScript =  File.ReadAllText("./Shader/NormalMap.vert") ;
			gl.ShaderSource(vertShader, vertScript);
			gl.CompileShader(vertShader);

			StringBuilder builder = new StringBuilder(2048);
			gl.GetShaderInfoLog(vertShader,2048, System.IntPtr.Zero, builder);
			string resoult = builder.ToString();

			if (!resoult.Equals(""))
			{
				System.Console.WriteLine("=== Vertex ===");
				System.Console.WriteLine(resoult);
				int a = 0;
			}


			uint fragShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
			string fragScript = File.ReadAllText("./Shader/NormalMap.frag");
			gl.ShaderSource(fragShader, fragScript);
			gl.CompileShader(fragShader);

			gl.GetShaderInfoLog(fragShader, 2048, System.IntPtr.Zero, builder);
			resoult = builder.ToString();

			if (!resoult.Equals(""))
			{
				System.Console.WriteLine("=== Fragment ===");
				System.Console.WriteLine(resoult);
				int a = 0;
			}

			uint program = gl.CreateProgram();
			gl.AttachShader(program, vertShader);
			gl.AttachShader(program, fragShader);
			gl.LinkProgram(program);

			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			gl.UseProgram(program);

			//gl.Uniform
			gl.LoadIdentity();
			gl.Translate(Width/2, Height/2, 0.0f);
			gl.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
			gl.Begin(OpenGL.GL_QUADS);

			gl.Color(0.0f, 1.0f, 0.0f);
			gl.Vertex(Width/2, 0.0f, -Height / 2);
			gl.Vertex(-Width / 2, 0.0f, -Height / 2);
			gl.Vertex(-Width / 2, 0.0f, Height / 2);
			gl.Vertex(Width / 2, 0.0f, Height / 2);

			gl.End();

			gl.UseProgram(0);

			//  Flush OpenGL.
			gl.Flush();
		}

		private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
		}

		private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			OpenGL gl = args.OpenGL;
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.LoadIdentity();
			gl.Ortho2D(
				0,
				gl.RenderContextProvider.Width,
				gl.RenderContextProvider.Height,
				0);

			gl.MatrixMode(OpenGL.GL_MODELVIEW);
		}
	}
}
