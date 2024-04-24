using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE481_hw2
{
    public class Layer
    {
        #region Ctor
        public Layer(double centerOfGravity, double b, double h) 
        {
            CenterOfGravity = centerOfGravity; B = b; H = h;
        }

        #endregion

        #region Private Fields

        private double _CenterOfGravity;
        private double _B;
        private double _H;

        #endregion

        #region Public Properties
        public double CenterOfGravity { get => _CenterOfGravity; set => _CenterOfGravity = value; }
        public double B { get => _B; set => _B = value; }
        public double H { get => _H; set => _H = value; }
        public double Area { get => B * H; }

        #endregion
    }
}
