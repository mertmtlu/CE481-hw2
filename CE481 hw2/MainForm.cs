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

            trialSec.UpdateSectionProperties(300, 200, 30);
            trialSec.UpdateConfinement(true, 20, 200, 100);// diameter in mm and yield strength in MPa and spacing in mm
            trialSec.AddSteelLayer(LongBar.eVerticalLocation.TopSteel, 2, false, 24, 0, 0.018, 420, 0.003, 260); // Add 2 top steel
            trialSec.AddSteelLayer(LongBar.eVerticalLocation.BottomSteel, 2, false, 24, 0, 0.018, 420, 0.003, 260); // Add 2 bottom steel

        }
    }
}
