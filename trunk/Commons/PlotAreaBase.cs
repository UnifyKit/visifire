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

namespace Visifire.Commons
{
    public abstract class PlotAreaBase : VisualObject
    {
        #region Public Methods
        public PlotAreaBase()
        {
            
            this.Loaded += new RoutedEventHandler(OnLoaded);
        }

        public void OnLoaded(Object sender, EventArgs e)
        {
            
        }

        public override void Init()
        {
            base.Init();
        }
        public override void Render()
        {
            base.Render();
        }
        
        

        #endregion Public Methods

        #region Private Methods
        protected override void SetDefaults()
        {
           
            base.SetDefaults();
            this.SetValue(ZIndexProperty, 1);
        }
        #endregion Private Methods
    }
}