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
using Visifire.Commons;

namespace Visifire.Charts
{
    public class Label : LabelBase
    {

        #region Public Methods
        public Label()
        {
           
        }

        public override void Init()
        {
            base.Init();
            this.Text = "#YValue";

        }

        public void SetTextWrap(Double sz)
        {
            if (sz > _textBlock.Height * 5)
            {
                _textBlock.Width = sz;
                _textBlock.TextWrapping = TextWrapping.Wrap;
            }
        }

        public override void SetLeft()
        {
            throw new NotImplementedException();
        }

        public override void SetTop()
        {
            throw new NotImplementedException();
        }
        #endregion Public Methods

        #region Private Methods
        protected override void SetDefaults()
        {
            
            base.SetDefaults();
            
            Background = Parser.ParseColor("Transparent");
            this.SetValue(ZIndexProperty, 9999);

        }
        #endregion Private Methods


    }
}
