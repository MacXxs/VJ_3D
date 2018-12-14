using UnityEngine;

public class CarCam : MonoBehaviour
{
    Transform camTrans;
    Transform carCam;
    Transform car;
    
    [Tooltip("Offset de distància de seguiment de la càmara al cotxe, quant menor sigui el valor, més enrere es quedarà")]
    public float howClose = 5.0f;

    [Tooltip("Offset de distància entre la càmara i el cotxe, quant menor sigui el valor, més aprop del cotxe estarà la càmara")]
    public float yDistance = 4.0f;
    

    void Awake()
    {
        carCam = Camera.main.GetComponent<Transform>();
        camTrans = GetComponent<Transform>();
        car = camTrans.parent.GetComponent<Transform>();
    }

    void Start()
    {
        // En cas de que el pare es mori la cam es mou lliurement
        camTrans.parent = null;
    }

    void FixedUpdate()
    {
        // Es mou la càmara per apropar-se al cotxe
        camTrans.position = Vector3.Lerp(camTrans.position + new Vector3(0,yDistance,0), car.position, howClose * Time.fixedDeltaTime);
    }
}