using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AutoMapper;

namespace AudioBand.Settings.MappingProfiles
{
    public class SettingsProfileToUserProfile : Profile
    {
        public SettingsProfileToUserProfile()
        {
            CreateMap<ProfileV3, UserProfile>();
        }
    }
}
