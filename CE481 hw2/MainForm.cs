using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CE481_hw2
{
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public MainForm()
        {
            InitializeComponent();

            Section trialSec = new Section();

            trialSec.UpdateSectionProperties(500, 400, 30, 50);
            trialSec.UpdateConfinement(true, 20, 200, 100);// diameter in mm and yield strength in MPa and spacing in mm
            trialSec.AddSteelLayer(LongBar.eVerticalLocation.TopSteel, 2, false, 24, 0, 0.018, 420, 0.003, 260); // Add 2 top steel
            trialSec.AddSteelLayer(LongBar.eVerticalLocation.BottomSteel, 2, false, 24, 0, 0.018, 420, 0.003, 260); // Add 2 bottom steel

            var data = trialSec.GetCurvatureMomentAxis(1000, 0.0001);
            var a = 0;
            //string msg = "";
            //for (int i = 0; i < 50; i++)
            //{
            //    msg = msg + "x: " + data.X[i].ToString() + " Y: " + data.Y[i].ToString() + "\n";
            //}
            //MessageBox.Show(msg);

            //double strainLevel = 0.002;
            //double neutralAxis = 200;

            //var con = trialSec.GetConcreteForces(1000, strainLevel, neutralAxis);
            //var topS = trialSec.GetSteelForces(LongBar.eVerticalLocation.TopSteel, strainLevel, neutralAxis);
            //var botS = trialSec.GetSteelForces(LongBar.eVerticalLocation.BottomSteel, strainLevel, neutralAxis);
            //var topstra = trialSec.GetStrain(trialSec.GetBars(LongBar.eVerticalLocation.TopSteel)[0].Location, strainLevel, neutralAxis);
            //var botstra = trialSec.GetStrain(trialSec.GetBars(LongBar.eVerticalLocation.BottomSteel)[0].Location, strainLevel, neutralAxis);
            //var asd = trialSec.CalculateFC(strainLevel, true);

        }
    }
}
