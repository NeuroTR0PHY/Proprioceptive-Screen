using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NB
{
    public class Cursor : Cylinder
    {
        public bool track = true;
        private GameControl gc;
        //public float mouseIntensity;
        // Use this for initialization
        void Start()
        {
            gc = FindObjectOfType<GameControl>();
        }

        // Update is called once per frame
        void Update()
        {
            //var x = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            var x = Input.GetAxisRaw("Mouse X") * gc.mouseIntensity;
            var y = Input.GetAxisRaw("Mouse Y") * gc.mouseIntensity;
            if (track)
            {
                transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 0);
                var locCursor = Camera.main.WorldToScreenPoint(transform.position);
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                track = !track;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                RandomStart(0);
            }
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                Reset(true,this.gameObject);
            }
            if (Input.GetKeyUp(KeyCode.V))
            {
                Vanish();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                Appear();
            }
        }
    }

}
