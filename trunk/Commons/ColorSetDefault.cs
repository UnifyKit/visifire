/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Visifire.Commons
{
    public static class ColorSetDefaultList
    {
        #region Static Methods
        public static Dictionary<String,List<Brush>> GenerateColorSetDefaultList()
        {
            List<Brush> colorset;
            Dictionary<String, List<Brush>> defaultColorSet = new Dictionary<String, List<Brush>>();

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#b0aeaf" ));
            colorset.Add(Parser.ParseSolidColor("#ffc68c" ));
            colorset.Add(Parser.ParseSolidColor("#808000" ));
            colorset.Add(Parser.ParseSolidColor("#d5935e" ));
            colorset.Add(Parser.ParseSolidColor("#638090" ));
            colorset.Add(Parser.ParseSolidColor("#405b45" ));
            colorset.Add(Parser.ParseSolidColor("#5b5853" ));
            colorset.Add(Parser.ParseSolidColor("#8f4b32" ));
            colorset.Add(Parser.ParseSolidColor("#863901" ));
            colorset.Add(Parser.ParseSolidColor("#6d6743" ));
            colorset.Add(Parser.ParseSolidColor("#716820" ));
            colorset.Add(Parser.ParseSolidColor("#7e6046" ));
            defaultColorSet.Add("DarkShades",colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#00aFb0" ));
            colorset.Add(Parser.ParseSolidColor("#58a128" ));
            colorset.Add(Parser.ParseSolidColor("#3762ba" ));
            colorset.Add(Parser.ParseSolidColor("#ff66bb" ));
            colorset.Add(Parser.ParseSolidColor("#88c8d6" ));
            colorset.Add(Parser.ParseSolidColor("#ffcc00" ));
            colorset.Add(Parser.ParseSolidColor("#2fefef" ));
            colorset.Add(Parser.ParseSolidColor("#f84000" ));
            colorset.Add(Parser.ParseSolidColor("#7a7363" ));
            colorset.Add(Parser.ParseSolidColor("#b3c631" ));
            colorset.Add(Parser.ParseSolidColor("#ff97a3" ));
            colorset.Add(Parser.ParseSolidColor("#956da3" ));
            colorset.Add(Parser.ParseSolidColor("#31b77c" ));
            colorset.Add(Parser.ParseSolidColor("#b2bebf" ));
            colorset.Add(Parser.ParseSolidColor("#1999dd" ));
            defaultColorSet.Add("Visifire2", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#999967" ));
            colorset.Add(Parser.ParseSolidColor("#666666" ));
            colorset.Add(Parser.ParseSolidColor("#cccccc" ));
            colorset.Add(Parser.ParseSolidColor("#cccc9a" ));
            defaultColorSet.Add("SandyShades", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#58706d" ));
            colorset.Add(Parser.ParseSolidColor("#4b5757" ));
            colorset.Add(Parser.ParseSolidColor("#7c8a6e" ));
            colorset.Add(Parser.ParseSolidColor("#b0b087" ));
            colorset.Add(Parser.ParseSolidColor("#e3e3d1" ));
            defaultColorSet.Add("Caravan", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#312c20" ));
            colorset.Add(Parser.ParseSolidColor("#494d4b" ));
            colorset.Add(Parser.ParseSolidColor("#7c7052" ));
            colorset.Add(Parser.ParseSolidColor("#b3a176" ));
            colorset.Add(Parser.ParseSolidColor("#e2cb92" ));
            defaultColorSet.Add("Picasso", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#f79646" ));
            colorset.Add(Parser.ParseSolidColor("#4f81bd" ));
            colorset.Add(Parser.ParseSolidColor("#c0504d" ));
            colorset.Add(Parser.ParseSolidColor("#9bbb59" ));
            colorset.Add(Parser.ParseSolidColor("#8064a2" ));
            colorset.Add(Parser.ParseSolidColor("#4bacc6" ));
            defaultColorSet.Add("DullShades", colorset);


            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#4386d8" ));
            colorset.Add(Parser.ParseSolidColor("#ff9a2e" ));
            colorset.Add(Parser.ParseSolidColor("#db443f" ));
            colorset.Add(Parser.ParseSolidColor("#a8d44f" ));
            colorset.Add(Parser.ParseSolidColor("#8560b3" ));
            colorset.Add(Parser.ParseSolidColor("#3cbfe3" ));
            colorset.Add(Parser.ParseSolidColor("#AFD8F8" ));
            colorset.Add(Parser.ParseSolidColor("#008E8E" ));
            colorset.Add(Parser.ParseSolidColor("#8BBA00" ));
            colorset.Add(Parser.ParseSolidColor("#FABD0F" ));
            colorset.Add(Parser.ParseSolidColor("#FA6E46" ));
            colorset.Add(Parser.ParseSolidColor("#9D080D" ));
            colorset.Add(Parser.ParseSolidColor("#A186BE" ));
            colorset.Add(Parser.ParseSolidColor("#CC6600" ));
            colorset.Add(Parser.ParseSolidColor("#FDC689" ));
            defaultColorSet.Add("Visifire1", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#3264a0" ));
            colorset.Add(Parser.ParseSolidColor("#3a76be" ));
            colorset.Add(Parser.ParseSolidColor("#4385d7" ));
            colorset.Add(Parser.ParseSolidColor("#8faee0" ));
            colorset.Add(Parser.ParseSolidColor("#becdec" ));
            defaultColorSet.Add("VisiBlue", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#a33330" ));
            colorset.Add(Parser.ParseSolidColor("#c33b38" ));
            colorset.Add(Parser.ParseSolidColor("#da4440" ));
            colorset.Add(Parser.ParseSolidColor("#e3908e" ));
            colorset.Add(Parser.ParseSolidColor("#eebebd" ));
            defaultColorSet.Add("VisiRed", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#7e9e3b" ));
            colorset.Add(Parser.ParseSolidColor("#94bb46" ));
            colorset.Add(Parser.ParseSolidColor("#a8d44f" ));
            colorset.Add(Parser.ParseSolidColor("#c3de94" ));
            colorset.Add(Parser.ParseSolidColor("#daeac0" ));
            defaultColorSet.Add("VisiGreen", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#644886" ));
            colorset.Add(Parser.ParseSolidColor("#76559e" ));
            colorset.Add(Parser.ParseSolidColor("#8560b3" ));
            colorset.Add(Parser.ParseSolidColor("#af9cca" ));
            colorset.Add(Parser.ParseSolidColor("#cfc5df" ));
            defaultColorSet.Add("VisiViolet", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#2e8faa" ));
            colorset.Add(Parser.ParseSolidColor("#35a9c8" ));
            colorset.Add(Parser.ParseSolidColor("#3cbfe3" ));
            colorset.Add(Parser.ParseSolidColor("#8dd0e8" ));
            colorset.Add(Parser.ParseSolidColor("#bce2f1" ));
            defaultColorSet.Add("VisiAqua", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#d67423" ));
            colorset.Add(Parser.ParseSolidColor("#fc8828" ));
            colorset.Add(Parser.ParseSolidColor("#ff9a2e" ));
            colorset.Add(Parser.ParseSolidColor("#ffb785" ));
            colorset.Add(Parser.ParseSolidColor("#ffd4b6" ));
            defaultColorSet.Add("VisiOrange", colorset);

            colorset = new List<Brush>();
            colorset.Add(Parser.ParseSolidColor("#545454" ));
            colorset.Add(Parser.ParseSolidColor("#999999" ));
            colorset.Add(Parser.ParseSolidColor("#707070" ));
            colorset.Add(Parser.ParseSolidColor("#464646" ));
            colorset.Add(Parser.ParseSolidColor("#818181" ));
            colorset.Add(Parser.ParseSolidColor("#bbbbbb" ));
            defaultColorSet.Add("VisiGray", colorset);

            return defaultColorSet;
        }
        #endregion Static Methods
    }
}
