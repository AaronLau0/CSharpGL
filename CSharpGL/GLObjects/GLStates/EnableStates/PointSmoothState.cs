﻿namespace CSharpGL
{
    /// <summary>
    ///
    /// </summary>
    public class PointSmoothState : EnableState
    {
        // Activator needs a non-parameter constructor.
        /// <summary>
        /// 
        /// </summary>
        public PointSmoothState() : this(true) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableCapacity">true for enable, false for disable</param>
        public PointSmoothState(bool enableCapacity)
            : base(GL.GL_POINT_SMOOTH, enableCapacity)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.EnableCapacity)
            { return "OpenGL.Enable(GL_POINT_SMOOTH);"; }
            else
            { return "OpenGL.Disable(GL_POINT_SMOOTH);"; }
        }
    }
}