using AutoMapper;
using DataAccessClassLibrary.EFModels;
using DataAccessClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessClassLibrary.Helpers
{
    public class MapperHelper
    {
        public MapperHelper()
        {

        }
        private MapperConfiguration GetConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Elections, ElectionModel>().ReverseMap();
                cfg.CreateMap<Admins, AdminModel>().ReverseMap();
                cfg.CreateMap<Candidates, CandidateModel>().ReverseMap();
                cfg.CreateMap<VotesOfElection, VoteModel>().ReverseMap();
            });
            return configuration;
        }
        public IMapper GetMapper()
        {
            var configuration = GetConfiguration();
            // only during development, validate your mappings; remove it before release
             //configuration.AssertConfigurationIsValid();
            // use DI(http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
