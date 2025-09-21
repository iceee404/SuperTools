using System;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Printer, Printer>();
        CreateMap<TransferLog,TransferLog>();
    }
}
