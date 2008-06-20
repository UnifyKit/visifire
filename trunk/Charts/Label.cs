/*
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
 
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
            
            _textBlock.Width = sz;
            _textBlock.TextWrapping = TextWrapping.Wrap;
            
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

        #region Internal Properties
        internal String FontString
        {
            get
            {
                return base.FontString;
            }
        }
        #endregion

        #region Private Methods
        protected override void SetDefaults()
        {
            
            base.SetDefaults();
            
            Background = Parser.ParseSolidColor("Transparent");

            this.SetValue(ZIndexProperty, 9999);

        }

        private void SetTag(FrameworkElement element,String tag)
        {
            if(element != null)
                element.Tag = tag;
        }
        #endregion Private Methods

        #region Internal Methods
        internal void SetTags(String tag)
        {
            SetTag(_textBlock,tag);
        }
        #endregion Internal Methods
    }
}
