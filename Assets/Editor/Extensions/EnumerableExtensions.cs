using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Armageddon.Editor.Extensions {

    public static class EnumerableExtensions {

        public static object ElementAtOrDefault(this IEnumerable _sequence, int _index) {
            if(_sequence == null) throw new ArgumentNullException(nameof(_sequence));

            foreach (object _element in _sequence) {
                if(_index == 0) return _element;

                _index--;
            }

            return null;
        }

        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> _source, int _count) {
            if(_source == null) throw new ArgumentNullException(nameof(_source));

            return _count <= 0 ? _source.Skip(0) : SkipLastIterator(_source, _count);
        }

        private static IEnumerable<TSource> SkipLastIterator<TSource>(IEnumerable<TSource> _source, int _count) {
            Debug.Assert(_source != null);
            Debug.Assert(_count > 0);

            Queue<TSource> _queue = new Queue<TSource>();

            using (IEnumerator<TSource> _e = _source.GetEnumerator()) {
                while (_e.MoveNext()) {
                    if(_queue.Count == _count) {
                        do {
                            yield return _queue.Dequeue();

                            _queue.Enqueue(_e.Current);
                        } while (_e.MoveNext());

                        break;
                    } else {
                        _queue.Enqueue(_e.Current);
                    }
                }
            }
        }

    }

}