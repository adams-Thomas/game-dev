using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;
using Silk.NET.GLFW;

namespace gd.common;

public class Window
{
    private WindowOptions options;
    private static IWindow _window;
    private static GL _gl;
    private static uint _vao; // Vertex Access Objects
    private static uint _vbo; // Vertex Buffer Objects
    private static uint _ebo; // Element Buffer Objects
    private static uint _program;

    private static Vector3[] cubePositions = {
    new Vector3( 0.0f,  0.0f,  0.0f),
    new Vector3( 2.0f,  5.0f, -15.0f),
    new Vector3(-1.5f, -2.2f, -2.5f),
    new Vector3(-3.8f, -2.0f, -12.3f),
    new Vector3( 2.4f, -0.4f, -3.5f),
    new Vector3(-1.7f,  3.0f, -7.5f),
    new Vector3( 1.3f, -2.0f, -2.5f),
    new Vector3( 1.5f,  2.0f, -2.5f),
    new Vector3( 1.5f,  0.2f, -1.5f),
    new Vector3(-1.3f,  1.0f, -1.5f)
        };

    public Window(int width, int height, string title)
    {
        options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(width, height),
            Title = title
        };

        _window = Silk.NET.Windowing.Window.Create(options);
        _window.Load += setInputs;
        _window.Render += setOnRender;
    }

    public void Run()
    {
        _window.Run();
    }

    public static Vector3 Position { get; set; } = new Vector3(0, 0, 0);
    public static float Scale { get; set; } = 0.4f;
    public static Quaternion Rotation { get; set; } = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Glfw.GetApi().GetTime());

    private static unsafe void setInputs()
    {
        IInputContext input = _window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;

        _gl = _window.CreateOpenGL();

        _gl.ClearColor(Color.CornflowerBlue);

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        float[] vertices =
        {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        fixed (float* buf = vertices)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);

        uint[] indices =
        {
            0u, 1u, 3u,
            1u, 2u, 3u
        };

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        fixed (uint* buf = indices)
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);

        const string vertexCode = @"
            #version 330 core

            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTextureCoord;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;

            // Stores and outputs the data we want to the fragment shader
            out vec2 frag_textCoords;

            void main()
            {
                gl_Position = projection * view * model * vec4(aPosition, 1.0);
                frag_textCoords = aTextureCoord;
            }";

        const string fragmentCode = @"
            #version 330 core
            
            in vec2 frag_textCoords;

            out vec4 out_color;

            void main() {
                out_color = vec4(frag_textCoords.x, frag_textCoords.y, 0.0, 1.0);
            }
        ";

        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, vertexCode);

        _gl.CompileShader(vertexShader);
        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
        if (vStatus != (int)GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + _gl.GetShaderInfoLog(vertexShader));

        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, fragmentCode);

        _gl.CompileShader(fragmentShader);
        _gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
        if (fStatus != (int)GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + _gl.GetShaderInfoLog(fragmentShader));

        _program = _gl.CreateProgram();
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);

        _gl.LinkProgram(_program);

        _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int pStatus);
        if (pStatus != (int)GLEnum.True)
            throw new Exception("Program failed to link: " + _gl.GetProgramInfoLog(_program));

        _gl.DetachShader(_program, vertexShader);
        _gl.DetachShader(_program, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);

        const uint positionLoc = 0; // Has to be same as aLocation in shader
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)0);

        const uint texCoordLoc = 1;
        _gl.EnableVertexAttribArray(texCoordLoc);
        // Change offset to read the last 2 values of the vertex
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

    private static unsafe void setOnRender(double deltaTime)
    {
        _gl.Enable(EnableCap.DepthTest);
        _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

        _gl.BindVertexArray(_vao);
        _gl.UseProgram(_program);

        var difference = (float)(Glfw.GetApi().GetTime() * 50);

        Matrix4x4 view = Matrix4x4.Identity * Matrix4x4.CreateTranslation(0.0f, 0.0f, -3.0f);
        int viewLoc = _gl.GetUniformLocation(_program, "view");
        _gl.UniformMatrix4(viewLoc, 1, false, (float*)&view);

        Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(
            convertToRadians(45.0f), 800 / 600, 0.1f, 100.0f
        );
        int projectionLoc = _gl.GetUniformLocation(_program, "projection");
        _gl.UniformMatrix4(projectionLoc, 1, false, (float*)&projection);

        // _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);

        for (int i = 0; i < cubePositions.Length; i++)
        {
            Vector3 cube = cubePositions[i];
            // Matrix4x4 model = ;
            float angle = 20.0f * i;

            Matrix4x4 model = Matrix4x4.Identity 
                * Matrix4x4.CreateRotationY(convertToRadians(difference)) 
                * Matrix4x4.CreateRotationX(convertToRadians(difference))
                * Matrix4x4.CreateTranslation(cube);

            // Matrix4x4 model = Matrix4x4.Identity * Matrix4x4.CreateRotationX(convertToRadians(-55f));
            int modelLoc = _gl.GetUniformLocation(_program, "model");
            _gl.UniformMatrix4(modelLoc, 1, false, (float*)&model);

            _gl.DrawArrays(GLEnum.Triangles, 0, 36);
        }

    }

    private static float convertToRadians(float degrees)
    {
        return MathF.PI / 180f * degrees;
    }
}