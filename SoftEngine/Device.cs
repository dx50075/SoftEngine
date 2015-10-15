using SharpDX;
using System.Drawing;

namespace SoftEngine
{
    public class Device
    {
        private Bitmap m_backBuffer;

        public Device(int width,int height)
        {
            m_backBuffer = new Bitmap(width, height);
        }

        public void Clear()
        {
           
            using(System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(m_backBuffer))
            {
                g.Clear(System.Drawing.Color.Black);
            }
        }

        public void Present(System.Drawing.Graphics g)
        {
            if(null != g)
                g.DrawImage(m_backBuffer, 0, 0);
        }

        public void PutPixel(int x,int y,Color4 color)
        {
            System.Drawing.Color c = System.Drawing.Color.FromArgb((int)color.Alpha, (int)color.Red, (int)color.Green, (int)color.Blue);
            m_backBuffer.SetPixel(x, y, c);
        }

        public Vector2 Project(Vector3 coord,Matrix transMat)
        {
            var point = Vector3.TransformCoordinate(coord, transMat);
            var x = point.X * m_backBuffer.Width + m_backBuffer.Width / 2f;
            var y = -point.Y * m_backBuffer.Height + m_backBuffer.Height / 2f;

            return new Vector2(x, y);
        }

        public void DrawPoint(Vector2 point)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < m_backBuffer.Width && point.Y < m_backBuffer.Height)
                PutPixel((int)point.X, (int)point.Y, new Color4(1.0f, 1.0f, 0.0f, 1.0f));
        }

        public void DrawLine(Vector2 p1,Vector2 p2)
        {
            var dist = (p1 - p2).Length();
            if (dist < 2)
                return;
            Vector2 mid = p1 + (p2 - p1) / 2;
            DrawPoint(mid);
            DrawLine(p1, mid);
            DrawLine(mid, p2);
        }
        public void Render(Camera camera,params Mesh[] meshes)
        {
            var viewMatrix = Matrix.LookAtLH(camera.Postion, camera.Target, Vector3.UnitY);
            var projectionMatrx = Matrix.PerspectiveFovRH(0.78f, (float)m_backBuffer.Width / m_backBuffer.Height,
                0.01f, 1.0f);

            foreach(Mesh mesh in meshes)
            {
                var worldMatrix = Matrix.RotationYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z) *
                    Matrix.Translation(mesh.Postion);

                var transMat = worldMatrix * viewMatrix * projectionMatrx;
                foreach(var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var pixelA = Project(vertexA, transMat);
                    var pixelB = Project(vertexB, transMat);
                    var pixelC = Project(vertexC, transMat);
                    DrawLine(pixelA, pixelB);
                    DrawLine(pixelB, pixelC);
                    DrawLine(pixelC, pixelA);
                }
            }
        }
    }
}
