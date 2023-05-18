using UnityEngine;


namespace Linx.Framework.CodeStyle
{
    public interface ICodeStyle
    {
    }

    public class CodeStyle : MonoBehaviour, ICodeStyle
    {
        private static CodeStyle s_instance;
        private int m_speed;
        public int Code { get; set; }

        public int Test => test;

        private int m_a;
        [SerializeField] private int test;


        private void Awake()
        {
            s_instance = this;
            s_instance.SomeMethod();
        }

        private void SomeMethod()
        {
            Debug.Log(m_speed);
        }
    }
}