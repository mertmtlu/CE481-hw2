using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE481_hw2
{
    public class LongBar
    {
        // Section origin is left bottom corner

        #region
        public LongBar(Type type, double conCover, double secHeight, double uStrain, double uStrength, double yStrain, double yStrenght)
        {
            BarType = type; UltimateStrain = uStrain; UltimateStrength = uStrength; YieldStrain = yStrain; YieldStrength = yStrenght;

            //if (BarType == Top)

        }
        #endregion

        #region Private Fields
        private Type _BarType;
        private double _Location;
        private double _UltimateStrain;
        private double _UltimateStrength;
        private double _YieldStrain;
        private double _YieldStrength;
        private double _Diameter;


        #endregion

        #region Public Properties

        public Type BarType { get => _BarType; set => _BarType = value; }
        public double Location { get => _Location; set => _Location = value; }
        public double UltimateStrain { get => _UltimateStrain; set => _UltimateStrain = value; }
        public double UltimateStrength { get => _UltimateStrength; set => _UltimateStrength = value; }
        public double YieldStrength { get => _YieldStrength; set => _YieldStrength = value; }
        public double YieldStrain { get => _YieldStrain; set => _YieldStrain = value; }
        public double Diameter { get => _Diameter; set => _Diameter = value; }
        

        #endregion

        public enum Type
        {
            BottomSteel,
            TopSteel,
            None,
        }
    }
}
