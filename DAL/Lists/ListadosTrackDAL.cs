using DAL.Methods;
using DAL.Utils;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Lists
{
    public class ListadosTrackDAL
    {

        /// <summary>
        /// Esta función recibe una lista de IDs de canciones y devuelve sus Stats en un diccionario
        /// </summary>
        /// <param name="idTracks">Lista de IDs de canciones</param>
        /// <returns>Diccionario con las stats de cada canción</returns>
        public static Dictionary<long, Stats> getTracksStatsDAL(List<long> idTracks)
        {
            Dictionary<long, Stats> stats = new Dictionary<long, Stats>();

            foreach (long id in idTracks) {
                stats.Add(id, MetodosTrackDAL.getStatsByTrack(id));
            }

            return stats;
        }
    }
}
