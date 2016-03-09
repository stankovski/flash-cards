using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace FlashCards.Core.Model
{
    public class StrokeData
    {
        public StrokeData()
        {
            InkPoints = new List<InkPoint>();
        }

        public StrokeData(InkStroke stroke)
        {
            if (stroke == null)
            {
                throw new ArgumentNullException(nameof(stroke));
            }
            DrawingAttributes = stroke.DrawingAttributes;
            PointTransform = stroke.PointTransform;
            InkPoints = new List<InkPoint>(stroke.GetInkPoints());
        }

        public InkDrawingAttributes DrawingAttributes { get; set; }
        
        public Matrix3x2 PointTransform { get; set; }

        public List<InkPoint> InkPoints { get; private set; }

        public InkStroke ToInkStroke()
        {
            InkStrokeBuilder strokeBuilder = new InkStrokeBuilder();
            strokeBuilder.SetDefaultDrawingAttributes(DrawingAttributes);
            return strokeBuilder.CreateStrokeFromInkPoints(InkPoints, PointTransform);
        }
    }
}
