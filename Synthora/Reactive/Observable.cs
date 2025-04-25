using System;

namespace Synthora.Reactive
{
    internal static class Observable
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> action)
        {
            return source.Subscribe(new AnonymousObserver<T>(action));
        }
    }
} 