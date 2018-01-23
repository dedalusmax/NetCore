using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.Data.Common
{
    public static class ModelHelper
    {
        public static void UpdateManyToMany<T>(
            this ICollection<T> destination,
            ICollection<T> source,
            Func<T, int> keySelector,
            Action<T, T> updater = null,
            Action<T> remover = null)
            where T : class
        {
            foreach (var toRemove in destination.Where(_ => !source.Select(keySelector).Contains(keySelector(_))).ToList())
            {
                if (remover == null)
                    destination.Remove(toRemove);
                else
                    remover(toRemove);
            }

            if (updater != null)
            {
                foreach (var toUpdate in destination.Where(_ => source.Select(keySelector).Contains(keySelector(_))))
                {
                    var updated = source.Single(_ => keySelector(_) == keySelector(toUpdate));

                    updater(toUpdate, updated);
                }
            }

            foreach (var toAdd in source.Where(_ => !destination.Select(keySelector).Contains(keySelector(_))).ToList())
            {
                destination.Add(toAdd);
            }
        }

        public static void UpdateManyToMany<T>(
            this ICollection<T> destination,
            ICollection<T> source,
            Func<T, long> keySelector,
            Action<T, T> updater = null,
            Action<T> remover = null)
            where T : class
        {
            foreach (var toRemove in destination.Where(_ => !source.Select(keySelector).Contains(keySelector(_))).ToList())
            {
                if (remover == null)
                    destination.Remove(toRemove);
                else
                    remover(toRemove);
            }

            if (updater != null)
            {
                foreach (var toUpdate in destination.Where(_ => source.Select(keySelector).Contains(keySelector(_))))
                {
                    var updated = source.Single(_ => keySelector(_) == keySelector(toUpdate));

                    updater(toUpdate, updated);
                }
            }

            foreach (var toAdd in source.Where(_ => !destination.Select(keySelector).Contains(keySelector(_))).ToList())
            {
                destination.Add(toAdd);
            }
        }

        public static void UpdateManyToMany<TDestination, TSource>(
            this ICollection<TDestination> destination,
            ICollection<TSource> source,
            Func<TDestination, long> keySelectorDestination,
            Func<TSource, long> keySelectorSource,
            Func<TSource, TDestination> converter,
            Action<TDestination, TSource> updater = null,
            Action<TDestination> remover = null)
        {
            foreach (var toRemove in destination.Where(_ => !source.Select(keySelectorSource).Contains(keySelectorDestination(_))).ToList())
            {
                if (remover == null)
                    destination.Remove(toRemove);
                else
                    remover(toRemove);
            }

            if (updater != null)
            {
                foreach (var toUpdate in destination.Where(_ => source.Select(keySelectorSource).Contains(keySelectorDestination(_))))
                {
                    var updated = source.Single(_ => keySelectorSource(_) == keySelectorDestination(toUpdate));

                    updater(toUpdate, updated);
                }
            }

            foreach (var toAdd in source.Where(_ => !destination.Select(keySelectorDestination).Contains(keySelectorSource(_))).ToList())
            {
                destination.Add(converter(toAdd));
            }
        }
    }
}
