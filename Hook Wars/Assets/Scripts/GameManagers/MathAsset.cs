using UnityEngine;

public class MathAsset : MonoBehaviour
{
    private MathAsset i;
    private void Awake() => i = this;

    private void FixedUpdate()
    {
        
    }


    public static float GetAngleFromDir(Vector3 direction)
    {
        float n = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public static Vector3 GetDirection(GameObject from, GameObject to, int angle = 0)
    {
        var direction = (to.transform.position - from.transform.position).normalized;
        from.transform.eulerAngles = new Vector3(0, 0, GetAngleFromDir(direction) + angle);
        return direction;
    }

    public static void MoveObject(GameObject from, GameObject to, float speed, int angle = 0)
    {
        var direction = (to.transform.position - from.transform.position).normalized;
        from.transform.position = from.transform.position + direction * speed * Time.fixedDeltaTime;
        from.transform.eulerAngles = new Vector3(0, 0, GetAngleFromDir(direction) + angle);
    }

    public static void Shufle<T>(T[] array)
    {
        var rnd = new System.Random();

        for (int i = array.Length - 1; i >= 1; i--)
        {
            int j =  rnd.Next() % (i + 1);

            var temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }
}
