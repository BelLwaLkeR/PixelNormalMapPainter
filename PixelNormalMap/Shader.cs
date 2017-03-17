using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.Shaders;
using SharpGL;
using System.IO;
namespace MyFrameWork
{
    public class ShaderLoader
    {
        public static SharpGL.Shaders.ShaderProgram Create(OpenGL gl, string vertexShader, string flagmentShader)
        {
            SharpGL.Shaders.ShaderProgram shader = new SharpGL.Shaders.ShaderProgram();
            string vfile = File.ReadAllText(vertexShader);
            string ffile = File.ReadAllText(flagmentShader);


            shader.Create(gl, vfile, ffile, new Dictionary<uint, string>());

            return shader;
        }
    }

    public class Shader
    {
        private OpenGL gl;
        uint shader;
        uint type;
        public uint ShaderObject
        {
            get { return shader; }
        }
        public uint Type
        {
            get { return type; }
        }
        public Shader(OpenGL _gl)
        {
            gl = _gl;
        }

        

     public   void Finalize()
         {
             gl.DeleteShader(shader);
             shader = 0;
       
         }

        public void Create(uint type, string fileName)
        {
            shader = gl.CreateShader(type);

            Compile(fileName);
        }
        private void SetSource(string fileName)
        {
            try { 

            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                gl.ShaderSource(shader, reader.ReadToEnd());
                reader.Close();
            }
                }catch(IOException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }
        private void Compile(string fileName)
        {

            SetSource(fileName);
            gl.CompileShader(shader);
            int[] parameter = new int[] { 0 };
            gl.GetShader(shader, OpenGL.GL_COMPILE_STATUS, parameter);
            if (parameter[0] != OpenGL.GL_TRUE)
            {
                //失敗
                int[] infoLength = new int[] { 0 };
                gl.GetShader(shader, OpenGL.GL_INFO_LOG_LENGTH, infoLength);
                StringBuilder il = new StringBuilder(1000);
                gl.GetShaderInfoLog(shader, 10000, IntPtr.Zero, il);

                Assertion.Assert(false, "shader compile error\r\n" + fileName + "\r\n" + il.ToString());
            }
        }
    }


    public class ShaderProgram
    {
        OpenGL gl;
        uint shaderProgram;
        public uint Program
        {
            get { return shaderProgram; }
        }
        public ShaderProgram(OpenGL _gl)
        {
            gl = _gl;
            shaderProgram = gl.CreateProgram();
        }

      public  void Finalize()
        {
            gl.DeleteProgram(shaderProgram);
        }

        public void Create(string vertexShaderFile, string flagmentShaderFile)
        {

        }

        public void CompileShader(uint type, string shaderFile)
        {
            Shader shader = new Shader(gl);
            shader.Create(type,shaderFile);
            gl.AttachShader(shaderProgram, shader.ShaderObject);
            shader.Finalize();
        }
        public void Bind()
        {
            gl.UseProgram(shaderProgram);
        }

        public void UnBind()
        {
            gl.UseProgram(0);
        }

        public int Uniform(string uniformName)
        {
            return gl.GetUniformLocation(shaderProgram,uniformName);
        }

        public int Attribute(string attributeName)
        {
            return gl.GetAttribLocation(shaderProgram, attributeName);
        }

        public void SetTransformFeedback(string[] varyingNames)
        {
            gl.TransformFeedbackVaryings(shaderProgram, varyingNames.Length, varyingNames, OpenGL.GL_INTERLEAVED_ATTRIBS);
        }

        public void Link()
        {
            gl.LinkProgram(shaderProgram);

            int[] linked = new int[] { 0 };
            gl.GetProgram(shaderProgram, OpenGL.GL_LINK_STATUS, linked);
            if(linked[0] == OpenGL.GL_FALSE)
            {
                int[] length = new int[] { 0 };
                gl.GetProgram(shaderProgram, OpenGL.GL_INFO_LOG_LENGTH, length);

                int bufSize = length[0];

                StringBuilder il = new StringBuilder(bufSize);
                gl.GetProgramInfoLog(shaderProgram, bufSize, IntPtr.Zero, il);
                Assertion.Assert(false, "shader link error\r\n" + il.ToString());
            }
        }
    }

    public class Material
    {
        private ShaderProgram program;
        OpenGL gl;
        public Material()
        {

        }

        public Material(OpenGL _gl,string vertexShaderFile, string fragmentShaderFile)
        {
            gl = _gl;
            program = new ShaderProgram(_gl);
            program.CompileShader(OpenGL.GL_VERTEX_SHADER, vertexShaderFile);
            program.CompileShader(OpenGL.GL_FRAGMENT_SHADER, fragmentShaderFile);
            program.Link();
        }

        public Material(OpenGL _gl, string vertexShaderFile, string fragmentShaderFile, string geometryShader)
        {
            gl = _gl;
            program = new ShaderProgram(_gl);
            program.CompileShader(OpenGL.GL_VERTEX_SHADER, vertexShaderFile);
            program.CompileShader(OpenGL.GL_FRAGMENT_SHADER, fragmentShaderFile);
            program.CompileShader(OpenGL.GL_GEOMETRY_SHADER, geometryShader);

            program.Link();
        }

        public Material(OpenGL _gl, string vertexShaderFile, string geometryShader, string[] varyingNames)
        {
            gl = _gl;
            program = new ShaderProgram(_gl);
            program.CompileShader(OpenGL.GL_VERTEX_SHADER, vertexShaderFile);
            program.CompileShader(OpenGL.GL_GEOMETRY_SHADER, geometryShader);
            program.SetTransformFeedback(varyingNames);
            program.Link();
        }

        public Material(OpenGL _gl, string vertexShaderFile,string[] varyingNames)
        {
            gl = _gl;
            program = new ShaderProgram(_gl);
            program.CompileShader(OpenGL.GL_VERTEX_SHADER, vertexShaderFile);
            program.SetTransformFeedback(varyingNames);
            program.Link();
        }

        public void Finalize()
        {
            program.Finalize();
        }
        public void Bind()
        {
            program.Bind();
        }

        public void UnBind()
        {
            program.UnBind();
        }

        public void SetParameter(string uniformName, float value)
        {
            gl.Uniform1(program.Uniform(uniformName), value);
        }


        public void SetParameter(string uniformName, PixelNormalMap.Utility.Type.Vector2 value)
        {
            gl.Uniform2(program.Uniform(uniformName), value.x, value.y);   
        }

        public void SetParameter(string uniformName, PixelNormalMap.Utility.Type.Vector3 value)
        {
            gl.Uniform3(program.Uniform(uniformName), value.x, value.y,value.z);
        }
        //public void SetParameter(string uniformName, List<Vector3> value)
        //{
        //    List<float> values = new List<float>();

        //    for (int i = 0; i < value.Count; ++i)
        //    {
        //            values.Add(value[i].X);
        //            values.Add(value[i].Y);
        //            values.Add(value[i].Z);

        //    }



        //    gl.Uniform3(program.Uniform(uniformName),values.Count,values.ToArray());
        //}

        public void SetParameter(string uniformName, PixelNormalMap.Utility.Type.Vector4 value)
        {
            gl.Uniform4(program.Uniform(uniformName), value.x, value.y, value.z,value.w);
        }

        //public void SetParameter(string uniformName, GlmNet.mat4 value)
        //{
        //    gl.UniformMatrix4(program.Uniform(uniformName), 1, false, value.to_array());
        //}

        //public void SetParameter(string uniformName, GlmNet.mat4[] value)
        //{
        //    List<float> values = new List<float>();
        //    //for (int i = 0; i < value.Length; ++i)
        //    //{
        //    //    values = values.Concat(value[i].to_array()).ToArray();
        //    //}

        //    for (int i = 0; i < value.Length; ++i)
        //    {
        //        for (int j = 0; j < 4; ++j)
        //        { 
        //            values.Add(value[i][j].x);
        //            values.Add(value[i][j].y);
        //            values.Add(value[i][j].z);
        //            values.Add(value[i][j].w);

        //        }
        ////    }

        //    gl.UniformMatrix4(program.Uniform(uniformName), value.Length, false, values.ToArray());
        //}

        public void SetTexture(string uniformName, int value)
        {
            int location =program.Uniform(uniformName);
            gl.Uniform1(location, value);
        }
    }
}
