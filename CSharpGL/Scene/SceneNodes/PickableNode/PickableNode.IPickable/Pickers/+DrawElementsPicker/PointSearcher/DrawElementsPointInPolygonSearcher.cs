﻿using System;

namespace CSharpGL
{
    internal class DrawElementsPointInPolygonSearcher : DrawElementsPointSearcher
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="primitiveInfo"></param>
        /// <param name="picker"></param>
        /// <returns></returns>
        internal override uint Search(PickingEventArgs arg,
            RecognizedPrimitiveInfo primitiveInfo,
            DrawElementsPicker picker)
        {
            uint[] indexList = primitiveInfo.VertexIds;
            if (indexList.Length < 3) { throw new ArgumentException(); }

            IndexBuffer buffer = indexList.GenIndexBuffer(BufferUsage.StaticDraw);
            var cmd = new DrawElementsCmd(buffer, DrawMode.Points);
            picker.Node.Render4InnerPicking(arg, ControlMode.ByFrame, cmd);
            uint id = ColorCodedPicking.ReadStageVertexId(arg.X, arg.Y);

            buffer.Dispose();

            if (id != uint.MaxValue)
            { return id; }
            else
            { throw new Exception("This should not happen!"); }
        }
    }
}