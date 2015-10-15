using SharpDX;

namespace SoftEngine
{
    public struct Face
    {
        public int A;
        public int B;
        public int C;
    }
    public class Mesh
    {
        public string Name { get; set; }
        public Vector3[] Vertices { get; private set; }

        public Face[] Faces { get; set; }
        public Vector3 Postion { get; set; }
        public Vector3 Rotation { get; set; }

        public Mesh(string name,int verticesCount,int faceCount)
        {
            Vertices = new Vector3[verticesCount];
            Faces = new Face[faceCount];
            Name = name;
        }
    }
}
