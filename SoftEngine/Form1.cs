using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftEngine
{
    public partial class Form1 : Form
    {
        private Device device;
        private Mesh mesh;
        private Camera camera;
        public Form1()
        {
           
            InitializeComponent();
            Init();
        }

        void Init()
        {
            device = new Device(this.panel2.Width, this.panel2.Height);

            mesh = new Mesh("Cube", 8,12);
            mesh.Vertices[0] = new SharpDX.Vector3(-1, 1, 1);
            mesh.Vertices[1] = new SharpDX.Vector3(1, 1, 1);
            mesh.Vertices[2] = new SharpDX.Vector3(-1, -1, 1);
            mesh.Vertices[3] = new SharpDX.Vector3(1, -1, 1);
            mesh.Vertices[4] = new SharpDX.Vector3(-1, 1, -1);
            mesh.Vertices[5] = new SharpDX.Vector3(1, 1, -1);
            mesh.Vertices[6] = new SharpDX.Vector3(1, -1, -1);
            mesh.Vertices[7] = new SharpDX.Vector3(-1, -1, -1);

            mesh.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            mesh.Faces[1] = new Face { A = 1, B = 2, C = 3 };
            mesh.Faces[2] = new Face { A = 1, B = 3, C = 6 };
            mesh.Faces[3] = new Face { A = 1, B = 5, C = 6 };
            mesh.Faces[4] = new Face { A = 0, B = 1, C = 4 };
            mesh.Faces[5] = new Face { A = 1, B = 4, C = 5 };

            mesh.Faces[6] = new Face { A = 2, B = 3, C = 7 };
            mesh.Faces[7] = new Face { A = 3, B = 6, C = 7 };
            mesh.Faces[8] = new Face { A = 0, B = 2, C = 7 };
            mesh.Faces[9] = new Face { A = 0, B = 4, C = 7 };
            mesh.Faces[10] = new Face { A = 4, B = 5, C = 6 };
            mesh.Faces[11] = new Face { A = 4, B = 6, C = 7 };

            camera = new Camera();
            camera.Postion = new SharpDX.Vector3(0, 0, 10f);
            camera.Target = SharpDX.Vector3.Zero;
        }
        private void MyPaint(object sender, PaintEventArgs e)
        {
            device.Clear();
            mesh.Rotation = new SharpDX.Vector3(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);
            device.Render(camera, mesh);
            device.Present(e.Graphics);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.panel2.Invalidate();
        }
    }

}
