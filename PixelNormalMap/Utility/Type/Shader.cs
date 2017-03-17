using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelNormalMap.Utility.Type
{
	/// <summary>
	/// 頂点シェーダとフラグメントシェーダをセットにしたクラス
	/// </summary>
	class Shader
	{
		//頂点シェーダ
		public uint vertex;

		//フラグメントシェーダ
		public uint fragment;

		/// <summary>
		/// 未指定での初期化
		/// </summary>
		public Shader() {
			vertex		= 0;
			fragment	= 0;
		}

		/// <summary>
		/// 各シェーダを指定して初期化
		/// </summary>
		/// <param name="vertex">頂点シェーダのID</param>
		/// <param name="fragment">フラグメントシェーダのID</param>
		public Shader(uint vertex, uint fragment) {
			this.vertex		= vertex;
			this.fragment	= fragment;
		}
	}
}
