using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float mouseSensitivity; // 마우스 감도 조정을 추가하니 더 편함
    float rotation = 0;
    bool isRolling;
    Coroutine rollingCoroutine = null;

    private void Start()
    {
        isRolling = false;
        ControlCursor();
    }

    private void Update()
    {
        if (!isRolling)
        {
            MovePlayer();
            Aim();
            RotatePlayer();
        }
    }

    private void OnDisable()
    {
        if (rollingCoroutine != null)
            StopCoroutine(rollingCoroutine);
    }

    private void ControlCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float tempSpeed;

        Vector3 dir = new Vector3(x, 0, z);

        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rollingCoroutine == null)
                rollingCoroutine = StartCoroutine(RollPlayer(dir));
            //RollPlayer(dir);
        }


        if (dir != Vector3.zero)
        {
            // 실제로 이동이 이뤄지는 곳
            transform.Translate(dir * speed * Time.deltaTime, Space.Self);


            // 애니메이션 변화를 위한 파라미터를 할당하는곳
            if (z >= 0)
            {
                tempSpeed = dir.magnitude * speed;
            }
            else
            {
                tempSpeed = -1 * dir.magnitude * speed;
            }

            animator.SetFloat("Speed", tempSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void Aim()
    {
        if (Input.GetMouseButton(0))
            animator.SetBool("Aim", true);
        else
            animator.SetBool("Aim", false);
    }

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
    }

    private IEnumerator RollPlayer(Vector3 _dir)
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        //구르기가 진행되는 동안 3배 빠르게 이동해야함

        float curTime = 0;
        float motionTime = 2.1f;
        while(curTime < motionTime)
        {
            transform.Translate(_dir * 3f * speed * Time.deltaTime, Space.Self); // 숫자를 크게 바꿔보니 빠르게 이동하긴 한다
            curTime += Time.deltaTime;
            yield return null;

        }

        isRolling = false;
        rollingCoroutine = null;
        // yield return new WaitForSeconds(2.1f); // 구르기 시간
        //animator.SetFloat("Speed", _dir.sqrMagnitude * 3 * speed); // 굳이?
    }
}

//private void RollPlayer(Vector3 _dir)
//{
//    transform.Translate(_dir * 3 * speed * Time.deltaTime, Space.Self);
//    //animator.set

//    animator.SetTrigger("Roll");
//    animator.SetFloat("Speed", _dir.sqrMagnitude * 3 * speed);
//}
//}