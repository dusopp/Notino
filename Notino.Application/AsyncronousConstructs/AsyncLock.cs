using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.AsyncronousConstructs
{
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> lockReleaser;

        public AsyncLock()
        {
            lockReleaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            
            var wait = semaphore.WaitAsync();

            return wait.IsCompleted ?
                        lockReleaser :
                        wait.ContinueWith((_, state) => 
                            (IDisposable)state,
                            lockReleaser.Result, 
                            CancellationToken.None,
                            TaskContinuationOptions.ExecuteSynchronously,
                            TaskScheduler.Default
                        );
        }

        private sealed class Releaser : IDisposable
        {
            private readonly AsyncLock toRelease;

            internal Releaser(AsyncLock toRelease) 
            { 
                this.toRelease = toRelease;
            }

            public void Dispose() 
            { 
                toRelease.semaphore.Release();
            }
        }
    }
}
