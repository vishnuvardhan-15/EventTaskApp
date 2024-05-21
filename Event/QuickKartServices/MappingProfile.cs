using AutoMapper;
using QuickKartDataAccessLayer.Models;
using QuickKartServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickKartServices
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Products, Product>();
            CreateMap<Product, Products>();
           
        }
    }
}
