using UnityEngine;
/*Bazı oyunlarda ekrana basılı tuttuğunuz sürece karakter koşar, bıraktığınızda durur (veya tam tersi). Bu scripti modüler yapalım,
 editörden "Basınca gitsin" veya "Basınca dursun" diye seçebilin.*/
public class TapMove : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float speed = 5f;
    
    // Eğer bu tikliyse: Basılı tutunca gider.
    // Tikli değilse: Karakter hep gider, basılı tutunca durur.
    [SerializeField] private bool moveOnHold = true; 

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        bool isHolding = InputManager.Instance.IsTouching;
        bool shouldMove = false;

        if (moveOnHold)
        {
            // Basılı tutuyorsan git
            shouldMove = isHolding;
        }
        else
        {
            // Basılı tutmuyorsan git (Basınca dur)
            shouldMove = !isHolding;
        }

        if (shouldMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}