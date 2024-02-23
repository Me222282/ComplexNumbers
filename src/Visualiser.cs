using System;
using Zene.Graphics;
using Zene.Structs;
using Zene.Windowing;

namespace maths
{
    public class Visualiser : Window
    {
        public Visualiser(int width, int height, string title, IExpression expression)
            : base(width, height, title)
        {
            _exp = expression;
            GenerateMap();
            _shader = BasicShader.GetInstance();
            
            _refCube = new DrawObject<Vector3, byte>(
                new Vector3[]
                {
                    new Vector3(-1, 1, 1), 
                    new Vector3(1, 1, 1),
                    new Vector3(1, -1, 1),
                    new Vector3(-1, -1, 1),

                    new Vector3(-1, 1, -1),
                    new Vector3(1, 1, -1),
                    new Vector3(1, -1, -1),
                    new Vector3(-1, -1, -1)
                }, new byte[]
                {
                    // Front
                    0, 3, 2,
                    2, 1, 0,

                    // Back
                    5, 6, 7,
                    7, 4, 5,

                    // Left
                    4, 7, 3,
                    3, 0, 4,

                    // Right
                    1, 2, 6,
                    6, 5, 1,

                    // Top
                    4, 0, 1,
                    1, 5, 4,

                    // Bottom
                    2, 3, 7,
                    7, 6, 2
                }, 1, 0, AttributeSize.D3, BufferUsage.DrawFrequent
            );
        }
        
        private DrawObject<Vector3, uint> _draw;
        private DrawObject<Vector3, byte> _refCube;
        
        private Vector2 _size = (10, 10);
        private double _precision = 0.1;
        
        private IExpression _exp;
        public IExpression Expression
        {
            get => _exp;
            set
            {
                _exp = value;
                GenerateMap();
            }
        }
        
        private Vector3 CameraPos = Vector3.Zero;

        private Radian _rotateX = Radian.Percent(0.5);
        private Radian _rotateY = 0;
        private Radian _rotateZ = 0;

        private IMatrix _rotationMatrix;

        private double _moveSpeed = 0.01;
        
        private BasicShader _shader;
        
        private void GenerateMap()
        {
            Vector2 end = _size / 2d;
            
            Vector2 v = -end;
            
            Vector3[,] map = new Vector3[(int)(_size.Y / _precision) + 1, (int)(_size.X / _precision) + 1];
            
            Vector2I pos = 0;
            while (v.Y <= end.Y)
            {
                while (v.X <= end.X)
                {
                    Complex comp = _exp.Calculate((Complex)v);
                    map[pos.Y, pos.X] = (v.X, comp.Modulus(), v.Y);
                    
                    pos.X++;
                    v.X += _precision;
                }
                
                v.X = -end.X;
                pos.X = 0;
                pos.Y++;
                v.Y += _precision;
            }
            
            CreateDO(map);
        }
        private unsafe void CreateDO(Vector3[,] map)
        {
            int w = map.GetLength(1);
            uint[] ind = new uint[(map.GetLength(0) - 1) * (w - 1) * 6];
            
            int i = 0;
            for (int y = 0; y < map.GetLength(0) - 1; y++)
            {
                for (int x = 0; x < w - 1; x++)
                {
                    ind[i] = (uint)((y * w) + x);
                    i++;
                    ind[i] = (uint)((y * w) + x + 1);
                    i++;
                    ind[i] = (uint)(((y + 1) * w) + x);
                    i++;
                    
                    ind[i] = (uint)(((y + 1) * w) + x);
                    i++;
                    ind[i] = (uint)(((y + 1) * w) + x + 1);
                    i++;
                    ind[i] = (uint)((y * w) + x + 1);
                    i++;
                }
            }
            
            fixed (Vector3* ptr = map)
            {
                _draw = new DrawObject<Vector3, uint>(
                    new ReadOnlySpan<Vector3>(ptr, map.Length), ind, 1, 0,
                    AttributeSize.D3, BufferUsage.DrawFrequent);
            }
        }

        protected override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);
            
            e.Context.Framebuffer.Clear(BufferBit.Colour | BufferBit.Depth);
            
            _rotationMatrix = Matrix3.CreateRotationY(_rotateY) * Matrix3.CreateRotationX(_rotateX);

            Vector3 cameraMove = new Vector3(0, 0, 0);

            if (this[Keys.A])       { cameraMove.X += _moveSpeed; }
            if (this[Keys.D])       { cameraMove.X -= _moveSpeed; }
            if (this[Keys.W])       { cameraMove.Z -= _moveSpeed; }
            if (this[Keys.S])       { cameraMove.Z += _moveSpeed; }
            if (this[Keys.Space])   { cameraMove.Y -= _moveSpeed; }
            if (this[Mods.Control]) { cameraMove.Y += _moveSpeed; }

            if (this[Keys.LeftShift])   { cameraMove *= 2; }
            if (this[Keys.RightShift])  { cameraMove *= 0.25; }
            if (this[Mods.Alt])         { cameraMove *= 4; }

            CameraPos += cameraMove * new Matrix3(_rotationMatrix);
            
            DrawManager.View = Matrix4.CreateTranslation(CameraPos) * Matrix4.CreateRotationY(_rotateY) *
                Matrix4.CreateRotationX(_rotateX) * Matrix4.CreateRotationZ(_rotateZ);
            
            _shader.ColourSource = ColourSource.UniformColour;
            _shader.Colour = ColourF.Orange;
            
            State.PolygonMode = PolygonMode.Line;
            e.Context.Shader = _shader;
            e.Context.Draw(_draw);
            
            e.Context.Draw(_refCube);
        }

        protected override void OnSizeChange(VectorIEventArgs e)
        {
            base.OnSizeChange(e);
            
            DrawManager.Projection = Matrix4.CreatePerspectiveFieldOfView(
                Radian.Degrees(60), e.X / (double)e.Y, 0.1, 3000);
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e[Keys.Escape])
            {
                Close();
                return;
            }
            if (e[Keys.Tab])
            {
                FullScreen = !FullScreen;
                return;
            }
            if (e[Keys.BackSpace])
            {
                CameraPos = Vector3.Zero;
                _rotateX = Radian.Percent(0.5);
                _rotateY = 0;
                _rotateZ = 0;
                _moveSpeed = 1;
                
                return;
            }
            if (e[Keys.M])
            {
                if (CursorMode != CursorMode.Normal)
                {
                    CursorMode = CursorMode.Normal;
                    return;
                }

                CursorMode = CursorMode.Disabled;
                return;
            }
        }
        private Vector2 _mouseLocation = Vector2.Zero;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (CursorMode != CursorMode.Disabled) { return; }

            if (new Vector2(e.X, e.Y) == _mouseLocation) { return; }

            double distanceX = e.X - _mouseLocation.X;
            double distanceY = e.Y - _mouseLocation.Y;

            _mouseLocation = new Vector2(e.X, e.Y);

            _rotateY += Radian.Degrees(distanceX * 0.1);
            _rotateX += Radian.Degrees(distanceY * 0.1);
        }
    }
}