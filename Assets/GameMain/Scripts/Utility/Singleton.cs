namespace BOO
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly object _lock = new object();
        private static T _instance;

        // 公共静态属性用于获取单例实例
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        // 防止外部实例化
        protected Singleton() { }
    }

}