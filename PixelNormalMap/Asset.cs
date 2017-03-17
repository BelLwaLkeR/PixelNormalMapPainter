using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFrameWork
{
    public enum eAssetType
    {
        SCENE,
        TEXTURE,
        MESH,
        SKINNING,
        PARTICLE,
        MATERIAL,
        MODEL,
        FOLDER,
        SCRIPT
    }

    public class Asset : PropertyModel
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }


        private string filepath;
        public string FilePath
        {
            get { return filepath; }
            set { SetProperty(ref filepath,value); }
        }

        private eAssetType assetType;
        public eAssetType AssetType
        {
            get { return assetType; }
            set { SetProperty(ref assetType, value); }
        }

        private int fileSize;
        public int FileSize
        {
            get {return fileSize;}
            set{
                SetProperty(ref fileSize, value);
            }
        }
    }

    public class AssetManager
    {
        static Dictionary<string, Texture> TextureAsset;
//        static  Dictionary<string, Mesh> MeshAsset;

        public static string GetAssetType(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    UInt16 strSize;
                    strSize = reader.ReadUInt16();

                    char[] charas = reader.ReadChars(strSize);
                    return new string(charas);
                }
            }
        }
    }
}
