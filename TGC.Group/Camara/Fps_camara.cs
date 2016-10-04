
using Microsoft.DirectX;
using TGC.Core.Camara;

namespace TGC.Examples.Camara
{
    class FPS_camara : TgcCamera
    {

        //var globales
        Vector3 pos_camara, pos_pj, offset_pj;
        float altura_cam;
        float offset_frontal;
        float rotacionY;

        public FPS_camara crear_camara(Vector3 pos_personaje, float altura, float desplazamiento)
        {

            this.pos_pj = pos_personaje;
            this.altura_cam = altura;
            this.offset_frontal = desplazamiento;
            return this;
        }

        public override void UpdateCamera(float elapsedTime)
        {

            calcularPosicion(out pos_camara, out pos_pj);
            //setCamera(pos,posicion_personaje);

        }

        /*public void setCamera(Vector3 pos, Vector3 posicion_personaje) {

            pos_pj = posicion_personaje;

        }*/

        private void calcularPosicion(out Vector3 pos, out Vector3 posicion_personaje)
        {
            posicion_personaje = Vector3.Add(pos_pj, offset_pj);

            var matrix_transformacion = Matrix.Translation(0, altura_cam, offset_frontal) *
                                        Matrix.RotationY(rotacionY) *
                                        Matrix.Translation(posicion_personaje);

            pos.X = matrix_transformacion.M41;
            pos.Y = matrix_transformacion.M42;
            pos.Z = matrix_transformacion.M43;
        }



        public void rotarY(float angulo)
        {

            rotacionY += angulo;

        }


    }

}

