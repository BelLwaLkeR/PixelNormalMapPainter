using System.Windows;
using SharpGL;
using SharpGL.Shaders;
using System.Text;
using System.IO;
using PixelNormalMap.Device;

namespace PixelNormalMap
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		SharpGLManager glManager ;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OpenGLControl_Loaded(object sender, RoutedEventArgs e)
		{

		}
		private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			glManager.Draw(Width, Height);

		}

		private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
			glManager = SharpGLManager.GetInstance(args.OpenGL);

		}

		private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			glManager.WindowResize();
		}
	}
}
