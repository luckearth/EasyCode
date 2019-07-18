using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EasyCode.Core.Infrastructure.Mapper;

namespace EasyCode.ViewModel.Mapper
{
    public class EasyMapperProfile : Profile, IMapperProfile
    {
        public EasyMapperProfile()
        {

        }
        public int Order => 1;
    }
}
