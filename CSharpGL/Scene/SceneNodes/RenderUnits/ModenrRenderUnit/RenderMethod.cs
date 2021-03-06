﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// A smallest unit that can render somthing.
    /// </summary>
    [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]
    public partial class RenderMethod
    {
        private const string strRenderMethod = "RenderMethod";
        /// <summary>
        /// Shader Program that does the rendering algorithm.
        /// </summary>
        [Category(strRenderMethod)]
        [Description("Shader Program that does the rendering algorithm.")]
        public ShaderProgram Program { get; private set; }

        /// <summary>
        /// Vertex Array Object.
        /// </summary>
        [Category(strRenderMethod)]
        [Description("Vertex Array Object.")]
        public VertexArrayObject[] VertexArrayObjects { get; private set; }

        /// <summary>
        ///
        /// </summary>
        [Category(strRenderMethod)]
        [Description("OpenGL toggles.")]
        public GLStateList StateList { get; private set; }

        /// <summary>
        /// A smallest unit that can render somthing.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="vaos"></param>
        /// <param name="states"></param>
        public RenderMethod(ShaderProgram program, VertexArrayObject[] vaos, params GLState[] states)
        {
            this.Program = program;
            this.VertexArrayObjects = vaos;
            this.StateList = new GLStateList();
            this.StateList.AddRange(states);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformFeedbackObj"></param>
        public void Render(TransformFeedbackObject transformFeedbackObj)
        {
            this.Render(ControlMode.ByFrame, transformFeedbackObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlMode">index buffer is accessable randomly or only by frame.</param>
        public void Render(ControlMode controlMode = ControlMode.ByFrame)
        {
            this.Render(controlMode, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlMode">index buffer is accessable randomly or only by frame.</param>
        /// <param name="transformFeedbackObj"></param>
        public void Render(ControlMode controlMode, TransformFeedbackObject transformFeedbackObj)
        {
            ShaderProgram program = this.Program;
            GLStateList stateList = this.StateList;

            // 绑定shader
            program.Bind();
            program.PushUniforms(); // push new uniform values to GPU side.

            stateList.On();

            if (transformFeedbackObj != null)
            {
                transformFeedbackObj.Bind();
                foreach (var vao in this.VertexArrayObjects)
                {
                    transformFeedbackObj.Begin(vao.DrawCommand.Mode);
                    vao.Draw(controlMode);
                    transformFeedbackObj.End();
                }
                transformFeedbackObj.Unbind();
            }
            else
            {
                foreach (var vao in this.VertexArrayObjects)
                {
                    vao.Draw(controlMode);
                }
            }

            stateList.Off();

            // 解绑shader
            program.Unbind();
        }
    }
}
