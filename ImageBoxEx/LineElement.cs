using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LMI.Gocator.Tools
{

    /// <summary>
    /// Start Point && EndPoint
    /// </summary>
    class LineElement:IGraphicElement

    {
        PointF _startPoint;
        PointF _endPoint;
        GraphicsElementType type = GraphicsElementType.Line;
        Pen _linePen;
        Brush _lineBrush;

        public PointF StartPoint { get => _startPoint; set => _startPoint = value; }
        public PointF EndPoint { get => _endPoint; set => _endPoint = value; }
        public Pen LinePen { get => _linePen; set => _linePen = value; }
        public Brush LineBrush { get => _lineBrush; set => _lineBrush = value; }

        GraphicsElementType IGraphicElement.GetGraphicsType()
        {
            return this.type;
        }

        public LineElement()
        {

        }

        public LineElement(PointF pointS, PointF pointE)
        {
            this.StartPoint = pointS;
            this.EndPoint = pointE;
        }

    }
}
