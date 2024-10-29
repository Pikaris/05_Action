using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : RecycleObject
{
    // 1. 빌보드
    // 2. 활성화되면 아래에서 위로 점점 올라간다(커브로 설정)
    // 3. 활성화되면 점점 투명해지며 작아진다(커브로 설정)

    public AnimationCurve movement;

    public AnimationCurve fade;

    public float duration = 1.0f;

    public float maxHeight = 1.5f;

    float baseHeight = 0.0f;

    float elapsedTime = 0.0f;

    TextMeshPro damageText;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    protected override void OnReset()
    {
        elapsedTime = 0.0f;
        damageText.color = Color.white;
        transform.localScale = Vector3.one;
        baseHeight = transform.position.y;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float timeRatio = elapsedTime / duration;

        float curveMove = movement.Evaluate(timeRatio);
        float currentHeight = baseHeight + curveMove * maxHeight;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        float curveAlpha = fade.Evaluate(timeRatio);
        damageText.color = new Color(1, 1, 1, curveAlpha);
        transform.localScale = Vector3.one * curveAlpha;

        if (elapsedTime > duration)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
