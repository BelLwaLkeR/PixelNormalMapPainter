using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;
using SharpGL;


using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Windows;
namespace MyFrameWork
{

	public class Texture : Asset
	{


		private int width;
		private int height;

		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		private uint format;

		public uint Format
		{
			get { return format; }
			set { format = value; }
		}

		private string depth;
		public string Depth
		{
			get { return depth; }
			set { depth = value; }
		}




		public Bitmap Data
		{
			get;
			set;
		}

		BitmapData bitmap;
		public BitmapData BitMapData

		{
			get { return bitmap; }
			set { bitmap = value; }
		}

		private uint target;
		public uint Target
		{
			get { return target; }
			set { target = value; }
		}
		
		private string pixelFormat;
		public string PixelFormat
		{
			get { return pixelFormat; }
			set { pixelFormat = value; }
		}


		private string mediaFormat;
		public string MediaFormat
		{
			get { return mediaFormat; }
			set { mediaFormat = value; }
		}

		private string source;
		public string Source
		{
			get { return source; }
			set { source = value; }
		}
		private int size;
		public int Size
		{
			get { return size; }
			set { size = value; }
		}

		//private System.Windows.Media.PixelFormat mediaFormat;
		//public System.Windows.Media.PixelFormat MediaFormat
		//{
		// get { return mediaFormat; }
		// set { mediaFormat = value; }
		//}
	}


	public class CubeTexture
	{
		public Texture[] textures = new Texture[6];
		public CubeTexture(Texture[] texture)
		{
			textures = texture;
		}

	}

	public class GLCubeTexture
	{
		private OpenGL gl;
		uint[] id;

		public GLCubeTexture(OpenGL _gl, CubeTexture texture)
		{
			id = new uint[1];
			gl = _gl;
			gl.GenTextures(1, id);
			change(texture);
		}

		public void Finalize()
		{
			gl.DeleteTextures(1, id);
		}

		public void change(CubeTexture textures)
		{
			gl.Enable(OpenGL.GL_TEXTURE_2D);
			gl.BindTexture(OpenGL.GL_TEXTURE_CUBE_MAP, id[0]);
			uint format = OpenGL.GL_BGR;

			foreach (var texture in textures.textures)
			{
				gl.TexImage2D(texture.Target,
				 0,
				 format,
				 texture.Width,
				 texture.Height,
				 0,
				 format,
				 OpenGL.GL_UNSIGNED_BYTE,
				 texture.BitMapData.Scan0);
			}
			gl.TexParameter(OpenGL.GL_TEXTURE_CUBE_MAP, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
			gl.TexParameter(OpenGL.GL_TEXTURE_CUBE_MAP, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
			gl.TexParameter(OpenGL.GL_TEXTURE_CUBE_MAP, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_CLAMP);
			gl.TexParameter(OpenGL.GL_TEXTURE_CUBE_MAP, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_CLAMP);

			gl.BindTexture(OpenGL.GL_TEXTURE_CUBE_MAP, 0);
			gl.Disable(OpenGL.GL_TEXTURE_2D);

		}

		public void Bind(uint activeNumber)
		{

			gl.ActiveTexture(activeNumber);
			gl.BindTexture(OpenGL.GL_TEXTURE_CUBE_MAP, id[0]);
		}

		public void UnBind()
		{

			gl.BindTexture(OpenGL.GL_TEXTURE_CUBE_MAP, 0);
			gl.ActiveTexture(OpenGL.GL_TEXTURE0);
			gl.Disable(OpenGL.GL_TEXTURE_2D);
		}
	}

	public class GLTexture : Asset
	{
		private OpenGL gl;
		uint[] id;


		public void Finalize()
		{
			gl.DeleteTextures(1, id);
		}
		private string name;

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		int width;
		int height;

		public int Width
		{
			get { return width; }
			set { SetProperty(ref width, value); }
		}

		public int Height
		{
			get { return height; }
			set { SetProperty(ref height, value); }

		}

		private uint format;

		public uint Format
		{
			get { return format; }
			set { SetProperty(ref format, value); }

		}


		private string depth;
		public string Depth
		{
			get { return depth; }
			set { SetProperty(ref depth, value); }

		}

		private string filepath;
		public string FilePath
		{
			get { return filepath; }
			set { SetProperty(ref filepath, value); }
		}

		public Texture Target
		{
			get;
			set;
		}

		private int size;
		public int Size
		{
			get { return size; }
			set { SetProperty(ref size, value); }
		}

		private string source;
		public string Source
		{
			get
			{
				return source;
			}
			set { source = value; }
		}



		public GLTexture(OpenGL _gl, Texture texture)
		{
			gl = _gl;
			id = new uint[1];
			gl.GenTextures(1, id);
			Target = texture;
			if (texture != null)
				change(texture);

		}



		public void change(Texture texture)
		{
			if (texture == null)
			{
				// gl.Disable(OpenGL.GL_TEXTURE_2D);
				gl.DeleteTextures(1, id);
				return;
			}
			Target = texture;
			Width = texture.Width;
			Height = texture.Height;
			Depth = texture.Depth;
			Name = texture.Name;
			Format = texture.Format;
			FilePath = texture.FilePath;
			Size = texture.Size;
			gl.Enable(OpenGL.GL_TEXTURE_2D);

			gl.BindTexture(OpenGL.GL_TEXTURE_2D, id[0]);
			uint format = OpenGL.GL_BGRA;
			uint internalFormat = OpenGL.GL_RGBA;


			if (int.Parse(Depth) == 24)
			{
				internalFormat = OpenGL.GL_RGB;
				format = OpenGL.GL_BGR;

			}

			gl.TexImage2D(texture.Target,
		  0,
		  internalFormat,
		  texture.Width,
		  texture.Height,
		  0,
		  format,
		  OpenGL.GL_UNSIGNED_BYTE,
		  texture.BitMapData.Scan0);

			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
			gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

			gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
			texture.Data.Dispose();

		}

		public void bind(uint activeNumber)
		{
			gl.ActiveTexture(activeNumber);

			gl.BindTexture(OpenGL.GL_TEXTURE_2D, id[0]);



		}

		public void unBind()
		{
			gl.ActiveTexture(OpenGL.GL_TEXTURE0);
			gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
			gl.Disable(OpenGL.GL_TEXTURE_2D);
		}

	}

	public class TextureLoader
	{
		public static BitmapImage Load(string fileName)
		{
			BitmapImage bitmap = new BitmapImage(new Uri(fileName));
			return bitmap;
		}

		public static Texture create(string fileName)
		{

			if (!System.IO.File.Exists(fileName))
			{
				return null;
			}

			FileStream fs;
			int fileSize = (int)new FileInfo(fileName).Length;


			fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			Bitmap bufBMP = new Bitmap((Bitmap)System.Drawing.Bitmap.FromStream(fs));
			fs.Close();

			BitmapDecoder dec	= BitmapDecoder.Create(
				fs,
				BitmapCreateOptions.PreservePixelFormat,
				BitmapCacheOption.Default
			);
			string f			= dec.Frames[0].Format.ToString();
			System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bufBMP.Width, bufBMP.Height);

			//拡張子を取得
			string extention = System.IO.Path.GetExtension(fileName);
			System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

			//今後bmpもやるかもしれないので分岐を作る
			if (extention == ".png")
			{
				format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
			}



			BitmapData gbitmapdata = bufBMP.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bufBMP.PixelFormat);



			bufBMP.UnlockBits(gbitmapdata);
			Bitmap gImage1 = new Bitmap(bufBMP);

			//正規表現で数値だけ抽出
			string pixelFormatStr = gImage1.PixelFormat.ToString();
			Regex re = new Regex(@"[^0-9]");
			string pixelDepth = re.Replace(pixelFormatStr, "");
			//BitmapDecoder bd = BitmapDecoder.Create(fs,BitmapCreateOptions.PreservePixelFormat,BitmapCacheOption.Default);

			return new Texture
			{
				Name		= System.IO.Path.GetFileName(fileName),
				FilePath	= fileName,
				Width		= gImage1.Width,
				Height		= gImage1.Height,
				PixelFormat = gImage1.PixelFormat.ToString(),
				Depth		= pixelDepth,
				Data		= gImage1,
				BitMapData	= gbitmapdata,
				Target		= OpenGL.GL_TEXTURE_2D,
				MediaFormat	= f,
				Source		= fileName,
				Size		= fileSize
			};

		}

		public static CubeTexture createCube(string[] fileName)
		{

			MyFrameWork.Assertion.Assert(fileName.Length == 6, "キューブテクスチャが6枚ありません");
			Texture[] texture = new Texture[6];
			for (int i = 0; i < fileName.Length; ++i)
			{

				Image img = Image.FromFile(fileName[i]);

				Bitmap gImage1 = new Bitmap(img);
				System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, gImage1.Width, gImage1.Height);

				//拡張子を取得
				string extention = System.IO.Path.GetExtension(fileName[i]);
				System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
				//今後bmpもやるかもしれないので分岐を作る
				if (extention == ".png")
				{
					format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
				}



				BitmapData gbitmapdata = gImage1.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, gImage1.PixelFormat);



				gImage1.UnlockBits(gbitmapdata);



				//正規表現で数値だけ抽出
				string pixelFormatStr = gImage1.PixelFormat.ToString();
				Regex re = new Regex(@"[^0-9]");
				string pixelDepth = re.Replace(pixelFormatStr, "");

				uint[] targets = new uint[6];
				targets[0] = OpenGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_X;

				targets[1] = OpenGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y;

				targets[2] = OpenGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z;

				targets[3] = OpenGL.GL_TEXTURE_CUBE_MAP_POSITIVE_X;

				targets[4] = OpenGL.GL_TEXTURE_CUBE_MAP_POSITIVE_Y;

				targets[5] = OpenGL.GL_TEXTURE_CUBE_MAP_POSITIVE_Z;


				texture[i] = new Texture
				{
					Name = System.IO.Path.GetFileName(fileName[i]),
					FilePath = fileName[i],
					Width = gImage1.Width,
					Height = gImage1.Height,
					Depth = pixelDepth,
					BitMapData = gbitmapdata,
					Target = targets[i],

				};
			}
			return new CubeTexture(texture);
		}

		public void load(string filepath)
		{
			try
			{
				Color[,] pixelData;
				int widht, height;

				System.Drawing.Imaging.ImageFormat format;

				//パスから画像を読み込む
				using (Bitmap img = new Bitmap(Image.FromFile(filepath)))
				{
					widht = img.Width;
					height = img.Height;
					format = img.RawFormat;

					pixelData = new Color[img.Width, img.Height];
					for (int y = 0; y < img.Height; ++y)
					{
						for (int x = 0; x < img.Width; ++x)
						{
							pixelData[x, y] = img.GetPixel(x, y);
						}
					}
				}

				Color[,] newPixelData = new Color[widht, height];
				//カラーをもとにグレースケール変換して新たにカラーを生成
				for (int y = 0; y < height; ++y)
				{
					for (int x = 0; x < widht; ++x)
					{
						Color originalColor = pixelData[x, y];
						int brightness = (int)(originalColor.GetBrightness() * 255);
						Color newColor = Color.FromArgb(originalColor.A, brightness, brightness, brightness);
						newPixelData[x, y] = newColor;
					}
				}

				//新しいビットマップを生成して保存
				using (Bitmap saveImg = new Bitmap(widht, height))
				{
					for (int y = 0; y < height; ++y)
					{
						for (int x = 0; x < widht; ++x)
						{
							saveImg.SetPixel(x, y, newPixelData[x, y]);

						}
					}

					string outputPath = filepath.Insert(filepath.LastIndexOf('.'), "" + DateTime.Now.Ticks);
					saveImg.Save(outputPath, format);
				}
				Console.WriteLine("success");
			}
			catch (Exception e)
			{
				Console.WriteLine("error");
				Console.WriteLine(e.ToString());
			}

		}
	}
}
