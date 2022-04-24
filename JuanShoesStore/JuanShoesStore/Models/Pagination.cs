using System;
using System.Collections.Generic;
using System.Linq;

namespace JuanShoesStore.Models
{
    public class Pagination<T> : List<T>
    {
        public Pagination(List<T> items, int itemsCount, int currentPage, int itemsPerPage)
        {
            CurrentPage = currentPage;
            TotalPage = (int)Math.Ceiling(itemsCount/(double)itemsPerPage);
            ItemsPerPage = itemsPerPage;
            this.AddRange( items );

        }

        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public bool HasPrev { get; set; }
        public bool HasNext { get; set; }

        public static Pagination<T> Create(IQueryable<T> query, int currentPage, int itemsPerPage)
        {
            return new Pagination<T>(query.Skip((currentPage-1)*itemsPerPage).Take(itemsPerPage).ToList(), query.Count(),currentPage, itemsPerPage );
           /* return new Pagination<T>(query.ToList(), query.Count(),currentPage, itemsPerPage );*/
        }

    }



}

