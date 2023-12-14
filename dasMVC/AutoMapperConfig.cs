using AutoMapper;
using dasMVC.Entities;
using dasMVC.Models;

namespace dasMVC
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<User,UserModel>().ReverseMap();
		}
	}
}
