using System;
using System.Collections.Generic;

namespace Matslumpiator.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Receptmodels> Items { get; set; }
        public Pager Pager { get; set; }
        public string Sökord { get; set; }
        public int Size { get; set; }
        public bool ChickenFilter { get; set; }
        public bool VegoFilter { get; set; }
        public bool FishFilter { get; set; }
        public bool BeefFilter { get; set; }
        public bool PorkFilter { get; set; }
        public bool SausageFilter { get; set; }
        public bool MeatFilter { get; set; }
        public bool OtherFilter { get; set; }
        public string Cookingtime { get; set; } 
    }


    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize)
        {

            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }

}