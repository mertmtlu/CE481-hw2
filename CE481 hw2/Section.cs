using DevExpress.Charts.Heatmap.Native;
using DevExpress.CodeParser;
using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static CE481_hw2.LongBar;

namespace CE481_hw2
{
    public class Section
    {
        #region Ctor
        public Section()
        {
            SteelList = new List<LongBar>();

            Confined = false;
            ConfinementDiameter = 0;
            ConfinementYieldStrength = 0;
        }
        #endregion

        #region Private Fields

        private double _Height;// H in mm
        private double _BaseLength;// b in mm
        private double _UnconfinedStrength;// fco in MPa
        private List<LongBar> _SteelList;
        private double conCover; // mm
        private bool _Confined;
        private double _ConfinementDiameter;// mm
        private double _ConfinementYieldStrength;// MPa
        private double _UltimateStrain;
        private double _ConfinementSpacing; // mm


        

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

        #region Public Methods

        #region Section Properties
        public void AddSteelLayer(eVerticalLocation verticalLocation, int numOfSteel, bool differentInsideDia, double insideDia, double outsideDia, double uStrain, double uStrength, double yStrenght)
        {
            if (differentInsideDia == false)
            {
                outsideDia = insideDia;
            }

            for (int i = 0; i < 2; i++)
            {
                SteelList.Add(new LongBar(verticalLocation, eHorizontalLocation.OutsideSteel, ConCover + ConfinementDiameter + outsideDia / 2, Height, uStrain, uStrength, yStrenght, outsideDia));
            }
            for (int i = 0; i < numOfSteel - 2; i++)
            {
                SteelList.Add(new LongBar(verticalLocation, eHorizontalLocation.InsideSteel, ConCover + ConfinementDiameter + insideDia / 2, Height, uStrain, uStrength, yStrenght, insideDia));
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

        public void UpdateSectionProperties(double height, double baseLength, double unconfinedStrength, double conCover)
        {
            Height = height;
            BaseLength = baseLength;
            UnconfinedStrength = unconfinedStrength;
            ConCover = conCover;
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

            return 2 * AiySquared + GetAiSquared(eVerticalLocation.BottomSteel) + GetAiSquared(eVerticalLocation.TopSteel);
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

                double ke = (1 - GetSumAi() / (6 * b0 * h0)) * (1 - ConfinementSpacing / (2 * b0)) * (1 - ConfinementSpacing / (2 * h0)) * Math.Pow((1 - longBarArea / (b0 * h0)), -1);

                //double rho = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (b0 * h0);// TODO Using only one stirrup. Therefore, this section has only two legs. Moreover, rhoX = rhoY = rho.
                double rhoX = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (h0 * ConfinementSpacing);
                double rhoY = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (b0 * ConfinementSpacing);

                double fex = ke * rhoX * ConfinementYieldStrength;
                double fey = ke * rhoY * ConfinementYieldStrength;

                double fe = (fex + fey) / 2;

                result = 2.254 * Math.Sqrt(1 + 7.94 * fe / UnconfinedStrength) - 2 * fe / UnconfinedStrength -1.254;

            }
            else 
            {
                result = 1; // TODO check later if gama is 1 in not confined zone
            }

            return result;
        }

        public double GetFCC(bool layerIsConfined)
        {
            double result = GetGamaC(layerIsConfined) * UnconfinedStrength;
            return result;
        }

        public double CalculateFC(double strainC, bool layerIsConfined)
        {
            double strainCO = 0.002;
            double strainCC = strainCO * (1 + 5 * (GetGamaC(layerIsConfined) - 1));
            double x = strainC / strainCC;

            double Esec = GetFCC(layerIsConfined) / strainCC;
            double r = ConcreteElasticModulus / (ConcreteElasticModulus - Esec);

            double result = GetFCC(layerIsConfined) * r * x / (r - 1 + Math.Pow(x, r));

            return result;
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

        public double GetOmegaWE()
        {
            double b0 = B0;
            double h0 = H0;
            double rho = (2 * Math.PI * Math.Pow(ConfinementDiameter, 2) / 4) / (Math.Max(b0, h0) * ConfinementSpacing);

            return GetAlpha() * rho * ConfinementYieldStrength / UnconfinedStrength;
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

        #region Section Layer Methods

        public List<Layer> GetConfinedLayers(int numberOfTotalLayers)
        {
            if (Confined)
            {
                var confinedLayers = new List<Layer>();

                double b0 = B0;
                double h0 = H0 / (numberOfTotalLayers - 2);

                for (int i = 0; i < numberOfTotalLayers - 2; i++)
                {
                    confinedLayers.Add(new Layer(i * h0 + h0 / 2 + ConfinementDiameter / 2 + ConCover, b0, h0));
                }

                return confinedLayers;
            }
            return new List<Layer>();
        }

        protected List<Layer> GetUnconfinedLayers(int numberOfTotalLayers)
        {
            if (Confined)
            {
                var layersForConfinedSection = new List<Layer>();

                // Add layers for the parts that are left in concrete cover

                // For top layer
                //layersForConfinedSection.Add(new Layer((ConCover + ConfinementDiameter / 2) / 2, BaseLength, ConCover + ConfinementDiameter / 2));
                double datum = Height - conCover - ConfinementDiameter / 2;

                double hTop = (Height - datum) / 50;
                double bTop = BaseLength;

                for (int i = 0; i < 50; i++)
                {
                    layersForConfinedSection.Add(new Layer(datum + i * hTop + hTop / 2, bTop, hTop));
                }


                // For middle layers                                                                                              _______________
                double h = H0 / (numberOfTotalLayers - 2);                                                                   //  |   _________   |
                double b = (BaseLength - B0) / 2;                                                                            //  |//|         |//|
                for (int i = 0; i < numberOfTotalLayers - 2; i++)                                                            //  |//|         |//|
                {                                                                                                            //  |//|         |//|
                    layersForConfinedSection.Add(new Layer((ConCover + ConfinementDiameter / 2) + i * h + h / 2, b, h));     //  |//|         |//|
                    // Add second layer for left and right                                                                       |//|         |//|
                    layersForConfinedSection.Add(new Layer((ConCover + ConfinementDiameter / 2) + i * h + h / 2, b, h));     //  |//|         |//|
                }                                                                                                            //  |//|_________|//|
                                                                                                                             //  |_______________|
                return layersForConfinedSection;
            }
            else
            {
                var layersForUnconfinedSection = new List<Layer>();

                double b = BaseLength;
                double h = Height/ numberOfTotalLayers;

                for (int i = 0; i < numberOfTotalLayers; i++)
                {
                    layersForUnconfinedSection.Add(new Layer(i * h + h / 2, b, h));
                }

                return layersForUnconfinedSection;
            }


        }

        #endregion

        #region Force and Moment Methods

        public double GetStrain(double location, double strainAtTop, double neutralAxis)
        {
            // a*location+b=GetStrain
            // a*neutralAxis+b=0 ---> b = -a*neturalAxis

            //double a = strainInput / ((Height - conCover - ConfinementDiameter / 2) - neutralAxis);
            //double b = -a * neutralAxis;

            if (location <= Height && location >= 0)
            {
                //double result = a * location + b;
                //- ConCover - ConfinementDiameter / 2
                double slope = strainAtTop / (Height - neutralAxis);
                double result = slope * (location - neutralAxis);

                return result;
            }
            else { return 0; } // location of desired strain should be on the section



        }
        public double GetSteelForce(eVerticalLocation type, double strainAtTop, double neutralAxis)
        {
            var steels = GetBars(type);
            double strain = GetStrain(steels[0].Location, strainAtTop, neutralAxis);
            var ultimateStrain = steels[0].UltimateStrain;
            var yieldStrength = steels[0].YieldStrength;
            var yieldStrain = steels[0].YieldStrain;
            var elasticModulusOfSteel = 200000; // 200GPa = 200000MPa

            double totalForces = 0;

            if (Math.Abs(strain) > ultimateStrain)
            {
                totalForces = 0;
            }
            else if (Math.Abs(strain) > yieldStrain)
            {
                for (int i = 0; i < steels.Count; i++)
                {
                    if (strain < 0)
                    {
                        totalForces -= yieldStrength * steels[i].Area;
                    }
                    else
                    {
                        totalForces += yieldStrength * steels[i].Area;
                    }
                }
            }
            else
            {
                for ( int i = 0;i < steels.Count; i++)
                {
                    totalForces += elasticModulusOfSteel * strain * steels[i].Area;
                }
            }


            return totalForces;
        }
        public double GetConcreteForce(int numberOfTotalLayers, double strainAtTop, double neutralAxis)
        {
            var confinedLayers = GetConfinedLayers(numberOfTotalLayers);
            var unconfinedLayers = GetUnconfinedLayers(numberOfTotalLayers);

            double totalForce = 0;

            for (int i = 0; i < confinedLayers.Count; i++)
            {
                var strain = GetStrain(confinedLayers[i].CenterOfGravity, strainAtTop, neutralAxis);
                if (strain > 0) 
                {
                    var fc = CalculateFC(strain, true);
                    totalForce += fc * confinedLayers[i].Area;
                }
            }

            for (int i = 0; i < unconfinedLayers.Count; i++)
            {
                var strain = GetStrain(unconfinedLayers[i].CenterOfGravity, strainAtTop, neutralAxis);
                if (strain > 0)
                {
                    totalForce += CalculateFC(strain, false) * unconfinedLayers[i].Area;
                }
            }

            return totalForce;
        }
        public double GetTotalForce(int numberOfTotalLayers, double strainAtTop, double neutralAxis)
        {
            var concreteForce = GetConcreteForce(numberOfTotalLayers, strainAtTop, neutralAxis);
            var botSteelForce = GetSteelForce(eVerticalLocation.BottomSteel, strainAtTop, neutralAxis);
            var topSteelFOrce = GetSteelForce(eVerticalLocation.TopSteel, strainAtTop, neutralAxis);

            return concreteForce + botSteelForce + topSteelFOrce;
        }
        public double GetConcreteMoment(int numberOfTotalLayers, double strainAtTop, double neutralAxis)
        {
            // Neutral axis should already be calculated
            var confinedLayers = GetConfinedLayers(numberOfTotalLayers);
            var unconfinedLayers = GetUnconfinedLayers(numberOfTotalLayers);

            double totalMoment = 0;

            for (int i = 0; i < confinedLayers.Count; i++)
            {
                var strain = GetStrain(confinedLayers[i].CenterOfGravity, strainAtTop, neutralAxis);
                if (strain > 0)
                {
                    var fc = CalculateFC(strain, true);
                    totalMoment += fc * confinedLayers[i].Area * (confinedLayers[i].CenterOfGravity - neutralAxis);
                }
            }

            for (int i = 0; i < unconfinedLayers.Count; i++)
            {
                var strain = GetStrain(unconfinedLayers[i].CenterOfGravity, strainAtTop, neutralAxis);
                if (strain > 0)
                {
                    totalMoment += CalculateFC(strain, false) * unconfinedLayers[i].Area * (unconfinedLayers[i].CenterOfGravity - neutralAxis);
                }
            }

            return totalMoment;
        }
        public double GetTotalMoment(int numberOfTotalLayers, double strainAtTop, double neutralAxis)
        {
            var topSteelMoment = GetSteelForce(eVerticalLocation.TopSteel, strainAtTop, neutralAxis) * (GetBars(eVerticalLocation.TopSteel)[0].Location - neutralAxis); // Neutral axis should already be calculated
            var bottomSteelMoment = GetSteelForce(eVerticalLocation.BottomSteel, strainAtTop, neutralAxis) * (GetBars(eVerticalLocation.BottomSteel)[0].Location - neutralAxis); // Neutral axis should already be calculated
            var concreteMoment = GetConcreteMoment(numberOfTotalLayers, strainAtTop, neutralAxis);

            var result = topSteelMoment + bottomSteelMoment + concreteMoment;
            return result;
        }

        #endregion

        #region Calculation Methods

        public double GetNeutralAxis(int numberOfTotalLayers,double strainAtTop)
        {
            double neutralAxis = Height / 2.0;
            if (strainAtTop == 0)
            {
                return Height / 2;
            }
            else
            {
                double totalForce = 1;
                double neutralAxisIncriment = Height / numberOfTotalLayers;

                while (totalForce > 0)
                {
                    totalForce = GetTotalForce(numberOfTotalLayers, strainAtTop, neutralAxis);
                    neutralAxis += neutralAxisIncriment;

                    if (neutralAxis >= Height - conCover - ConfinementDiameter / 2) { break; }

                }
                return neutralAxis;
            }
        }

        public GraphData GetStrainNeutralAxis(int numberOfTotalLayers, double StrainIncriment)
        {
            var strainNeutralAxis = new GraphData();
            double ultimateStrain = GetUltimateStrain();
            double strain = 0.0;

            while (strain<ultimateStrain)
            {
                double neutralAxis = GetNeutralAxis(numberOfTotalLayers, strain);

                strainNeutralAxis.X.Add(strain);
                strainNeutralAxis.Y.Add(neutralAxis);

                strain += StrainIncriment;
            }
            return strainNeutralAxis;
        }

        public GraphData GetCurvatureMomentAxis(int numberOfTotalLayers, double StrainIncriment)
        {
            var strainCurvatureMoment = new GraphData();
            double ultimateStrain = GetUltimateStrain();
            double strain = 0.0;

            while (strain < ultimateStrain)
            {
                double neutralAxis = GetNeutralAxis(numberOfTotalLayers, strain);
                double moment = GetTotalMoment(numberOfTotalLayers, strain,neutralAxis);
                double curvature = strain / (Height  - neutralAxis);

                strainCurvatureMoment.X.Add(curvature);
                strainCurvatureMoment.Y.Add(moment);

                strain += StrainIncriment;
            }
            return strainCurvatureMoment;
        }

        #endregion

        #endregion

    }
}
