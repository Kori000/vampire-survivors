using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有抛射行为的基本脚本[放置在抛射武器的预制件上]
/// </summary>
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction; // 方向
    public float destroyAfterSeconds; // 销毁时间
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds); // 销毁时间到达后销毁对象
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) // 左
        {
            scale.x = -scale.x;
            scale.y = -scale.y;
        }
        else if (dirx == 0 && diry > 0) // 上
        {
            scale.x = -scale.x;
        }
        else if (dirx == 0 && diry < 0) // 下
        {
            scale.y = -scale.y;
        }
        else if (dir.x > 0 && dir.y > 0) // 右上
        {
            rotation.z = 0f;
        }
        else if (dir.x > 0 && dir.y < 0) // 右下
        {
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y > 0) // 左上
        {
            scale.x = -scale.x;
            scale.y = -scale.y;
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y < 0) // 左下
        {
            scale.x = -scale.x;
            scale.y = -scale.y;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
