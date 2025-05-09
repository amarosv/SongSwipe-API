﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Genre
    {
        #region Propiedades
        public long Id { get; set; }
        public String Name { get; set; }
        public String Picture { get; set; }
        public String Picture_Small { get; set; }
        public String Picture_Medium { get; set; }
        public String Picture_Big { get; set; }
        public String Picture_XL { get; set; }
        #endregion

        #region Constructores
        public Genre() { }

        public Genre(long id, string name, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL)
        {
            Id = id;
            Name = name;
            Picture = picture;
            Picture_Small = pictureSmall;
            Picture_Medium = pictureMedium;
            Picture_Big = pictureBig;
            Picture_XL = pictureXL;
        }
        #endregion

    }
}
