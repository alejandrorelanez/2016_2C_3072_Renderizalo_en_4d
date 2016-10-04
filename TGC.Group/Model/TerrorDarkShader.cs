
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Group.Camara;
using TGC.Core.Collision;
using TGC.Core.SkeletalAnimation;
using TGC.Core.UserControls.Modifier;
using TGC.Core.Utils;

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

        private TgcScene scene;
        private TgcMesh mainMesh;
        private TgcSkeletalMesh personaje;
        private Camara_FPS camaraInterna;        

        #endregion

        public override void Init()
        {           
            var d3dDevice = D3DDevice.Instance.Device;

            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(MediaDir + "Scene\\escenario-tp-TgcScene.xml");                        
            
            var skeletalLoader = new TgcSkeletalLoader();
            personaje = skeletalLoader.loadMeshAndAnimationsFromFile(
                MediaDir + "SkeletalAnimations\\Robot\\Robot-TgcSkeletalMesh.xml",
                MediaDir + "SkeletalAnimations\\Robot\\",
                new[]
                {
                    MediaDir + "SkeletalAnimations\\Robot\\Caminando-TgcSkeletalAnim.xml",
                    MediaDir + "SkeletalAnimations\\Robot\\Parado-TgcSkeletalAnim.xml"
                });

            //Configurar animacion inicial
            personaje.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.Position = new Vector3(50, 1, 50);
            personaje.rotateY(Geometry.DegreeToRadian(180f));
            //personaje.Position = new Vector3(200, 0, -300);
            personaje.Scale = new Vector3(0.75f, 0.75f, 0.75f);


               camaraInterna = new Camara_FPS(personaje.Position, 0, 100);
               camaraInterna.TargetDisplacement = new Vector3(0, 80, 0);

            Camara = camaraInterna;

            
            
        }

        public override void Update()
        {
            PreUpdate();
            var velocidadCaminar = 400f;
            var velocidadRotacion = 120f;            
            var moveForward = 0f;
            float rotate = 0;
            var moving = false;
            var rotating = false;
                        
            if (Input.keyDown(Key.W))
            {
                moveForward = -velocidadCaminar;
                moving = true;
            }
                        
            if (Input.keyDown(Key.S))
            {
                moveForward = velocidadCaminar;
                moving = true;
            }
            
            if (Input.keyDown(Key.D))
            {
                rotate = velocidadRotacion;
                rotating = true;
            }

            if (Input.keyDown(Key.A))
            {
                rotate = -velocidadRotacion;
                rotating = true;
            }

            //Si hubo rotacion 
            if (rotating) 
            {
                var rotAngle = Geometry.DegreeToRadian(rotate * ElapsedTime);
                personaje.rotateY(rotAngle);
                camaraInterna.rotateY(rotAngle);
            }
            
            if (moving)
            {
                personaje.playAnimation("Caminando", true);             
                var lastPos = personaje.Position;

                //La velocidad de movimiento tiene que multiplicarse por el elapsedTime para hacerse independiente de la velocida de CPU
                //Ver Unidad 2: Ciclo acoplado vs ciclo desacoplado
                personaje.moveOrientedY(moveForward * ElapsedTime);

                //Detectar colisiones
                var collide = false;
                foreach (var mesh in scene.Meshes)
                {
                    var result = TgcCollisionUtils.classifyBoxBox(personaje.BoundingBox, mesh.BoundingBox);
                    if (result == TgcCollisionUtils.BoxBoxResult.Adentro ||
                        result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {
                        collide = true;
                        break;
                    }
                }
                               
                if (collide)
                {
                    personaje.Position = lastPos;
                }

                camaraInterna.Target = personaje.Position;
            }

            else
            {
                personaje.playAnimation("Parado", true);
            }

            camaraInterna.ajustarPosicionDeCamara(scene,personaje);
       }           


        public override void Render()
        {
            PreRender();
            
            scene.renderAll();                                       

            PostRender();
        }

        public override void Dispose()
        {            
            scene.disposeAll();
        }
    }
}
