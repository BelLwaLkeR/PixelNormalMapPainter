using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace PixelNormalMap.Utility
{
	class DataLoader
	{
		public static string TextLoader(string path) {
			try
			{
				return File.ReadAllText(path);
			}
			catch (Exception e) {
				Debug.Assert(!(e is ArgumentNullException || e is ArgumentException), "path[" + path + "]が不正です。");
				Debug.Assert(!(e is FileNotFoundException), "ファイル[" + path + "]が見つかりませんでした。");
				Debug.Assert(!(e is DirectoryNotFoundException), "ファイルのディレクトリ[" + path + "]が見つかりませんでした。");
			}
			return null;
		}
	}
}
