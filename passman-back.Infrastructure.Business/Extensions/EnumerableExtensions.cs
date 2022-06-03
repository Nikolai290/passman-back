using AutoMapper;
using passman_back.Domain.Core.DbEntities;
using SearchingLibrary.Models;
using SearchingLibrary.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace passman_back.Infrastructure.Business.Extensions {
    public static class EnumerableExtensions {

        public static IList<TEntity> Restruct<TEntity>(
            this IList<TEntity> list,
            IList<TEntity> newList
        ) where TEntity : AbstractDbEntity {
            var ids = newList.Select(x => x.Id);
            list = list.Where(x => ids.Contains(x.Id)).ToList();
            foreach (var item in newList.Except(list)) {
                list.Add(item);
            }

            return list;
        }

        public static IEnumerable<TEntity> SearchAnd<TEntity>(
            this IEnumerable<TEntity> list,
            string search,
            IMapper mapper,
            ISearcher searcher
        ) where TEntity : AbstractDbEntity {
            if (string.IsNullOrEmpty(search)) return list;
            var searchableSource = mapper.Map<IEnumerable<SearchableEntity<long>>>(list);
            var result = searcher.SearchAnd(searchableSource, search).Select(x => x.Id);
            return list.Where(item => result.Any(id => item.Id == id));
        }

        public static IEnumerable<TEntity> SearchOr<TEntity>(
            this IEnumerable<TEntity> list,
            string search,
            IMapper mapper,
            ISearcher searcher
        ) where TEntity : AbstractDbEntity {
            if (string.IsNullOrEmpty(search)) return list;
            var searchableSource = mapper.Map<IEnumerable<SearchableEntity<long>>>(list);
            var result = searcher.SearchOr(searchableSource, search).Select(x => x.Id);
            return list.Where(item => result.Any(id => item.Id == id));
        }
    }
}
