using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LMI.Gocator.Tools
{
    class RectangleElement:IGraphicElement
    {

        /// <summary>
        /// (x,y) TopLeft  width height
        /// </summary>
        float _x;
        float _y;
        float _width;
        float _height;
        Pen _rectPen;
        Brush _rectBrush;
        GraphicsElementType type = GraphicsElementType.Rectangle;

        public float X { get => _x; set => _x = value; }
        public float Y { get => _y; set => _y = value; }
        public float Width { get => _width; set => _width = value; }
        public float Height { get => _height; set => _height = value; }
        public Pen RectPen { get => _rectPen; set => _rectPen = value; }
        public Brush RectBrush { get => _rectBrush; set => _rectBrush = value; }

        GraphicsElementType IGraphicElement.GetGraphicsType()
        {
            return this.type;
        }

        public RectangleF GetRect()
        {
            return new RectangleF(X, Y, Width, Height);
        }

        public RectangleElement()
        {

        }

        public RectangleElement(RectangleF rect)
        {
            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;
        }
        
    }
}
