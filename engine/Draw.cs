using Silk.NET.OpenGL;

namespace gd.engine
{
    public static class Draw
    {
        public static unsafe void DrawShape(String shape, GL _gl)
        {
            float[] vertices = shape switch
            {
                "cube" => CubeVertices,
                "rectangle" => RectangleVertices,
                _ => []
            };

            fixed (float* buffer = vertices)
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buffer, BufferUsageARB.StaticDraw);
        }

        public static float[] CubeVertices =
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

        public static float[] RectangleVertices =
        {
            -1f, -0.5f, -0.5f,  0.0f, 0.0f,
            1f, -0.5f, -0.5f,  1.0f, 0.0f,
            1f,  0.5f, -0.5f,  1.0f, 1.0f,
            1f,  0.5f, -0.5f,  1.0f, 1.0f,
            -1f,  0.5f, -0.5f,  0.0f, 1.0f,
            -1f, -0.5f, -0.5f,  0.0f, 0.0f,

            -1f, -0.5f,  0.5f,  0.0f, 0.0f,
            1f, -0.5f,  0.5f,  1.0f, 0.0f,
            1f,  0.5f,  0.5f,  1.0f, 1.0f,
            1f,  0.5f,  0.5f,  1.0f, 1.0f,
            -1f,  0.5f,  0.5f,  0.0f, 1.0f,
            -1f, -0.5f,  0.5f,  0.0f, 0.0f,

            -1f,  0.5f,  0.5f,  1.0f, 0.0f,
            -1f,  0.5f, -0.5f,  1.0f, 1.0f,
            -1f, -0.5f, -0.5f,  0.0f, 1.0f,
            -1f, -0.5f, -0.5f,  0.0f, 1.0f,
            -1f, -0.5f,  0.5f,  0.0f, 0.0f,
            -1f,  0.5f,  0.5f,  1.0f, 0.0f,

            1f,  0.5f,  0.5f,  1.0f, 0.0f,
            1f,  0.5f, -0.5f,  1.0f, 1.0f,
            1f, -0.5f, -0.5f,  0.0f, 1.0f,
            1f, -0.5f, -0.5f,  0.0f, 1.0f,
            1f, -0.5f,  0.5f,  0.0f, 0.0f,
            1f,  0.5f,  0.5f,  1.0f, 0.0f,

            -1f, -0.5f, -0.5f,  0.0f, 1.0f,
            1f, -0.5f, -0.5f,  1.0f, 1.0f,
            1f, -0.5f,  0.5f,  1.0f, 0.0f,
            1f, -0.5f,  0.5f,  1.0f, 0.0f,
            -1f, -0.5f,  0.5f,  0.0f, 0.0f,
            -1f, -0.5f, -0.5f,  0.0f, 1.0f,

            -1f,  0.5f, -0.5f,  0.0f, 1.0f,
            1f,  0.5f, -0.5f,  1.0f, 1.0f,
            1f,  0.5f,  0.5f,  1.0f, 0.0f,
            1f,  0.5f,  0.5f,  1.0f, 0.0f,
            -1f,  0.5f,  0.5f,  0.0f, 0.0f,
            -1f,  0.5f, -0.5f,  0.0f, 1.0f
        };
    }
}