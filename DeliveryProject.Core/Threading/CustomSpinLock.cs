namespace DeliveryProject.Core.Threading
{
    public class CustomSpinLock : IDisposable
    {
        private volatile int _lock = 0;
        private bool _disposed = false;

        public void Enter()
        {
            while (Interlocked.CompareExchange(ref _lock, 1, 0) != 0)
            {
            }
        }

        public void Exit()
        {
            Interlocked.Exchange(ref _lock, 0);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Exit(); 
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
