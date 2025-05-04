using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PaginatedArtists
    {
        #region Propiedades
        public String LinkNextPage { get; set; }
        public String LinkPreviousPage { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int Offset { get; set; }
        public int Last { get; set; }
        public int Limit { get; set; }
        public List<Artist> Artists { get; set; }
        #endregion

        #region Constructores
        public PaginatedArtists() { }

        public PaginatedArtists(string linkNextPage, string linkPreviousPage, int page, int totalPages, int offset, int last, int limit, List<Artist> artists)
        {
            LinkNextPage = linkNextPage;
            LinkPreviousPage = linkPreviousPage;
            Page = page;
            TotalPages = totalPages;
            Offset = offset;
            Last = last;
            Limit = limit;
            Artists = artists;
        }
        #endregion
    }
}
