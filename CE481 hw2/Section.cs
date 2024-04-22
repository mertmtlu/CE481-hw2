using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static CE481_hw2.LongBar;

namespace CE481_hw2
{
    public class Section
    {
        #region ctor
        public Section()
        {
            SteelList = new List<LongBar>();

            Confined = false;
            ConfinementDiameter = 0;
            ConfinementYieldStrength = 0;
        }
        #endregion

        #region Private Fields

        private double _Height;// H
        private double _BaseLength;// b
        private double _UnconfinedStrength;// fco
        private List<LongBar> _SteelList;
        private double conCover;
        private bool _Confined;
        private double _ConfinementDiameter;// mm
        private double _ConfinementYieldStrength;// Mpa
        private double _UltimateStrain;
        private double _ConfinementSpacing;


        

        #endregion

        #region Public Properties
        
        public double Height { get => _Height; set => _Height = value; }
        public double BaseLength { get => _BaseLength; set => _BaseLength = value; }
        public double UnconfinedStrength { get => _UnconfinedStrength; set => _UnconfinedStrength = value; }
        public List<LongBar> SteelList { get => _SteelList; set => _SteelList = value; }
        public double ConCover { get => conCover; set => conCover = value; }
        public bool Confined { get => _Confined; set => _Confined = value; }
        public double ConfinementDiameter { get => _ConfinementDiameter; set => _ConfinementDiameter = value; }
        public double ConfinementYieldStrength { get => _ConfinementYieldStrength; set => _ConfinementYieldStrength = value; }
        public double UltimateStrain { get => _UltimateStrain; set => _UltimateStrain = value; }
        public double B0 { get => BaseLength - ConfinementDiameter - 2 * ConCover; }
        public double H0 { get => Height - ConfinementDiameter - 2 * ConCover; }
        public double ConcreteElasticModulus { get => 5000 * Math.Sqrt(UnconfinedStrength); }
        public double ConfinementSpacing { get => _ConfinementSpacing; set => _ConfinementSpacing = value; }


        #endregion

        #region Methods

        #region Section Properties
        public void AddSteelLayer(eVerticalLocation verticalLocation, int numOfSteel, bool differentInsideDia, double insideDia, double outsideDia, double uStrain, double uStrength, double yStrain, double yStrenght)
        {
            if (differentInsideDia == false)
            {
                outsideDia = insideDia;
            }

            for (int i = 0; i < 2; i++)
            {
                SteelList.Add(new LongBar(verticalLocation, eHorizontalLocation.OutsideSteel, ConCover + ConfinementDiameter + outsideDia / 2, Height, uStrain, uStrength, yStrain, yStrenght, outsideDia));
            }
            for (int i = 0; i < numOfSteel - 2; i++)
            {
                SteelList.Add(new LongBar(verticalLocation, eHorizontalLocation.InsideSteel, ConCover + ConfinementDiameter + insideDia / 2, Height, uStrain, uStrength, yStrain, yStrenght, insideDia));
            }
        }

        public void UpdateConfinement(bool confined, double confinementDia, double confinementYieldStrength, double confinementSpacing)
        {
            Confined = confined;
            if (confined)
            {
                ConfinementYieldStrength = confinementYieldStrength;
                ConfinementDiameter = confinementDia;
                ConfinementSpacing = confinementSpacing;
            }
            else
            {
                ConfinementYieldStrength = 0;
                ConfinementDiameter = 0;
                ConfinementSpacing = 0;
            }


        }

        public void UpdateSectionProperties(double height, double baseLength, double unconfinedStrength)
        {
            Height = height;
            BaseLength = baseLength;
            UnconfinedStrength = unconfinedStrength;
        }

        #endregion

        #region Overloaded GetBar Method
        public List<LongBar> GetBars(eVerticalLocation typeOfSteel)
        {
            var result = new List<LongBar>();

            for (int i = 0; i < SteelList.Count; i++)
            {
                if(SteelList[i].VerticalLocation == typeOfSteel)
                {
                    result.Add(SteelList[i]);
                }
            }

            return result;
        }

        public List<LongBar> GetBars(eHorizontalLocation typeOfSteel)
        {
            var result = new List<LongBar>();

            for (int i = 0; i < SteelList.Count; i++)
            {
                if (SteelList[i].HorizontalLocation == typeOfSteel)
                {
                    result.Add(SteelList[i]);
                }
            }

            return result;
        }

        public List<LongBar> GetBars(eVerticalLocation verTypeOfSteel, eHorizontalLocation horTypeOfSteel)
        {
            var result = new List<LongBar>();

            for (int i = 0; i < SteelList.Count; i++)
            {
                if (SteelList[i].VerticalLocation == verTypeOfSteel && SteelList[i].HorizontalLocation == horTypeOfSteel)
                {
                    result.Add(SteelList[i]);
                }
            }

            return result;
        }

        #endregion

        #region Concrete Methods
        public double GetAiSquared(eVerticalLocation type)
        {
            var numberOfSpaces = GetBars(type).Count - 1;
            var outsideDia = GetBars(type, eHorizontalLocation.OutsideSteel)[0].Diameter;

            double centerToCenterDist = (BaseLength - 2 * conCover - 2 * ConfinementDiameter - outsideDia) / numberOfSpaces;

            return numberOfSpaces * Math.Pow(centerToCenterDist,2);
        }

        public double GetSumAi()
        {
            double topSteelOutsideDia = GetBars(eVerticalLocation.TopSteel, eHorizontalLocation.OutsideSteel)[0].Diameter;
            double bottomSteelOutsideDia = GetBars(eVerticalLocation.BottomSteel, eHorizontalLocation.OutsideSteel)[0].Diameter;
            double AiySquared = Math.Pow(Height - 2 * conCover - 2 * ConfinementDiameter - (topSteelOutsideDia + bottomSteelOutsideDia) / 2, 2);

            return AiySquared + GetAiSquared(eVerticalLocation.BottomSteel) + GetAiSquared(eVerticalLocation.TopSteel);
        }

        public double GetGamaC(bool layerIsConfined) // should be using 2 times
        {
            double result = 0;

            if (Confined && layerIsConfined)
            {
                double b0 = B0;
                double h0 = H0;
                
                double longBarArea = 0;
                for (int i = 0; i < SteelList.Count; i++)
                {
                    longBarArea += SteelList[i].Area;
                }

                double ke = (1 - GetSumAi() / (6 * b0 * h0)) * (1 - ConfinementSpacing / (2 * b0)) * (1 - ConfinementSpacing / (2 * h0)) * (1 - longBarArea / (b0 * h0));

                double rho = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (b0 * h0);// TODO Using only one stirrup. Therefore, this section has only two legs. Moreover, rhoX = rhoY = rho.

                double fex = ke * rho * UnconfinedStrength;
                double fey = ke * rho * UnconfinedStrength;

                double fe = (fex + fey) / 2;

                result = 2.254 * Math.Sqrt(1 + 7.94 * fe / UnconfinedStrength) - 2 * fe / UnconfinedStrength;

            }
            else 
            {
                result = 1; // TODO check later if gama is 1 in not confined zone
            }

            return result;
        }

        public double GetFCC(bool layerIsConfined)
        {
            return GetGamaC(layerIsConfined) * UnconfinedStrength;
        }

        public double CalculateFC(double strainC, bool layerIsConfined)
        {
            double strainCO = 0.002;
            double strainCC = strainCO*(1+5*(GetGamaC(layerIsConfined)-1));
            double x = strainC / strainCC;

            double Esec = GetFCC(layerIsConfined) / strainCC;
            double r = ConcreteElasticModulus / (ConcreteElasticModulus - Esec);


            return GetFCC(layerIsConfined) * r / (r - 1 + Math.Pow(x, r));
        }

        #endregion

        #region Ultimate Strain Methods

        public double GetAlpha()
        {
            double result = 0; // If not confined should be zero.

            if (Confined)
            {
                double b0 = B0;
                double h0 = H0;

                double longBarArea = 0;
                for (int i = 0; i < SteelList.Count; i++)
                {
                    longBarArea += SteelList[i].Area;
                }

                result = (1 - GetSumAi() / (6 * b0 * h0)) * (1 - ConfinementSpacing / (2 * b0)) * (1 - ConfinementSpacing / (2 * h0));

            }

            return result;
        }

        public double GetFCE()
        {
            return 0; // TODO 
        }

        public double GetOmegaWE()
        {
            double b0 = B0;
            double h0 = H0;
            double rho = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (b0*ConfinementSpacing);

            return GetAlpha() * rho * ConfinementYieldStrength / GetFCE();
        }

        public double GetUltimateStrain()
        {
            if (Confined)
            {
                double ultimateStrain = 0.0035 + 0.04 * Math.Sqrt(GetOmegaWE());

                if (ultimateStrain > 0.018)
                {
                    ultimateStrain = 0.018;
                }

                return ultimateStrain;
            }
            else
            {
                return 0.0035;
            }
        }

        #endregion




        #endregion

    }
}
