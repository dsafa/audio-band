using System.Collections.Generic;
using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AudioBand.Settings.Models.v4;
using AutoMapper;

namespace AudioBand.Settings.MappingProfiles
{
    public class SettingsV3ToSettingsV4Profile : Profile
    {
        public SettingsV3ToSettingsV4Profile()
        {
            CreateMap<SettingsV3, SettingsV4>()
                .ForMember(dest => dest.AudioSource, opt => opt.MapFrom(src => src.AudioSource))
                .ForMember(dest => dest.CurrentProfileName, opt => opt.MapFrom(src => src.CurrentProfileName))
                .ForMember(dest => dest.AudioSourceSettings, opt => opt.MapFrom(src => src.AudioSourceSettings))
                .ForMember(dest => dest.Profiles, opt => opt.MapFrom(src => src.Profiles));
            CreateMap<KeyValuePair<string, ProfileV3>, UserProfile>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Key))
                .ForAllOtherMembers(opt => opt.MapFrom(src => src.Value));
        }
    }
}
