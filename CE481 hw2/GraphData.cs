using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE481_hw2
{
    public class GraphData
    {
        #region Ctor
        public GraphData() 
        {
            X = new List<double>();
            Y = new List<double>();
        }
        #endregion

        #region Private Fields
        private List<double> _X;
        private List<double> _Y;

        #endregion

        #region Public Properties
        public List<double> X { get => _X; set => _X = value; }
        public List<double> Y { get => _Y; set => _Y = value; }

        #endregion

    }
}
