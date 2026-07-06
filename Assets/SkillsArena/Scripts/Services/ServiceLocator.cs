namespace SkillsArena
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

        public void RegisterService<TService>(TService service) where TService : class, IService
        {
            Service<TService>.ServiceInstance = service;
        }

        public TService GetService<TService>() where TService : class, IService
        {
            return Service<TService>.ServiceInstance;
        }

        private class Service<TService> where TService : class, IService
        {
            public static TService ServiceInstance;
        }
    }
}