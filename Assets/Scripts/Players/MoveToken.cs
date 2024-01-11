using UnityEngine;

public class MoveToken : MonoBehaviour
{

    [SerializeField]
    private float _Speed = 20f;

    private bool _Endzone = false;
    private bool _EndzoneRight = false;
    private bool _EndzoneLeft = false;


    void Update()
    {
        if (MoveCondition())
        {
            transform.Translate(_Speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);
        }
        else
        {
            if (!_Endzone)
                transform.Translate(_Speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);
        }

    }

    private bool MoveCondition()
    {
        return _EndzoneRight && Input.GetAxis("Horizontal") <= 0 || _EndzoneLeft && Input.GetAxis("Horizontal") >= 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        _Endzone |= other.tag == "StopPlayer";
        _EndzoneRight |= (other.tag == "StopPlayer" && other.transform.position.x >= 3);
        _EndzoneLeft |= (other.tag == "StopPlayer" && other.transform.position.x <= -3);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "StopPlayer")
        {
            _Endzone = false;
            _EndzoneRight = false;
            _EndzoneLeft = false;
        }
    }
}
