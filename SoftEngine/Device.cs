using SharpDX;
using System.Drawing;
using System;
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
            int a = (int)color.Alpha * 255;
            int r = (int)color.Red * 255;
            int g = (int)color.Green * 255;
            int b = (int)color.Blue * 255;
            System.Drawing.Color c = System.Drawing.Color.FromArgb(a,r,g,b);
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
                PutPixel((int)point.X, (int)point.Y, new Color4(1.0f, 0.0f, 0.0f, 1.0f));
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

        public void BresenhamDrawLine(Vector2 p0,Vector2 p1)
        {
            int x0 = (int)p0.X;
            int y0 = (int)p0.Y;
            int x1 = (int)p1.X;
            int y1 = (int)p1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;
            while(true)
            {
                DrawPoint(new Vector2(x0, y0));
                if (x0 == x1 && y0 == y1)
                    break;
                var e2 = 2 * err;
                if(e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if(e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void Render(Camera camera,params Mesh[] meshes)
        {
            var viewMatrix = Matrix.LookAtLH(camera.Postion, camera.Target, Vector3.UnitY);
            var projectionMatrx = Matrix.PerspectiveFovLH(0.78f, (float)m_backBuffer.Width / m_backBuffer.Height,
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
                    BresenhamDrawLine(pixelA, pixelB);
                    BresenhamDrawLine(pixelB, pixelC);
                    BresenhamDrawLine(pixelC, pixelA);
                }
            }
        }
    }
}
