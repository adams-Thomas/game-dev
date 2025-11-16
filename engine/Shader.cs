using System.Collections;
using System.Numerics;
using Silk.NET.OpenGL;

namespace gd.engine
{
    public class Shader
    {
        private uint _program;
        private GL _gl;
        public Shader(GL gl, params ShaderDetails[] shaders)
        {
            _gl = gl;
            _program = _gl.CreateProgram();

            uint[] shaderIds = shaders.Select(s =>
            {
                uint id = loadShader(s);
                _gl.AttachShader(_program, id);
                return id;
            }).ToArray();

            _gl.LinkProgram(_program);

            _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int pStatus);
            if (pStatus != (int)GLEnum.True)
                throw new Exception("Program failed to link: " + _gl.GetProgramInfoLog(_program));

            foreach (uint shader in shaderIds)
            {
                _gl.DetachShader(_program, shader);
                _gl.DeleteShader(shader);
            }
        }

        public void Use()
        {
            _gl.UseProgram(_program);
        }

        public unsafe void SetUniform(String name, Matrix4x4 matrix)
        {
            int viewLoc = _gl.GetUniformLocation(_program, name);
            _gl.UniformMatrix4(viewLoc, 1, false, (float*)&matrix);
        }

        private uint loadShader(ShaderDetails shaderDetails)
        {
            string src = File.ReadAllText("shaders/" + shaderDetails.Path);
            uint shader = _gl.CreateShader(shaderDetails.Type);

            _gl.ShaderSource(shader, src);
            _gl.CompileShader(shader);

            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
            if (status != (int)GLEnum.True)
                throw new Exception(shaderDetails.Path + " failed to compile: " + _gl.GetShaderInfoLog(shader));

            return shader;
        }
    }

    public class ShaderDetails
    {
        public string Path { get; set; }
        public ShaderType Type { get; set; }

        public ShaderDetails(String path, ShaderType type)
        {
            Path = path;
            Type = type;
        }
    }
}