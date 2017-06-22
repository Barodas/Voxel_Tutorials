using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASTutorial
{
    public class Modify : MonoBehaviour
    {
        private Vector2 rot;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, 100))
                {
                    EditTerrain.SetBlock(hit, new BlockAir());
                }
            }

            rot = new Vector2(
                rot.x + Input.GetAxis("Mouse X") * 3, 
                rot.y + Input.GetAxis("Mouse Y") * 3
                );

            transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

            transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
            transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        }
    }
}