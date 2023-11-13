
export var gl: WebGL2RenderingContext;
export function setGL(_gl: WebGL2RenderingContext) {
  gl = _gl;
}

export const FBO = (function() 
{
  const visible = 
  {
    create : function(gl: any, width: number, height: number)
    {
      const FBO = gl.createFramebuffer();
      FBO.width = width;
      FBO.height = height;

      FBO.bind = function(gl: any, color: any, depth: number = null)
      {
        gl.bindFramebuffer(gl.FRAMEBUFFER, this);
        gl.viewport(0, 0, this.width, this.height);

        gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT0, gl.TEXTURE_2D, color, 0);

        if (depth !== null)
        {
          gl.bindRenderbuffer(gl.RENDERBUFFER, depth);
          gl.framebufferRenderbuffer(gl.FRAMEBUFFER, gl.DEPTH_ATTACHMENT, gl.RENDERBUFFER, depth);
        }
      };
      return FBO;
    },

    bindDefault: function(gl: any)
    {
      gl.bindFramebuffer(gl.FRAMEBUFFER, null);
      gl.viewport(0, 0, gl.drawingBufferWidth, gl.drawingBufferHeight);
    }
  };

  return Object.freeze(visible);
})();