namespace MovieCatalog.Tests.Mocks.Extensions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        public T Current => this.inner.Current;

        public void Dispose()
        {
            this.inner.Dispose();
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.inner.MoveNext());
        }
    }
}
