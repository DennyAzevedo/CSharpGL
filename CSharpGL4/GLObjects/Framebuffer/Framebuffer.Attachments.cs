﻿using System;
using System.Collections.Generic;

namespace CSharpGL
{
    /// <summary>
    ///
    /// </summary>
    public enum RenderbufferAttachment : uint
    {
        /// <summary>
        ///
        /// </summary>
        ColorAttachment0 = GL.GL_COLOR_ATTACHMENT0,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment1 = GL.GL_COLOR_ATTACHMENT1,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment2 = GL.GL_COLOR_ATTACHMENT2,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment3 = GL.GL_COLOR_ATTACHMENT3,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment4 = GL.GL_COLOR_ATTACHMENT4,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment5 = GL.GL_COLOR_ATTACHMENT5,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment6 = GL.GL_COLOR_ATTACHMENT6,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment7 = GL.GL_COLOR_ATTACHMENT7,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment8 = GL.GL_COLOR_ATTACHMENT8,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment9 = GL.GL_COLOR_ATTACHMENT9,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment10 = GL.GL_COLOR_ATTACHMENT10,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment11 = GL.GL_COLOR_ATTACHMENT11,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment12 = GL.GL_COLOR_ATTACHMENT12,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment13 = GL.GL_COLOR_ATTACHMENT13,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment14 = GL.GL_COLOR_ATTACHMENT14,

        /// <summary>
        ///
        /// </summary>
        ColorAttachment15 = GL.GL_COLOR_ATTACHMENT15,

        /// <summary>
        ///
        /// </summary>
        DepthAttachment = GL.GL_DEPTH_ATTACHMENT,

        /// <summary>
        ///
        /// </summary>
        StencilAttachment = GL.GL_STENCIL_ATTACHMENT,

        /// <summary>
        ///
        /// </summary>
        DepthStencilAttachment = GL.GL_DEPTH_STENCIL_ATTACHMENT,
    }

    public partial class Framebuffer
    {
        private static readonly uint[] attachment_id =
        {
			GL.GL_COLOR_ATTACHMENT0,
			GL.GL_COLOR_ATTACHMENT1,
			GL.GL_COLOR_ATTACHMENT2,
			GL.GL_COLOR_ATTACHMENT3,
			GL.GL_COLOR_ATTACHMENT4,
			GL.GL_COLOR_ATTACHMENT5,
			GL.GL_COLOR_ATTACHMENT6,
			GL.GL_COLOR_ATTACHMENT7,
			GL.GL_COLOR_ATTACHMENT8,
			GL.GL_COLOR_ATTACHMENT9,
			GL.GL_COLOR_ATTACHMENT10,
			GL.GL_COLOR_ATTACHMENT11,
			GL.GL_COLOR_ATTACHMENT12,
			GL.GL_COLOR_ATTACHMENT13,
			GL.GL_COLOR_ATTACHMENT14,
			GL.GL_COLOR_ATTACHMENT15,
        };

        private List<Renderbuffer> colorBufferList = new List<Renderbuffer>();
        private Renderbuffer depthBuffer;
        private int nextColorAttachmentIndex = 0;

        /// <summary>
        /// Attach a texture.
        /// <para>Bind() this framebuffer before invoking this method.</para>
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public void Attach(Texture texture)
        {
            if (nextColorAttachmentIndex >= attachment_id.Length)
            { throw new IndexOutOfRangeException("Not enough color attach points!"); }

            glFramebufferTexture2D(
    GL.GL_FRAMEBUFFER, attachment_id[nextColorAttachmentIndex++], GL.GL_TEXTURE_2D, texture.Id, 0);
        }

        /// <summary>
        /// Attach a render buffer.
        /// <para>Bind() this framebuffer before invoking this method.</para>
        /// </summary>
        /// <param name="renderbuffer"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public void Attach(Renderbuffer renderbuffer, FramebufferTarget target = FramebufferTarget.Framebuffer)
        {
            switch (renderbuffer.BufferType)
            {
                case RenderbufferType.DepthBuffer:
                    AttachDepthbuffer(renderbuffer, target);
                    break;

                case RenderbufferType.ColorBuffer:
                    AttachColorbuffer(renderbuffer, target);
                    break;

                default:
                    throw new Exception("Unexpected RenderbufferType!");
            }
        }

        private void AttachColorbuffer(Renderbuffer renderbuffer, FramebufferTarget target)
        {
            if (nextColorAttachmentIndex >= attachment_id.Length)
            { throw new IndexOutOfRangeException("Not enough attach points!"); }
            if (this.colorBufferList.Count > 0)
            {
                if (this.Width != renderbuffer.Width
                    || this.Height != renderbuffer.Height)
                {
                    throw new Exception("Size not match!");
                }
            }

            glFramebufferRenderbuffer((uint)target, attachment_id[nextColorAttachmentIndex++], GL.GL_RENDERBUFFER, renderbuffer.Id);
            this.colorBufferList.Add(renderbuffer);
            this.Width = renderbuffer.Width;
            this.Height = renderbuffer.Height;
        }

        private void AttachDepthbuffer(Renderbuffer renderbuffer, FramebufferTarget target)
        {
            if (this.depthBuffer != null)
            { throw new Exception("Depth buffer already exists!"); }

            glFramebufferRenderbuffer((uint)target, (uint)RenderbufferAttachment.DepthAttachment, GL.GL_RENDERBUFFER, renderbuffer.Id);
            this.depthBuffer = renderbuffer;
        }

        //  TODO: We should be able to just use the code below - however we
        //  get invalid dimension issues at the moment, so recreate for now.
        ///// <summary>
        ///// resize this framebuffer.
        ///// </summary>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        //public void Resize(int width, int height)
        //{
        //    //glBindRenderbuffer(GL.GL_RENDERBUFFER, colourRenderBufferId);
        //    //glRenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_RGBA, width, height);
        //    //glBindRenderbuffer(GL.GL_RENDERBUFFER, depthRenderBufferId);
        //    //glRenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH_ATTACHMENT, width, height);
        //    //var complete = OpenGL.GetDelegateFor<OpenGL.glCheckFramebufferStatusEXT>()(GL.GL_FRAMEBUFFER);
        //    this.depthBuffer.Resize(GL.GL_DEPTH_ATTACHMENT, width, height);
        //    faoreach (var item in this.colorBufferList)
        //    {
        //        item.Resize(GL.GL_RGBA, width, height);
        //    }
        //    this.CheckCompleteness();
        //}
    }
}