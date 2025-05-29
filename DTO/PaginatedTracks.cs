using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PaginatedTracks
    {
        #region Propiedades
        public String LinkNextPage { get; set; }
        public String LinkPreviousPage { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalTracks { get; set; }
        public int Offset { get; set; }
        public int Last { get; set; }
        public int Limit { get; set; }
        public List<Track> Tracks { get; set; }
        #endregion

        #region Constructores
        public PaginatedTracks() { }

        public PaginatedTracks(string linkNextPage, string linkPreviousPage, int page, int totalPages, int totalTracks, int offset, int last, int limit, List<Track> tracks)
        {
            LinkNextPage = linkNextPage;
            LinkPreviousPage = linkPreviousPage;
            Page = page;
            TotalPages = totalPages;
            TotalTracks = totalTracks;
            Offset = offset;
            Last = last;
            Limit = limit;
            Tracks = tracks;
        }
        #endregion
    }
}
