using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.Core.Infrastructure.Mapper;

namespace EasyCode.ViewModel.Mapper
{
    public static class EntityMapper
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
    }
}
