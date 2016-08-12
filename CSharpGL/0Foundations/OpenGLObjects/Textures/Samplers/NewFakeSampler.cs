﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// 
    /// </summary>
    class NewFakeSampler : NewSamplerBase
    {

        /// <summary>
        /// 纹理坐标通常的范围是从(0, 0)到(1, 1)，如果我们把纹理坐标设置为范围以外会发生什么？OpenGL默认的行为是重复这个纹理图像（我们简单地忽略浮点纹理坐标的整数部分），但OpenGL提供了更多的选择
        /// </summary>
        /// <param name="wrapping"></param>
        public void SetWrapping(TextureWrapping wrapping)
        {
            this.wrapping = wrapping;
        }

        /// <summary>
        /// 组成纹理的图片数据和其要贴上去的形状的大小往往是不一样的。两种情况，纹理图片小，贴图区域大，需要放大纹理称为：magnification；或者反过来，缩小纹理显示出来，称为 minification.在做放大喝缩小的操作的时候的具体的策略如下
        /// </summary>
        public void SetTextureFilter(TextureFilter textureFilter)
        {
            this.textureFilter = textureFilter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapping"></param>
        /// <param name="textureFiltering"></param>
        /// <param name="mipmapFiltering"></param>
        public NewFakeSampler(TextureWrapping wrapping, TextureFilter textureFiltering, MipmapFilter mipmapFiltering)
            : base(wrapping, textureFiltering, mipmapFiltering)
        {
        }

        public override void Build(BindTextureTarget target)
        {
            /* Clamping to edges is important to prevent artifacts when scaling */
            OpenGL.TexParameteri((uint)target, OpenGL.GL_TEXTURE_WRAP_S, (int)this.Wrapping);
            OpenGL.TexParameteri((uint)target, OpenGL.GL_TEXTURE_WRAP_T, (int)this.Wrapping);
            /* Linear filtering usually looks best for text */
            OpenGL.TexParameteri((uint)target, OpenGL.GL_TEXTURE_MIN_FILTER, (int)this.TextureFilter);
            OpenGL.TexParameteri((uint)target, OpenGL.GL_TEXTURE_MAG_FILTER, (int)this.TextureFilter);

            // TODO: mipmap filter not working yet.
        }
    }
}