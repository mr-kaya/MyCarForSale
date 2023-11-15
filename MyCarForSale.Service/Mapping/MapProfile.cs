using AutoMapper;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Repository.Configurations;

namespace MyCarForSale.Service.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<BaseEntity, BaseEntityDto>().ReverseMap();
        CreateMap<CarFeaturesEntity, CarFeaturesEntityDto>().ReverseMap();
        CreateMap<CarImagesEntity, CarImagesEntityDto>().ReverseMap();
        CreateMap<CarImagesEntity, CarFeaturesWithClassAndImagesDto>();
        CreateMap<MainClassificationEntity, MainClassificationEntityDto>().ReverseMap();
        CreateMap<UserAccountEntity, UserAccountEntityDto>().ReverseMap();
        CreateMap<UserFavoritesEntity, UserFavoritesEntityDto>().ReverseMap();
        
        CreateMap<CarFeaturesEntity, CarFeaturesWithMainClassDto>();
        CreateMap<CarFeaturesEntity, CarFeaturesWithImagesDto>();
        CreateMap<CarFeaturesEntity, CarFeaturesWithClassAndImagesDto>().ReverseMap();
        //CreateMap<BaseEntity, BaseEntityWithCarImageEntityDto>();fsds
    }
}