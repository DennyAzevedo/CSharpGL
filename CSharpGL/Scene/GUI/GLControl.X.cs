﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    public partial class GLControl
    {
        /// <summary>
        /// 相对于Parent左下角的位置(Left Down location)
        /// </summary>
        [Category(strGLControl)]
        [Description("相对于Parent左下角的位置(Left Down location)")]
        public int X
        {
            get { return this.left; }
            set
            {
                if (this.left != value)
                {
                    this.left = value;
                    GLControl parent = this.parent;
                    if (parent != null)
                    {
                        this.absLeft = parent.absLeft + this.left;
                        GLControl.LayoutAfterXChanged(parent, this);
                    }
                }
            }
        }

        private static void LayoutAfterXChanged(GLControl parent, GLControl control)
        {
            GUIAnchorStyles anchor = control.Anchor;
            if ((anchor & rightAnchor) == rightAnchor)
            {
                control.Width = parent.width - control.left - control.right;
            }
            else
            {
                control.right = parent.width - control.left - control.width;
            }
        }

    }
}
