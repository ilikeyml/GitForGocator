using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LMI.Gocator.Tools
{

    /// <summary>
    /// Graphics elements group
    /// </summary>
    
    class GraphicsGroup
    {
        List<IGraphicElement> _graphicElementList;

        public List<IGraphicElement> GraphicElementList { get => _graphicElementList; set => _graphicElementList = value; }

        #region public function        

        static Pen defaultPen = new Pen(Color.Red);

        public void GraphicsGroupDraw( Graphics g)
        {
            if(this.GraphicElementList.Count > 0)
            {
                foreach (var item in this.GraphicElementList)
                {
                    GraphicsElementType type = item.GetGraphicsType();
                    switch (type)
                    {
                        case GraphicsElementType.None:
                            break;
                        case GraphicsElementType.Line:
                            LineElement _line = item as LineElement;
                            g.DrawLine(_line.LinePen, _line.StartPoint, _line.EndPoint);
                            break;
                        case GraphicsElementType.Rectangle:
                            RectangleElement _rect = item as RectangleElement;
                            g.FillRectangle(_rect.RectBrush, _rect.GetRect());
                            g.DrawRectangle(_rect.RectPen, _rect.X,_rect.Y,_rect.Width,_rect.Height);                           
                            break;
                        default: break;
                    }
                }

            }
        }

        #endregion

        #region Color
        Color RandomColorGenerator()
        {
            Random random = new Random();
            return Color.FromArgb(128, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
        #endregion

    }
}
