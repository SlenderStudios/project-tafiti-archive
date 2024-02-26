//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace VESilverlight {

    // Base class for all Sample Controls - overrides Width, Height and
    // mouse events to use the implementation root
    public abstract class ControlBase : Control {

        #region Public Methods

        public ControlBase(Type assemblyType)
        {
            //the control template must be in an embeded resource - find it
            Stream resourceStream = null;
            //if the assembly is built with VS there will be a prefix before
            //the resource name we expect. The resource name will be at the 
            //end after a dot
            string dotResource = '.' + ResourceName;
            Assembly assembly = assemblyType.Assembly;
            string[] names = assembly.GetManifestResourceNames();
            foreach (string name in names)
            {
                if (name.Equals(ResourceName) || name.EndsWith(dotResource))
                {
                    resourceStream = assembly.GetManifestResourceStream(name);
                    break;
                }
            }
            Debug.Assert(resourceStream != null, "the resource template" + ResourceName + " not found");
            StreamReader sr = new StreamReader(resourceStream);
            string xaml = sr.ReadToEnd();
            try
            {
                actualControl = InitializeFromXaml(xaml);
            }
            catch (Exception e)
            {

            }
            sr.Close();
            Debug.Assert(actualControl != null, "failed to initialize the control");

            base.Width = actualControl.Width;
            base.Height = actualControl.Height;

            Loaded += new EventHandler(OnLoaded);
        }

        public ControlBase() : this(typeof(ControlBase))
        {
           
        }

        //forwards FindName to the actual control
        public new DependencyObject FindName(string name)
        {
            return actualControl.FindName(name);
        }

        /// <summary>
        /// Recursively sets the width/height of all children to the specified value
        /// </summary>
        /// <param name="parentElement">starting element</param>
        /// <param name="width">width to set</param>
        /// <param name="height">height to set</param>
        public void RecursivelySetWidthAndHeight(FrameworkElement parentElement, double width, double height)
        {
            RecursivelySetWidthAndHeight(parentElement, width, height, true);
        }

        /// <summary>
        /// Recursively sets the width/height of all children to the specified value
        /// </summary>
        /// <param name="parentElement">starting element</param>
        /// <param name="width">width to set</param>
        /// <param name="height">height to set</param>
        /// <param name="adjustTextBlocks">true iff TextBlock elements are to be centered</param>
        public void RecursivelySetWidthAndHeight(FrameworkElement parentElement, double width, double height, bool adjustTextBlocks)
        {
            // Set parent element
            parentElement.Width = width;
            parentElement.Height = height;

            // If adjusting text blocks
            if (adjustTextBlocks)
            {
                TextBlock parentTextBlock = parentElement as TextBlock;
                if (null != parentTextBlock)
                {
                    // Center the parentElement TextBlock
                    parentTextBlock.SetValue(Canvas.LeftProperty, (width - parentTextBlock.ActualWidth) / 2);
                    parentTextBlock.SetValue(Canvas.TopProperty, (height - parentTextBlock.ActualHeight) / 2);
                }
            }

            // Recurse through the children
            Panel parentElementPanel = parentElement as Panel;
            if (null != parentElementPanel)
            {
                foreach (FrameworkElement childElement in parentElementPanel.Children)
                {
                    RecursivelySetWidthAndHeight(childElement, width, height, adjustTextBlocks);
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        // Sets/gets the Width of the actual control
        public new double Width
        {
            get { return actualControl.Width; }
            set
            {
                base.Width = actualControl.Width = value;
                UpdateLayout();
            }
        }

       

        // Sets/gets the Height of the actual control
        public virtual new double Height
        {
            get { return actualControl.Height; }
            set
            {
                base.Height = actualControl.Height = value;
                UpdateLayout();
            }
        }

        #endregion Public Properties

        #region Public Events

        // Fired when the mouse leaves root object
        public event EventHandler RootLeave;

        #endregion Public Events

        #region Protected Methods

        // Finds the root and registers for its MouseLeave
        protected virtual void OnLoaded(object sender, EventArgs args)
        {
            FrameworkElement root = this;
            do {
                root = root.Parent as FrameworkElement;
            } while (root != null && root.Parent != null);

            if (root != null) {
                root.MouseLeave += new EventHandler(RaiseRootLeave);
            }
            Height = base.Height;
            Width = base.Width;
        }

        // called when the element is loaded or Width or Height is changed - 
        // base implementation does nothing
        protected virtual void UpdateLayout() { }

        #endregion Protected Methods

        #region Protected Proiperties

        // The resource name used to initialize the actual object
        protected abstract string ResourceName { get;}

        // sometimes this one neds to be modified
        protected FrameworkElement ActualControl
        {
            get { return actualControl; }
        }

        #endregion Protected Proiperties

        #region Private Methods

        // Fire RootLeave event if needed
        private void RaiseRootLeave(object sender, EventArgs args)
        {
            if (RootLeave != null) {
                RootLeave(sender, args);
            }
        }

        #endregion Private Methods

        #region Data

        private FrameworkElement actualControl = null; 

        #endregion Data
    }
}
