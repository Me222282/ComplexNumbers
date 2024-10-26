using System;
using System.Collections.Generic;
using Zene.Graphics;
using Zene.Structs;
using Zene.Windowing;

namespace maths
{
    public enum Mode
    {
        Mag,
        Real,
        I,
        Sum,
        Arg
    }
    
    public class Visualiser : Window
    {
        public Visualiser(int width, int height, string title, IExpression expression)
            : this(width, height, title, new Graph(expression))
        {
            
        }
        public Visualiser(int width, int height, string title, IExpression y, IExpression x)
            : this(width, height, title, new Graph(y, x))
        {
            
        }
        public Visualiser(int width, int height, string title, Graph expression)
            : base(width, height, title)
        {
            _exp = expression;
            GenerateMap();
            _shader = BasicShader.GetInstance();
            _lighting = new LightingShader(1, 1);
            _lighting.SetLight(0, new Light(ColourF3.White, ColourF3.Zero, 0.0014, 0.000007, _lightDir, true));
            _lighting.ShadowDither = false;
            
            _shadows = new Zene.Graphics.Z3D.ShadowMapper(10000, 10000);
            
            _refCube = CreateDrawObject(
                new Vector3[]
                {
                    new Vector3(-0.5, 0.5, 0.5), 
                    new Vector3(0.5, 0.5, 0.5),
                    new Vector3(0.5, -0.5, 0.5),
                    new Vector3(-0.5, -0.5, 0.5),

                    new Vector3(-0.5, 0.5, -0.5),
                    new Vector3(0.5, 0.5, -0.5),
                    new Vector3(0.5, -0.5, -0.5),
                    new Vector3(-0.5, -0.5, -0.5)
                }, 1, new uint[]
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
                });
        }
        
        private DrawObject<Vector3, uint> _draw;
        private DrawObject<Vector3, uint> _refCube;
        
        private Vector2 _size = (10, 10);
        private int _dp = 1;
        private double _precision = 0.1;
        public double Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                _dp = (int)Math.Ceiling(-Math.Log10(_precision));
            }
        }
        
        private Graph _exp;
        public Graph Expression
        {
            get => _exp;
            set
            {
                _exp = value;
                GenerateMap();
            }
        }
        
        private Vector3 _cameraPos = Vector3.Zero;

        private Radian _rotateX = Radian.Percent(0.5);
        private Radian _rotateY = 0;
        private Radian _rotateZ = 0;

        private IMatrix _rotationMatrix;

        private double _moveSpeed = 0.05;
        private bool _gradColour = false;
        private double _fov = 80d;
        private double _renderDist = 3000d;
        
        private BasicShader _shader;
        private LightingShader _lighting;
        private Zene.Graphics.Z3D.ShadowMapper _shadows;
        private Vector3 _lightDir = (-0.5, -1d, -0.5);
        private double _dpSize = 15d;
        private bool _doLighting = false;
        private bool _lightRotate = false;
        private readonly IMatrix _lightRotation = Matrix3.CreateRotationX(Radian.Percent(-0.000125)) * Matrix3.CreateRotationZ(Radian.Percent(0.000125));
        private bool _matInv = true;
        
        private Mode _mode = Mode.Mag;
        private PolygonMode _poly = PolygonMode.Line;
        private double Round(double v) => Math.Round(v, _dp, MidpointRounding.AwayFromZero);
        private void GenerateMap()
        {
            if (_gradColour)
            {
                GenerateMapColour();
                return;
            }
            
            Vector2 end = _size / 2d;
            
            Vector2 v = -end;
            
            Vector3[,] map = new Vector3[(int)(_size.Y / _precision) + 1, (int)(_size.X / _precision) + 1];
            
            Vector2I pos = 0;
            while (Round(v.Y) <= end.Y)
            {
                while (Round(v.X) <= end.X)
                {   
                    map[pos.Y, pos.X] = _exp.Calculate((Complex)v, _mode);
                    
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
                    ind[i] = (uint)(((y + 1) * w) + x);
                    i++;
                    ind[i] = (uint)((y * w) + x + 1);
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
                _draw = CreateDrawObject(new ReadOnlySpan<Vector3>(ptr, map.Length), 1, ind);
            }
        }
        
        private unsafe void GenerateMapColour()
        {
            Vector2 end = _size / 2d;
            Vector2 v = -end;
            
            Vector3[,] map = new Vector3[(int)(_size.Y / _precision) + 1, ((int)(_size.X / _precision) + 1) * 2];
            
            Vector2I pos = 0;
            while (Round(v.Y) <= end.Y)
            {
                while (Round(v.X) <= end.X)
                {
                    map[pos.Y, pos.X * 2] = _exp.Calculate((Complex)v, _mode);
                    
                    map[pos.Y, pos.X * 2 + 1] = (1d, (1 + Math.Sin(pos.Y * _precision * 10d)) / 2, (1 + Math.Cos(pos.X * _precision * 10d)) / 2);
                    
                    pos.X++;
                    v.X += _precision;
                }
                
                v.X = -end.X;
                pos.X = 0;
                pos.Y++;
                v.Y += _precision;
            }
            
            int w = map.GetLength(1) / 2;
            uint[] ind = new uint[(map.GetLength(0) - 1) * (w - 1) * 6];
            
            int i = 0;
            for (int y = 0; y < map.GetLength(0) - 1; y++)
            {
                for (int x = 0; x < w - 1; x++)
                {
                    ind[i] = (uint)((y * w) + x);
                    i++;
                    ind[i] = (uint)(((y + 1) * w) + x);
                    i++;
                    ind[i] = (uint)((y * w) + x + 1);
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
                _draw = CreateDrawObject(new ReadOnlySpan<Vector3>(ptr, map.Length), 2, ind);
                _draw.AddAttribute(ShaderLocation.Colour, 1, AttributeSize.D3);
            }
        }
        
        private DrawObject<Vector3, uint> CreateDrawObject(ReadOnlySpan<Vector3> verts, byte vs, uint[] inds)
        {   
            Zene.Graphics.Z3D.Object3D.AddNormals(verts, vs, inds, out List<Vector3> nv, out List<uint> ni);
            DrawObject<Vector3, uint> dO = new DrawObject<Vector3, uint>(
                nv.ToArray(), ni.ToArray(), (byte)(vs + 1), 0,
                AttributeSize.D3, BufferUsage.DrawFrequent);
            dO.AddAttribute(ShaderLocation.Normal, vs, AttributeSize.D3);
            return dO;
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

            if (this[Keys.LeftShift])   { cameraMove *= 4; }
            if (this[Keys.RightShift])  { cameraMove *= 0.25; }
            if (this[Mods.Alt])         { cameraMove *= 10; }
            if (this[Keys.Q])   { cameraMove *= 100; }

            _cameraPos += cameraMove * new Matrix3(_rotationMatrix);
            
            _lighting.CameraPosition = -_cameraPos;
            _lighting.DrawLighting = true;
            _lighting.ColourSource = ColourSource.UniformColour;
            _lighting.Material = _matInv ?
                new Material(Material.Source.Default, Material.Source.None, Shine.None) :
                new Material(new ColourF3(1.3f, 1.2f, 1.1f), new ColourF3(0.6f, 0.3f, 0.5f), Shine.XL);
            _lighting.AmbientLight = new Colour(12, 12, 15);
            
            e.Context.View = Matrix4.CreateTranslation(_cameraPos) * Matrix4.CreateRotationY(_rotateY) *
                Matrix4.CreateRotationX(_rotateX) * Matrix4.CreateRotationZ(_rotateZ);
            
            if (_doLighting)
            {
                if (_lightRotate)
                {
                    _lightDir *= _lightRotation;
                }
                
                _shadows.Projection = Matrix4.CreateOrthographic(_dpSize, _dpSize, 10000d, -10000d);
                _shadows.View = Matrix4.LookAt(Vector3.Zero, _lightDir, (0d, 1d, 0d));
                e.Context.RenderState.PolygonMode = PolygonMode.Fill;
                _shadows.Framebuffer.Clear(BufferBit.Depth);
                //_shadows.Draw(_refCube);
                Render(_shadows, _lighting);
                
                _lighting.ShadowMap = _shadows.DepthMap;
                _lighting.LightSpaceMatrix = _shadows.View * _shadows.Projection;
                
                _lighting.SetLightPosition(0, (_lightDir, 0d));
            }
            
            e.Context.DepthState.Testing = true;
            IBasicShader ibs = _shader;
            if (_doLighting)
            {
                ibs = _lighting;
            }
            e.Context.Shader = ibs;
            e.Context.RenderState.PolygonMode = _poly;
            Render(e.Context, ibs);
        }
        private void Render(IDrawingContext dc, IBasicShader shader)
        {
            shader.Colour = ColourF.Orange;
            
            if (_gradColour)
            {
                shader.ColourSource = ColourSource.AttributeColour;
            }
            else
            {
                shader.ColourSource = ColourSource.UniformColour;
            }
            
            dc.Model = Matrix4.CreateScale(1d, -1d, 1d);
            dc.Draw(_draw);
            
            shader.ColourSource = ColourSource.UniformColour;
            shader.Colour = ColourF.Aqua;
            dc.Draw(_refCube);
        }
        
        protected override void OnSizeChange(VectorIEventArgs e)
        {
            base.OnSizeChange(e);
            
            DrawContext.Projection = Matrix4.CreatePerspectiveFieldOfView(
                Radian.Degrees(_fov), e.X / (double)e.Y, 0.1, _renderDist);
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
                _cameraPos = Vector3.Zero;
                _rotateX = Radian.Percent(0.5);
                _rotateY = 0;
                _rotateZ = 0;
                
                return;
            }
            if (e[Keys.M] && e[Mods.Shift])
            {
                _matInv = !_matInv;
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
            if (e[Keys.V] && e[Mods.Shift])
            {
                _mode--;
                if (_mode < 0)
                {
                    _mode = Mode.Arg;
                }
                
                GenerateMap();
                return;
            }
            if (e[Keys.V])
            {
                _mode++;
                if (_mode > Mode.Arg)
                {
                    _mode = Mode.Mag;
                }
                
                GenerateMap();
                return;
            }
            if (e[Keys.F])
            {
                if (_poly == PolygonMode.Fill)
                {
                    _poly = PolygonMode.Line;
                    return;
                }
                
                _poly = PolygonMode.Fill;
                return;
            }
            if (e[Keys.Equal] && this[Keys.B])
            {
                _dpSize *= 2d;
                GenerateMap();
                return;
            }
            if (e[Keys.Minus] && this[Keys.B])
            {
                _dpSize /= 2d;
                GenerateMap();
                return;
            }
            if (e[Keys.Equal] && e[Mods.Shift])
            {
                _size *= 2d;
                _dpSize *= 2d;
                GenerateMap();
                return;
            }
            if (e[Keys.Minus] && e[Mods.Shift])
            {
                _size /= 2d;
                _dpSize /= 2d;
                GenerateMap();
                return;
            }
            if (e[Keys.Equal])
            {
                Precision *= 0.5;
                GenerateMap();
                return;
            }
            if (e[Keys.Minus])
            {
                Precision /= 0.5;
                GenerateMap();
                return;
            }
            if (e[Keys.C])
            {
                _gradColour = !_gradColour;
                GenerateMap();
                return;
            }
            if (e[Keys.L])
            {
                _doLighting = !_doLighting;
                return;
            }
            if (e[Keys.R])
            {
                _lightRotate = !_lightRotate;
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
            
            if (this[Keys.RightAlt])
            {
                distanceX *= 0.1;
                distanceY *= 0.1;
            }
            
            _rotateY += Radian.Degrees(distanceX * 0.1);
            _rotateX += Radian.Degrees(distanceY * 0.1);
        }
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            
            if (this[Mods.Shift])
            {
                _renderDist *= e.DeltaY > 0 ? 2d : 0.5;
            }
            else
            {
                _fov *= e.DeltaY > 0 ? 0.9 : 1.1;
                
                if (_fov > 180d) { _fov = 180d; }
                if (_fov <= 0d) { _fov = 0.5; }
            }
            
            DrawContext.Projection = Matrix4.CreatePerspectiveFieldOfView(
                Radian.Degrees(_fov), Size.X / (double)Size.Y, 0.1, _renderDist);
        }
    }
}