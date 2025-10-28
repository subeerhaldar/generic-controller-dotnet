using AutoMapper;
using GenericController.DTOs;
using GenericController.Models;

namespace GenericController.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MstMarketingNote, MstMarketingNoteDto>().ReverseMap();
            CreateMap<MstLegalChecklist, MstLegalChecklistDto>().ReverseMap();
            CreateMap<MstCommercialChecklist, MstCommercialChecklistDto>().ReverseMap();
        }
    }
}