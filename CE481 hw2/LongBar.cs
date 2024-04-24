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
        public LongBar(eVerticalLocation verticalLocation, eHorizontalLocation horizontalLocation, double depth, double secHeight, double uStrain, double uStrength, double yStrenght, double diameter)
        {
            VerticalLocation = verticalLocation; HorizontalLocation = horizontalLocation; UltimateStrain = uStrain; UltimateStrength = uStrength; YieldStrength = yStrenght; Diameter = diameter;

            if (VerticalLocation == eVerticalLocation.TopSteel)
            {
                Location = secHeight - depth;
            }
            else if ( VerticalLocation == eVerticalLocation.BottomSteel)
            {
                Location = depth;
            } // TODO else should give an error

        }
        #endregion

        #region Private Fields
        private eVerticalLocation _VerticalLocation;
        private eHorizontalLocation _HorizontalLocation; 
        private double _Location;
        private double _UltimateStrain;
        private double _UltimateStrength;
        private double _YieldStrength;
        private double _Diameter;


        #endregion

        #region Public Properties

        public eVerticalLocation VerticalLocation { get => _VerticalLocation; set => _VerticalLocation = value; }
        public eHorizontalLocation HorizontalLocation { get => _HorizontalLocation; set => _HorizontalLocation = value; }
        public double Location { get => _Location; set => _Location = value; }
        public double UltimateStrain { get => _UltimateStrain; set => _UltimateStrain = value; }
        public double UltimateStrength { get => _UltimateStrength; set => _UltimateStrength = value; }
        public double YieldStrength { get => _YieldStrength; set => _YieldStrength = value; }
        public double YieldStrain { get => YieldStrength / 200000; }
        public double Diameter { get => _Diameter; set => _Diameter = value; }
        public double Area { get => Math.PI * Math.Pow(Diameter, 2) / 4; }
        


        #endregion

        public enum eVerticalLocation
        {
            BottomSteel,
            TopSteel,
            None,
        }
        public enum eHorizontalLocation
        {
            OutsideSteel,
            InsideSteel,
            None,
        }
    }
}
