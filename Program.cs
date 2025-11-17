using System.Drawing;
using System.Numerics;
using gd.engine;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gd
{
    public class Program
    {
        private static IWindow _window;
        private static GL _gl;

        private static uint _vao; // Vertex Access Objects
        private static uint _vbo; // Vertex Buffer Objects
        private static uint _ebo; // Element Buffer Objects

        private static Vector2D<int> windowSize = new Vector2D<int>(800, 600);

        private static ShaderDetails[] Shaders = {
            new ShaderDetails("shader.vert", ShaderType.VertexShader),
            new ShaderDetails("shader.frag", ShaderType.FragmentShader)
        };
        private static engine.Shader Shader;

        private static List<Vector3> cubePositions = new List<Vector3> {
            new Vector3( 0.0f,  0.0f,  0.0f),
            // new Vector3( 2.0f,  5.0f, -15.0f),
            // new Vector3(-1.5f, -2.2f, -2.5f),
            // new Vector3(-3.8f, -2.0f, -12.3f),
            // new Vector3( 2.4f, -0.4f, -3.5f),
            // new Vector3(-1.7f,  3.0f, -7.5f),
            // new Vector3( 1.3f, -2.0f, -2.5f),
            // new Vector3( 1.5f,  2.0f, -2.5f),
            // new Vector3( 1.5f,  0.2f, -1.5f),
            // new Vector3(-1.3f,  1.0f, -1.5f)
        };

        static void Main(String[] args)
        {
            cubePositions = new List<Vector3>();
            int numShapes = 1;
            for (int i = 0; i < numShapes; i++)
            {
                for (int j = 0; j < numShapes; j++)
                {
                    cubePositions.Add(
                        new Vector3(j, 0 , i * -1)
                    );
                }
            }

            WindowOptions options = WindowOptions.Default with
            {
                Size = windowSize,
                Title = "Voxel Engine"
            };

            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.FramebufferResize += OnFramebufferResize;
            _window.Closing += OnClose;

            _window.Run();

            _window.Dispose();
        }

        private static unsafe void OnLoad()
        {
            IInputContext input = _window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
                input.Keyboards[i].KeyDown += KeyDown;

            _gl = _window.CreateOpenGL();
            _gl.ClearColor(Color.Gray);

            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

            Draw.DrawShape("cube", _gl);

            _ebo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

            Shader = new engine.Shader(_gl, Shaders);

            const uint positionLoc = 0;
            _gl.EnableVertexAttribArray(positionLoc);
            _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)0);

            const uint texCoordLoc = 1;
            _gl.EnableVertexAttribArray(texCoordLoc);
            _gl.VertexAttribPointer(texCoordLoc, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)(3 * sizeof(float)));

            // Clean up
            _gl.BindVertexArray(0); // vertex array must be unbound first
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            if (key == Key.Escape)
            {
                _window.Close();
            }
        }

        private static unsafe void OnRender(double deltaTime)
        {
            _gl.Enable(EnableCap.DepthTest);
            _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

            _gl.BindVertexArray(_vao);
            Shader.Use();

            Matrix4x4 view = Matrix4x4.Identity 
                // * Matrix4x4.CreateRotationX(convertToRadians(20)) 
                * Matrix4x4.CreateTranslation(-0.5f, 0.0f, -20.0f);
            Shader.SetUniform("view", view);

            Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(
                convertToRadians(45.0f), 800 / 600, 0.1f, 100.0f
            );
            Shader.SetUniform("projection", projection);

            var difference = (float)(Glfw.GetApi().GetTime() * 50);

            for (int i = 0; i < cubePositions.Count; i++)
            {
                Vector3 cube = cubePositions[i];
                // Matrix4x4 model = ;
                float angle = 20.0f * i;

                Matrix4x4 model = Matrix4x4.Identity
                    // * Matrix4x4.CreateScale(0.1f)
                    // * Matrix4x4.CreateRotationY(convertToRadians(-45))
                    // * Matrix4x4.CreateRotationX(convertToRadians(225))
                    * Matrix4x4.CreateRotationY(convertToRadians(difference))
                    * Matrix4x4.CreateRotationX(convertToRadians(difference))
                    * Matrix4x4.CreateTranslation(cube);

                Shader.SetUniform("model", model);

                _gl.DrawArrays(GLEnum.Triangles, 0, 36);
                // _gl.DrawArrays(GLEnum.Triangles, 0, 18);
            }
        }

        private static float convertToRadians(float degrees)
        {
            return MathF.PI / 180f * degrees;
        }

        private static void OnFramebufferResize(Vector2D<int> newSize)
        {
            _gl.Viewport(newSize);
            windowSize = newSize;
        }

        private static void OnClose()
        {
            // Shader.Dispose();
            // Texture.Dispose();
        }
    }
}

