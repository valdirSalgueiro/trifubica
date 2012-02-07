﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;

namespace ColorLand
{

    public class Camera
    {
        private const int CAMERA_START_POS_X = 0;
        private const int CAMERA_START_POS_Y = 0;

        private const int CAMERA_START_DESTINY_X = 320;
        private const int CAMERA_START_DESTINY_Y = 240;

        private const float MIN_ZOOM_INT = 0.16f;
        private const float MAX_ZOOM_INT = 1f;


        private const int CAMERA_SPEED = 5;

        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation

        private GameObject _objCenter;
        private Vector2 _destiny;

        private float speed = CAMERA_SPEED;


        public Camera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = new Vector2(CAMERA_START_DESTINY_X, CAMERA_START_DESTINY_Y);//Camera posicionada no centro da tela
            _destiny = new Vector2(CAMERA_START_DESTINY_X, CAMERA_START_DESTINY_Y);
            _objCenter = null;
        }


        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(_rotation) *
                                         Matrix.CreateScale(new Vector3(_zoom, _zoom, 0)) *
                                         Matrix.CreateTranslation(
                                            new Vector3(Game1.sSCREEN_RESOLUTION_WIDTH * 0.5f,
                                                        Game1.sSCREEN_RESOLUTION_HEIGHT * 0.5f, 0));
                                            /*new Vector3(Game1.getInstance().GraphicsDevice.Viewport.Width * 0.5f,
                                                        Game1.getInstance().GraphicsDevice.Viewport.Height * 0.5f, 0));*/
            return _transform;
        }

        public void zoomIn()
        {
            _zoom += 0.01f;
        }
        public void zoomOut()
        {
            if (_zoom > MIN_ZOOM_INT)
            {
                _zoom -= 0.01f;
            }
            else
            {
                _zoom = MIN_ZOOM_INT;
            }
        }

        public void zoomIn(float value)
        {
            _zoom += value;
            if (_zoom > MAX_ZOOM_INT)
            {
                _zoom = MAX_ZOOM_INT;
            }

        }

        public void zoomOut(float value)
        {
            if (_zoom > MIN_ZOOM_INT)
            {
                _zoom -= value;
            }
            else
            {
                _zoom = MIN_ZOOM_INT;
            }
        }

        public void setZoom(float zoom)
        {

            if (zoom > MIN_ZOOM_INT)
            {
                _zoom = zoom;
            }
            else
            {
                _zoom = MIN_ZOOM_INT;
            }

        }

        public void update()
        {
            if (_objCenter != null)
            {
                _destiny.X = _objCenter.getX();
                _destiny.Y = _objCenter.getY();
            }

            float y = _destiny.Y - _pos.Y;
            float x = _destiny.X - _pos.X;
            if (Math.Sqrt(y * y + x * x) <= speed)
            {
                _pos.X = _destiny.X;
                _pos.Y = _destiny.Y;
            }
            else
            {
                double angle = Math.Atan2(y, x); //180
                //double graus = angle * 180 / Math.PI;//Console.WriteLine("dx:"+_destiny.X +"px:"+_pos.X+"/y:"+y+"/ang:"+angle);
                _pos.X += speed * (float)Math.Cos(angle);
                _pos.Y += speed * (float)Math.Sin(angle);
            }

            // Console.WriteLine(_pos.X + "/" + _pos.Y);
        }

        //chama essa gambiarra só
        public void updateMalucoCenterCam(float mediaX, float mediaY)
        {
            if (_objCenter == null)
            {
                _destiny.X = mediaX;
                _destiny.Y = mediaY;
            }
        }

        public void centerCamTo(int x, int y)
        {

            this._destiny.X = x;
            this._destiny.Y = y;
            _objCenter = null;

        }

        public void centerCamTo(GameObject obj)
        {
            _objCenter = obj;
            if (obj != null)
            {
                _destiny.X = _objCenter.getX();
                _destiny.Y = _objCenter.getY();
            }
        }

        public GameObject getCenteredObject()
        {
            return _objCenter;
        }

        public float getZoomLevel()
        {
            return this._zoom;
        }

    }
}