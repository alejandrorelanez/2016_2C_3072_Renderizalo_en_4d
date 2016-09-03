
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Group.Camara;

namespace TGC.Group.Model
{
    public class TerrorDarkShader : TgcExample
    {
        public TerrorDarkShader(string mediaDir, string shaderDir) : base(mediaDir, shaderDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        #region Variables
        
        private TgcBox puerta00 { get; set; }
        private TgcBox puerta01 { get; set; }
        private TgcBox puerta02 { get; set; }

        private TgcPlane suelo;

        #endregion

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            var pathTexturaSuelo = MediaDir + "Texturas\\wood.bmp";

            var pisoTexture = TgcTexture.createTexture(d3dDevice, pathTexturaSuelo);
            suelo = new TgcPlane(new Vector3(-500, 0, -500), new Vector3(2000, 0, 2000), TgcPlane.Orientations.XZplane, pisoTexture, 10f, 10f);

            Camara = new TgcFpsCamera(new Vector3(0, 70, 0), 200, 100, Input);
        }

        public override void Update()
        {
            PreUpdate();
        }

        public override void Render()
        {
            PreRender();

            suelo.render();

            PostRender();
        }

        public override void Dispose()
        {
            suelo.dispose();
        }
    }
}
