using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using DevExpress.Spreadsheet.Charts;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CE481_hw2
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public static Section MainSection;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            checkUseConfinement.Checked = true;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                MainSection = new Section();
                double sectionHeight = Convert.ToDouble(txtSecHeight.Text);
                double sectionWidth = Convert.ToDouble(txtSecWidth.Text);
                double compressiveStrength = Convert.ToDouble(txtCompressiveStrength.Text);
                double conCover = Convert.ToDouble(txtConCover.Text);

                double topDia = Convert.ToDouble(txtTopSteelDia.Text);
                int topNum = Convert.ToInt32(txtTopSteelNum.Text);
                double topYield = Convert.ToDouble(txtTopSteelYieldStrength.Text);
                double topUltimateStrength = Convert.ToDouble(txtTopSteelUltimateStrength.Text);
                double topUltimateStrain = Convert.ToDouble(txtTopSteelUltimateStrain.Text);

                double botDia = Convert.ToDouble(txtBottomSteelDia.Text);
                int botNum = Convert.ToInt32(txtBottomSteelNum.Text);
                double botYield = Convert.ToDouble(txtBottomSteelYieldStrength.Text);
                double botUltimateStrength = Convert.ToDouble(txtBottomSteelUltimateStrength.Text);
                double botUltimateStrain = Convert.ToDouble(txtBottomSteelUltimateStrain.Text);

                bool confined = checkUseConfinement.Checked;
                

                int numOfLayer = Convert.ToInt32(txtNumOfLayer.Text);
                double strainIncriment = Convert.ToDouble(txtStrainInc.Text);


                MainSection.UpdateSectionProperties(sectionHeight, sectionWidth, compressiveStrength, conCover);
                MainSection.AddSteelLayer(LongBar.eVerticalLocation.TopSteel, topNum, false, topDia, topDia, topUltimateStrain, topUltimateStrength, topYield);
                MainSection.AddSteelLayer(LongBar.eVerticalLocation.BottomSteel, botNum, false, botDia, botDia, botUltimateStrain, botUltimateStrength, botYield);
                if (confined)
                {
                    double confinementdia = Convert.ToDouble(txtConfinementDia.Text);
                    double confinementYieldStrength = Convert.ToDouble(txtConfinementYieldStrength.Text);
                    double confinementSpacing = Convert.ToDouble(txtConfinementSpacing.Text);
                    MainSection.UpdateConfinement(confined, confinementdia, confinementYieldStrength, confinementSpacing);
                }
                else
                {
                    MainSection.UpdateConfinement(false, 0, 0, 0);
                }

                //Section trialSec = new Section();

                //trialSec.UpdateSectionProperties(500, 400, 30, 50);
                //trialSec.UpdateConfinement(true, 20, 200, 100);// diameter in mm and yield strength in MPa and spacing in mm
                //trialSec.AddSteelLayer(LongBar.eVerticalLocation.TopSteel, 2, false, 24, 0, 0.08, 420, 260); // Add 2 top steel
                //trialSec.AddSteelLayer(LongBar.eVerticalLocation.BottomSteel, 2, false, 24, 0, 0.08, 420, 260); // Add 2 bottom steel

                var data = MainSection.GetCurvatureMomentAxis(numOfLayer, strainIncriment);

                chartMomentCurvature.Series.Clear();

                DevExpress.XtraCharts.Series seriesData = new DevExpress.XtraCharts.Series("Moment-Curvature", DevExpress.XtraCharts.ViewType.Spline);

                for (int i = 0; i < data.X.Count; i++)
                {
                    seriesData.Points.Add(new DevExpress.XtraCharts.SeriesPoint(data.X[i] * 1000, data.Y[i] / 1000000));
                }

                // Add the series to the chart
                chartMomentCurvature.Series.Add(seriesData);

                chartMomentCurvature.Titles.Clear();
                DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle();
                chartTitle.Text = "Moment-Curvature";
                chartMomentCurvature.Titles.Add(chartTitle);

                DevExpress.XtraCharts.XYDiagram diagram = chartMomentCurvature.Diagram as DevExpress.XtraCharts.XYDiagram;


                diagram.AxisX.Title.Text = "Curvature (rad/m)";
                diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;

                diagram.AxisY.Title.Text = "Moment (kN.m)";
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;



                chartMomentCurvature.Refresh();

                //var u = trialSec.GetUltimateStrain() / (trialSec.Height - trialSec.GetNeutralAxis(numOfLayer, trialSec.GetUltimateStrain()));
                //var a = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Missing information!", "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkUseConfinement_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseConfinement.Checked)
            {
                txtConfinementDia.Enabled = true;
                txtConfinementYieldStrength.Enabled = true;
                txtConfinementSpacing.Enabled = true;
            }
            else
            {
                txtConfinementDia.Enabled = false;
                txtConfinementYieldStrength.Enabled = false;
                txtConfinementSpacing.Enabled = false;
            }
        }
    }
}