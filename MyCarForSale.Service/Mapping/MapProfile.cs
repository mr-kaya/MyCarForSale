using AutoMapper;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Service.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<BaseEntity, BaseEntityDto>().ReverseMap();
        CreateMap<CarFeaturesEntity, CarFeaturesEntityDto>().ReverseMap();
        CreateMap<CarImagesEntity, CarImagesEntityDto>().ReverseMap();
        CreateMap<MainClassificationEntity, MainClassificationEntityDto>().ReverseMap();
        CreateMap<UserAccountEntity, UserAccountEntityDto>().ReverseMap();
        CreateMap<UserFavoritesEntity, UserFavoritesEntityDto>().ReverseMap();
        CreateMap<CarFeaturesEntity, CarFeaturesWithMainClassDto>();
        CreateMap<CarFeaturesEntity, CarFeaturesWithImagesDto>();
        CreateMap<CarFeaturesEntity, CarFeaturesWithClassAndImagesDto>();
        //CreateMap<BaseEntity, BaseEntityWithCarImageEntityDto>();fsds
    }
}