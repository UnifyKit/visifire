using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;

namespace SLVisifireChartsTest
{
    public interface IOverriddenTitleObject
    {
        /// <summary> 
        /// OnApplyTemplate test actions.
        /// </summary> 
        OverriddenMethod ApplyTemplateActions { get; }

        /// <summary> 
        /// OnContentChanged test actions.
        /// </summary>
        OverriddenMethod<object, object> ContentChangedActions { get; }

    }

    /// <summary>
    /// Class that derives from ContentControl and overrides virtual members so 
    /// they can be analyzed. 
    /// </summary>
    public sealed partial class OverriddenTitleObject : Title, IOverriddenTitleObject
    {
        /// <summary>
        /// Initializes a new instance of the OverridenContentControl class. 
        /// </summary>
        public OverriddenTitleObject()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OverridenContentControl class.
        /// </summary> 
        /// <param name="constructorTest">
        /// Test actions to perform in the constructor.
        /// </param> 
        public OverriddenTitleObject(Action constructorTest)
            : base()
        {
            AssertInvariants();
            if (constructorTest != null)
            {
                constructorTest();
            }
        }

        /// <summary>
        /// Ensure that ContentControl invariants are correct. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Following common pattern")]
        public void AssertInvariants()
        {
        }

        /// <summary> 
        /// OnApplyTemplate test actions.
        /// </summary> 
        public OverriddenMethod ApplyTemplateActions
        {
            get
            {
                if (_applyTemplateActions == null)
                {
                    _applyTemplateActions = new OverriddenMethod(() => AssertInvariants());
                }
                return _applyTemplateActions;
            }
        }
        private OverriddenMethod _applyTemplateActions;

        /// <summary> 
        /// OnContentChanged test actions. 
        /// </summary>
        public OverriddenMethod<object, object> ContentChangedActions
        {
            get
            {
                if (_contentChangedActions == null)
                {
                    _contentChangedActions = new OverriddenMethod<object, object>(() => AssertInvariants());
                }
                return _contentChangedActions;
            }
        }
        private OverriddenMethod<object, object> _contentChangedActions;

        /// <summary>
        /// OnIsEnabledChanged test actions.
        /// </summary> 
        public OverriddenMethod<bool> IsEnabledChangedActions
        {
            get
            {
                if (_isEnabledChangedActions == null)
                {
                    _isEnabledChangedActions = new OverriddenMethod<bool>(() => AssertInvariants());
                }
                return _isEnabledChangedActions;
            }
        }
        private OverriddenMethod<bool> _isEnabledChangedActions;

    }
}
